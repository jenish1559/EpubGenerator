using System.Diagnostics;
using System.IO;
using System;
using WebSupergoo.ABCpdf10;
using WebSupergoo.ABCpdf10.Objects;
using WebSupergoo.ABCpdf10.Atoms;
using Logger = EPubGenerator.Infrastructure.Logger;

namespace EPubGenerator.Process
{
    internal class PreProcessor
    {
        private readonly Logger _logger;
        private readonly string _hotFolderInputPath;
        public PreProcessor(Logger logger, string hotFolderInputPath)
        {
            _hotFolderInputPath = hotFolderInputPath;
            _logger = logger;
        }
        public void RemoveWatermarkPage(string inputFilePath, string tempDirectory)
        {

            try
            {
                //AbcPdf Object
                using (Doc pdf = new Doc())
                {
                    _logger.Log.Information("Doc object created.");
                    pdf.Read(inputFilePath);
                    int pageCount = pdf.PageCount;

                    _logger.Log.Information($"total Page Count : {pdf.PageCount}");



                    //this method remove waterMark   
                    RemoveWaterMark(pdf);

                    for (int i = 0; i < pageCount; i++)
                    {
                        pdf.PageNumber = i;
                        var pageText = pdf.GetText("Text");

                        if (pageText.ToLower().Contains("google"))
                        {
                            _logger.Log.Information($"removed page no : {i}");
                            pdf.Delete(pdf.Page);
                            
                        }
                    }

                    var fileName = Path.GetFileNameWithoutExtension(inputFilePath);
                    var newfilePath = Path.Combine(@"E:\EpubGenerator\PostProcessRelated\Pdfs", $"{fileName}_output.pdf");
                    //var newfilePath = Path.Combine(tempDirectory, $"{fileName}_output.pdf");
                    pdf.Save(newfilePath);   //TODo Check
                    _logger.Log.Information($"Pdf Save at path : {newfilePath}");

                    MovePdfToHotFolder(newfilePath);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{this.GetType().FullName} : {ex.ToString()}");
                _logger.Log.Error($"{this.GetType().FullName} : {ex.ToString()}");
            }

        }

        public void RemoveWaterMark(Doc pdf)
        {
            _logger.Log.Information("Start removing watermark.");
            foreach (IndirectObject objct in pdf.ObjectSoup)
            {

                if (objct != null && objct.Atom.GetType().Name.Equals("DictAtom") && objct.Atom.ToString().ToUpper().Contains("EXTGSTATE"))
                {
                    Atom atom = objct.Atom;
                    
                    int count = ((DictAtom)atom).Count;
                    DictAtom dicAtom = (DictAtom)atom;

                    if (dicAtom["ca"] != null)
                    {
                        float caNumber = float.Parse(dicAtom["ca"].ToString());

                        if (caNumber != 1f)
                        {
                            dicAtom.Remove("ca");
                            dicAtom.Add("ca", 0f);
                        }

                    }
                    objct.Dispose();
                }
            }

            _logger.Log.Information("watermark removed");
        }

        /// <summary>
        ///  filePath is souce file which we will move to hotfolder 
        /// </summary>
        /// <param name="filePath"></param>
        public void MovePdfToHotFolder(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var filename = Path.GetFileName(filePath);
                    File.Copy(filePath, Path.Combine(_hotFolderInputPath, filename), true);
                }
                else
                {
                    throw new Exception($"{this.GetType().FullName} : file not found at {filePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.Log.Error($"{this.GetType().FullName} : {ex.ToString()}");
            }
           
        }

    }
}

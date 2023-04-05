
using EPubGenerator.Model;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System;
using EPubGenerator.Infrastructure;

namespace EPubGenerator.Process
{
    internal class HotFolder
    {
        private readonly Logger _logger;
        private readonly string _taskFilePath = "D:\\EPubGenerator\\HotFolderTask.hft";
        private readonly string _HotFolderEXEPath = @"C:\Program Files (x86)\ABBYY FineReader 15\HotFolder.exe";
        private readonly string _hftFileConfugPath = @"C:\Users\Stark Solutions\AppData\Local\ABBYY\FineReader\15\HotFolder\Task.hft";
        private readonly string _hftSourcePath;
        private readonly string _hftOutputPath;
        public HotFolder(Logger logger,string hftSourcePath,string hftOutputPath)
        {
            _logger = logger;
            _hftSourcePath= hftSourcePath;
            _hftOutputPath= hftOutputPath;
        }

        public void EditHotFolderTask()
        {
            
            try
            {   //create directory if not exist
                if (!Directory.Exists(_hftSourcePath))
                    Directory.CreateDirectory(_hftSourcePath);

                if (!Directory.Exists(_hftOutputPath))
                    Directory.CreateDirectory(_hftOutputPath);
               

                string htfFilePath = Path.Combine(Path.GetTempPath(),"TaskDemo.hft");


                XmlSerializer serializer = new XmlSerializer(typeof(HfTask));
                HfTask hftobj = (HfTask)serializer.Deserialize(File.OpenRead(_taskFilePath));

                hftobj.Name = "DemoTask";   //Task Name
                hftobj.Status = "scheduled";   //Task Status
                hftobj.EngineTask.AddImages.FolderPath = _hftSourcePath;   //set Source Folder
                //for fast mode recognize
                hftobj.EngineTask.AnalyzeRecognize.BatchOptions.Ocr.Options = "OCRO_DetectTables,OCRO_DetectPictures,OCRO_AlwaysTrustPdfText,OCRO_UseBadEncodingDetector";
                                                                               
                //without fast mode 
                //hftobj.EngineTask.AnalyzeRecognize.BatchOptions.Ocr.Options = "OCRO_DetectTables,OCRO_DetectPictures,OCRO_UseOnlyPdfText";

                hftobj.EngineTask.SaveStepsCollection.Step.SavePath = _hftOutputPath;   //set output directory
                hftobj.EngineTask.SaveStepsCollection.Step.ExportName = "[F]"; // here [F] preset for original filename

                if (File.Exists(_hftFileConfugPath))
                {
                    File.Delete(_hftFileConfugPath);
                }

                StreamWriter stream = new StreamWriter(htfFilePath);
                serializer.Serialize(stream, hftobj);
                stream.Close();
               
                File.Move(htfFilePath, _hftFileConfugPath);

                RunHotFolder();
            }
            catch (Exception ex)
            {
                _logger.Log.Error(ex.ToString());
            }
        }

        /// <summary>
        /// hftPath should be "C:\Users\..\AppData\Local\ABBYY\FineReader\15\HotFolder\Task.hft"
        /// </summary>
        /// <param name="hftPath"></param>
        public void RunHotFolder()
        {
            if (File.Exists(_hftFileConfugPath))
            {
                Task.Factory.StartNew(() =>
                {
                    var process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = _HotFolderEXEPath;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    try
                    {
                        process.Start();
                        _logger.Log.Information("HotFolder.exe started.");
                        //process.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        _logger.Log.Error(ex.ToString());
                    }

                });

            }
            else
            {
                _logger.Log.Warning($"task file(.hft) not found at path : {_hftFileConfugPath}");
            }
        }

    }
}

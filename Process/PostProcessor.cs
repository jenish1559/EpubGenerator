
using EPubGenerator.Infrastructure;
using System;
using System.Drawing;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using EPubGenerator.Model;
using EpubSharp;
using ICSharpCode.SharpZipLib.Zip;
using EPubGenerator.Provider;
using WebSupergoo.ABCpdf10;
using WebSupergoo.ABCpdf10.Operations;
using Serilog;
using Newtonsoft.Json;
using System.Windows;


namespace EPubGenerator.Process
{

    public class PostProcessor
    {
        readonly IncorrectWordDetailsProvider _provider = new IncorrectWordDetailsProvider();
        private readonly Logger _logger;
        private readonly object _thisLock = new object();
        // Add your Computer Vision subscription key and endpoint
        static string subscriptionKey = "3ef39ab1c58c47dc92ec886d1aefee40";
        static string endpoint = "https://textreaderapp.cognitiveservices.azure.com/";
        private static string outputdirectory = "D:\\OutputOcr";
        private string pngDirPath = "C:\\Users\\Stark Solutions\\Downloads\\data\\data";
        private string inputDir = @"E:\EpubGenerator\PostProcessRelated\Input"; // folder that contain epubs
        private string pdfDir = @"E:\EpubGenerator\PostProcessRelated\Pdfs"; // folder that contain pdfs
        private string outputDir = @"E:\EpubGenerator\PostProcessRelated\Output";
        


        public PostProcessor(Logger logger)
        {
            _logger = logger;
        }

        
        public void GenerateBook()
        {
            var epubs = Directory.GetFiles(inputDir);
            var path =  epubs.First();
            //var path = Path.Combine(inputDir, "-6xYAAAAcAAJ.epub");
            string fileName = Path.GetFileNameWithoutExtension(path);
            string filePath = Path.Combine(outputDir, fileName);

            var pdfs = Directory.GetFiles(pdfDir);
            //string pdfPath = Path.Combine(pdfDir, "-6xYAAAAcAAJ.pdf");
            string pdfPath = pdfs.First();
            int totalWordsOfFile;
            List<WordDetail> wordList = new List<WordDetail>();

            // read generated word details file.
            if (File.Exists(filePath + "_WordDetails.txt"))
            {
                string wordDetailsFileText = null;
                using (StreamReader txtReader = new StreamReader(filePath + "_WordDetails.txt"))
                {
                    wordDetailsFileText = txtReader.ReadToEnd();
                    txtReader.Close();
                    txtReader.Dispose();
                }

                totalWordsOfFile = Convert.ToInt32(wordDetailsFileText.Split(new string[] { "(Total Words in File)" }, StringSplitOptions.None)[0]);

                var tempWordList = wordDetailsFileText.Split(new string[] { "\r\n" }, StringSplitOptions.None).Skip(1).ToList();
                foreach (var word in tempWordList)
                {
                    var wordInfo = word.Split(new string[] { "|||" }, StringSplitOptions.None);
                    if (wordInfo.Count() > 2)
                    {
                        if (wordInfo[1] != "  ")
                            wordList.Add(new WordDetail() { IncorrectWord = wordInfo[0].Trim(), CorrectWord = wordInfo[1].Trim(), WordImageName = wordInfo[2].Trim() });
                        else
                            wordList.Add(new WordDetail() { IncorrectWord = wordInfo[0].Trim(), CorrectWord = null, WordImageName = wordInfo[2].Trim() });
                    }
                }
            }
            else
            {
                EpubBook book = EpubReader.Read(path);

                var htmlText = book.Resources.Html;
                string plainText = null;
                book = null;

                // read html contain and generate plain text of book.
                foreach (var text in htmlText)
                {
                    var html = text.TextContent;
                    int startingRange = html.IndexOf("<style");
                    string newtext = html;
                    int EndingRange = 0;
                    if (startingRange != -1)
                    {
                        EndingRange = html.IndexOf("</style>") + 8;
                        newtext = html.Remove(startingRange, EndingRange - startingRange);
                    }
                    if (newtext.Contains("<!--"))
                    {
                        startingRange = newtext.IndexOf("<!--");
                        EndingRange = newtext.IndexOf("-->") + 3;
                        newtext = newtext.Remove(startingRange, EndingRange - startingRange);
                    }
                    plainText += Regex.Replace(newtext, "<.*?>", string.Empty);
                }
                htmlText.Clear();
                htmlText = null;

                plainText = WebUtility.HtmlDecode(plainText);
                plainText = plainText.Replace("This is a digital copy of a book that was preserved for generations on library shelves before it was carefully scanned by Google as part of a project to make the world's books discoverable online. See the back of the book for detailed information.", String.Empty);
                plainText = plainText.Replace("Google™ Book Search", String.Empty);
                var index = plainText.IndexOf("About this Book - From Google");
                if (index != -1)
                {
                    plainText = plainText.Remove(index, plainText.Length - index);
                }
                var bookWordsList = plainText.Split(new char[] { ' ', '\n', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                // File.WriteAllText(@"D:\jenika\20-09-2022project old\Epub_Generator\-6w9AQAAMAAJ_output1.epub", plainText);

                totalWordsOfFile = bookWordsList.Count();
                plainText = null;

                wordList = _provider.getIncorrectWordList(bookWordsList);
                bookWordsList.Clear();
                bookWordsList = null;

                if (wordList != null)
                {
                    string wordDetailsFileData = null;
                    string wordFileData = null;

                    foreach (var word in wordList.AsEnumerable().Reverse())
                    {
                        wordDetailsFileData = wordDetailsFileData + word.IncorrectWord + " ||| " + word.CorrectWord + " ||| " + word.WordImageName + "\r\n";

                        if (word.CorrectWord == null)
                            wordFileData = wordFileData + word.IncorrectWord + "\r\n";
                        else
                            wordFileData = wordFileData + word.IncorrectWord + " | " + word.CorrectWord + "\r\n";
                    }
                    using (StreamWriter txtWriter = new StreamWriter(filePath + ".txt"))
                    {
                        txtWriter.Write(wordFileData);
                        wordFileData = null;
                        txtWriter.Close();
                        txtWriter.Dispose();
                    }
                    using (StreamWriter fileWriter = new StreamWriter(filePath + "_WordDetails.txt"))
                    {
                        fileWriter.WriteLine(totalWordsOfFile + "(Total Words in File)");
                        fileWriter.Write(wordDetailsFileData);
                        wordDetailsFileData = null;
                        fileWriter.Close();
                        fileWriter.Dispose();
                    }
                    //Process.Start(filePath + ".txt");
                }
            }

            wordList = null;
            Directory.CreateDirectory(filePath);

            FastZip zip = new FastZip();
            zip.ExtractZip(path, filePath, null);
            zip = null;

            //var files = Directory.GetFiles(Path.Combine(filePath, "OEBPS\\Content"));
            var files = Directory.GetFiles(filePath, "*.Xhtml");

            // remove Google text from xml file.
            var firstfile = files.First();
            string firstfileText = File.ReadAllText(firstfile);
            var Endingrange = 0;
            var startingrange = firstfileText.IndexOf("This is a digital");
            if (startingrange != -1)
            {
                Endingrange = firstfileText.IndexOf("detailed information.") + 21;
                if (Endingrange == 20)
                {
                    Endingrange = firstfileText.IndexOf("</p>");
                }
                firstfileText = firstfileText.Remove(startingrange, Endingrange - startingrange);
            }
            startingrange = firstfileText.IndexOf("<div class='fakecover'>");
            if (startingrange != -1)
            {
                Endingrange = firstfileText.IndexOf("<div class='pagebreak'");
                if (Endingrange != -1)
                    firstfileText = firstfileText.Remove(startingrange, Endingrange - startingrange);
            }
            else
            {
                startingrange = firstfileText.IndexOf("<div class=\"fakecover\">");
                if (startingrange != -1)
                {
                    Endingrange = firstfileText.IndexOf("<div class=\"pagebreak\"");
                    if (Endingrange != -1)
                        firstfileText = firstfileText.Remove(startingrange, Endingrange - startingrange);
                }
            }
            File.WriteAllText(firstfile, firstfileText);

            var lastfile = files.Last();
            string lastFiletext = File.ReadAllText(lastfile);
            startingrange = lastFiletext.IndexOf("<head>") + 6;
            Endingrange = lastFiletext.IndexOf("</head>");

            var lastPageText = "<body>"
                + "Check Out More Titles From HardPress Classics Series In this collection we are offering thousands of classic and hard to find books. This series spans a vast array of subjects – so you are bound to find something of interest to enjoy reading and learning about.<br><br>"
                + "Subjects: <br>" + "Architecture <br>" + "Art <br>" + "Biography & Autobiography <br>"
                + "Body, Mind & Spirit <br>" + "Children & Young Adult<br>" + "Dramas<br>"
                + "Education<br>" + "Fiction<br>" + "History<br>" + "Language Arts & Disciplines <br>"
                + "Law <br>" + "Literary Collections<br>" + "Music<br>" + "Poetry<br>" + "Psychology<br>"
                + "Science <br>" + "…and many more.<br><br>" + "Visit us at<a> www.hardpress.net </a><br>"
                + "< img alt = \"Image\" src = \"LastPageImage.jpg\" />" + " </body>";

            lastFiletext = lastFiletext.Remove(startingrange, Endingrange - startingrange);
            startingrange = lastFiletext.IndexOf("<body>") + 6;
            Endingrange = lastFiletext.IndexOf("</body>");
            lastFiletext = lastFiletext.Remove(startingrange, Endingrange - startingrange);
            lastFiletext = lastFiletext.Insert(startingrange, lastPageText);
            File.WriteAllText(lastfile, lastFiletext);

            var imagePath = Path.Combine(filePath, "OEBPS\\data");
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
                _logger.Log.Information("LastPageImage.jpg add to ePub file.");
            }
            // var pdfPath = files.FirstOrDefault(a => a.ToLower().Contains(fileName.ToLower()));

            _logger.Log.Information(Path.GetFileNameWithoutExtension(imagePath) + " book GenerateIncorrectWordImage process start.");

            string fileText = null;
            using (StreamReader txtReader = new StreamReader(filePath + "_WordDetails.txt"))
            {
                fileText = txtReader.ReadToEnd();
                txtReader.Close();
                txtReader.Dispose();
            }                                                                                         

            var wordlist = fileText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            wordList = new List<WordDetail>();
            foreach (var word in wordlist)
            {
                var wordInfo = word.Split(new string[] { "|||" }, StringSplitOptions.None);
                if (wordInfo.Count() > 2)
                {
                    if (wordInfo[1] != "  ")
                        wordList.Add(new WordDetail() { IncorrectWord = wordInfo[0].Trim(), CorrectWord = wordInfo[1].Trim(), WordImageName = wordInfo[2].Trim() });
                    else
                        wordList.Add(new WordDetail() { IncorrectWord = wordInfo[0].Trim(), CorrectWord = null, WordImageName = wordInfo[2].Trim() });
                }
            }

            _provider.GenerateIncorrectWordImage(pdfPath, wordList, imagePath);

            List<WordInfo> newWordList = ConvertListToWordInfo(wordList);

            bool isMerge = MergeImages(newWordList, imagePath);
            if (isMerge)
            {
                string mergeImgPath = Directory.GetParent(imagePath).FullName;
                var pngs = Directory.GetFiles(mergeImgPath, "*.png");
                int index = 1;
                AzureOperation azureOperation = new AzureOperation(_logger);
                foreach (var png in pngs)
                {
                    //var png = pngs.First();
                    var url = azureOperation.UploadFile(png).ToString();
                    Task.Run(() => OcrOperation(url, mergeImgPath, index)).Wait();
                    azureOperation.RemoveFile(url);
                    index++;
                }

                newWordList = UpdateList(wordList, mergeImgPath);

            }

            var replaceWordList = newWordList.Where(a => a.CorrectWord != null && a.OcrText == null).ToList();
            var incorrectWordList = newWordList.Where(a => a.CorrectWord == null && a.OcrText == null).ToList();
            var ocrWordList = newWordList.Where(a => a.OcrText != null && a.CorrectWord == null).ToList();

            File.WriteAllText(Path.Combine(outputDir, "state.txt"), $"---- {fileName} ---- \n totalWordsOfFile : {totalWordsOfFile} \n totalfoundIncorectwords :{newWordList.Count()} \n wrongWordCount : {incorrectWordList.Count()} \n replaceWordBySpellCheckCount : {replaceWordList.Count ()} \n OcrWordCount : {ocrWordList.Count()}");
            for (int index = 0; index < files.Count() - 1; index++)
            {
                string text = File.ReadAllText(files[index]);
                string pTagText = ".flow p {";
                startingrange = text.IndexOf(pTagText);
                text = text.Insert(startingrange + pTagText.Length, "font-family: Georgia;");

                text = Regex.Replace(text, ".flow .gstxt_hlt {\n  background-color: yellow;\n}", ".flow .gstxt_hlt {\n  background-color: white;\n}", RegexOptions.IgnoreCase);
                text = Regex.Replace(text, ".flow .gstxt_underline {\n  text-decoration: underline;\n}", ".flow .gstxt_underline {\n  text-decoration: none;\n}", RegexOptions.IgnoreCase);


                foreach (var word in replaceWordList)
                {
                    string incorrectWord = string.Format(@"\b{0}\b", Regex.Escape(word.IncorrectWord));

                    if (word.CorrectWord != null && word.CorrectWord != "")
                    {
                        text = Regex.Replace(text, incorrectWord, word.CorrectWord.Replace("$", "$$"));
                    }
                }
                foreach (var word in incorrectWordList)
                {
                    //string incorrectWord = string.Format(@"\b{0}\b", Regex.Escape(word.IncorrectWord));
                    string incorrectWord = string.Format(@"(?<!\S){0}(?!\S)", Regex.Escape(word.IncorrectWord));

                    if (File.Exists(Path.Combine(imagePath, word.WordImageName + ".png")))
                    {
                        string correctString = "<img alt=\"Image\" src=\"" + "OEBPS/data/" + word.WordImageName + ".png" + "\" style=\"margin-bottom:-2px; height:1em;\" />";
                        text = Regex.Replace(text, incorrectWord, correctString.Replace("$", "$$"));
                    }
                }

                foreach (var word in ocrWordList)
                {
                    string incorrectWord = string.Format(@"\b{0}\b", Regex.Escape(word.IncorrectWord));

                    if (word.OcrText != null && word.OcrText != "" && word.Confidence > 0.6)
                    {
                        text = Regex.Replace(text, incorrectWord, word.OcrText.Replace("$", "$$"));
                    }
                    else
                    {
                        incorrectWord = string.Format(@"(?<!\S){0}(?!\S)", Regex.Escape(word.IncorrectWord));

                        if (File.Exists(Path.Combine(imagePath, word.WordImageName + ".png")))
                        {
                            string correctString = "<img alt=\"Image\" src=\"" + "OEBPS/data/" + word.WordImageName + ".png" + "\" style=\"margin-bottom:-2px; height:1em;\" />";
                            text = Regex.Replace(text, incorrectWord, correctString.Replace("$", "$$"));
                        }
                    }
                }

                File.WriteAllText(files[index], text);
            }

            files = null;
            // int totalfoundIncorectwords = newWordList.Count();
            // int wrongWordCount = newWordList.Count(a => a.CorrectWord == null);
            // int replaceWordCount = newWordList.Count(a => a.CorrectWord != null);
            // int ocrWordCount = newWordList.Count(a => a.OcrText != null);
            //wordList.Clear();
            //wordList = null;
            var lastPageImg = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "LastPageImage.jpg");
           
            
            if (File.Exists(lastPageImg))
            {
                File.Copy(lastPageImg, Path.Combine(filePath, "LastPageImage.jpg"));
            }
            
            zip = new FastZip();
            zip.CreateZip(filePath + ".epub", filePath, true, null);
            zip = null;

            //File.Delete(filePath + "_WordDetails.txt");

            //decimal wrongWordsInPercentage = Convert.ToDecimal(wrongWordCount * 100) / totalWordsOfFile;

            //lock (_thisLock)
            //{
            //  csvWriter.WriteLine(fileName + "," + totalWordsOfFile + "," + wrongWordCount + "," + replaceWordCount + "," + wrongWordsInPercentage);
           // File.WriteAllText(Path.Combine(filePath,"state.csv"), $"totalWordsOfFile : {totalWordsOfFile} \n totalfoundIncorectwords :{totalfoundIncorectwords}\n wrongWordCount : {wrongWordCount} \n replaceWordCount : {replaceWordCount} \n OcrWordCount : {ocrWordCount}");
            //}

            //string newEpubFile = filePath + ".epub";
            //string cmdResult = kindlegen.GenerateEpubFileUsingKindlegen(newEpubFile);

            //if (cmdResult.ToLower().Contains("error"))
            //{
            //    var errorMessage = " Error to convert " + Path.GetFileNameWithoutExtension(newEpubFile) + " File.\n";
            //    log.Error(Path.GetFileNameWithoutExtension(epubFile.FilePath) + " book Mobi file can not Generated.");
            //    log.Error(Path.GetFileNameWithoutExtension(newEpubFile) + " -> " + errorMessage);

            //    var mobifolderFiles = Directory.GetFiles(mobisFolderpath);
            //    if (mobifolderFiles.Any(a => a.Contains(epubFile.FileName)))
            //    {
            //        var fileList = mobifolderFiles.Where(a => a.Contains(epubFile.FileName));
            //        foreach (var file in fileList)
            //        {
            //            File.Move(file, Path.Combine(failedFolderPath, Path.GetFileName(file)));
            //        }
            //    }
            //}

            //epubFile.Status = true;
            //epubFile.EndingTime = DateTime.Now;
            //var timeDiff = epubFile.EndingTime.Subtract(epubFile.StartingTime);
            //epubFile.TimeTaken = timeDiff.Minutes.ToString() + " Minute " + timeDiff.Seconds.ToString() + " Seconds";

            //Directory.Delete(filePath, true);
        }


        //public void MeargeImages(List<WordDetail> wordDetails,string imgPath)
        //{
        //    List<WordDetail> wordList = new List<WordDetail>();
        //    foreach(WordDetail word in wordDetails)
        //    {
        //        if(word.WordImageName != null && word.WordImageName != "")
        //        {
        //            wordList.Add(word);
        //        }
        //    }

        //    var files = Directory.GetFiles(imgPath);

        //    var fileno = 1;

        //    var bitmap = new Bitmap(1080, 1920);
        //    try
        //    {
        //        Graphics graphics = Graphics.FromImage(bitmap);
        //        Pen redPen = new Pen(Color.Red, 1.5f);
        //        Pen blackPen = new Pen(Color.Black, 2);
        //        // Draw a rectangle on the bitmap (for background color)
        //        graphics.FillRectangle(Brushes.Yellow, 0, 0, 1080, 1920);

        //        using (var g = Graphics.FromImage(bitmap))
        //        {
        //            int margintop = 10;
        //            int marginleft = 20;

        //            int maxheight = 0;
        //            foreach (WordDetail wordDetail in wordList.OrderBy(w => w.IncorrectWord))
        //            {
        //                if (!string.IsNullOrEmpty(wordDetail.WordImageName) && !string.IsNullOrWhiteSpace(wordDetail.WordImageName))
        //                {
        //                    foreach (var image in files)
        //                    {
        //                        if (image.Contains("content")) continue;

        //                        if (Path.GetFileNameWithoutExtension(image) == wordDetail.WordImageName)
        //                        {
        //                            var bmp = Image.FromFile(image);
        //                            maxheight = Math.Max(maxheight, bmp.Height);
        //                            if (margintop + bmp.Height > 1920)
        //                            {
        //                                maxheight = 0;
        //                                //bitmap.Save($"C:\\Users\\Stark Solutions\\Downloads\\data\\img{fileno}.png", System.Drawing.Imaging.ImageFormat.Png);
        //                                bitmap.Save(Path.Combine(Directory.GetParent(imgPath).Name, $"img{fileno}.png"), System.Drawing.Imaging.ImageFormat.Png);
        //                                fileno++;
        //                                margintop = 10;
        //                                marginleft = 20;
        //                            }
        //                            //check if 
        //                            if (marginleft + bmp.Width > 1080)
        //                            {
        //                                margintop += maxheight + 5;
        //                                marginleft = 20;
        //                                g.DrawLine(blackPen, 0, margintop + 2, 1080, margintop + 2);
        //                                margintop += 2;

        //                            }


        //                            if (maxheight != 0 && bmp.Height + 3 < maxheight)
        //                            {
        //                                var addmargin = ((maxheight - bmp.Height) / 2) - 1;
        //                                g.DrawImage(bmp, marginleft, margintop + addmargin, bmp.Width, bmp.Height);
        //                                // Create rectangle.
        //                                Rectangle rect = new Rectangle(marginleft, margintop + addmargin, bmp.Width, bmp.Height);
        //                                // g.DrawRectangle(redPen, rect);
        //                            }
        //                            else
        //                            {
        //                                g.DrawImage(bmp, marginleft, margintop, bmp.Width, bmp.Height);
        //                                // Create rectangle.
        //                                Rectangle rect = new Rectangle(marginleft, margintop, bmp.Width, bmp.Height);
        //                                //g.DrawRectangle(redPen, rect);
        //                            }
        //                            marginleft += 20 + bmp.Width;
        //                            //bitmap.Save($"C:\\Users\\Stark Solutions\\Downloads\\data\\img{fileno}.png", System.Drawing.Imaging.ImageFormat.Png);
        //                        }

        //                    }

        //                }
        //            }
        //            bitmap.Save(Path.Combine(Directory.GetParent(imgPath).FullName, $"img{fileno}.png"), System.Drawing.Imaging.ImageFormat.Png);

        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }


        //}

        public bool MergeImages(List<WordInfo> wordList, string imgDir)
        {

            try
            {
                //Step 1 : get all img file from directory and make reqiured objects
                #region Step 1 files, fileno, bitmap, graphics, redPen, blackPen
                var files = Directory.GetFiles(imgDir);
                var fileno = 1;
                int frameHeight = 3508;
                int frameWidth = 2480;
                var bitmap = new Bitmap(frameWidth, frameHeight);
                Graphics graphics = Graphics.FromImage(bitmap);
                Pen redPen = new Pen(Color.Red, 1.5f);
                Pen blackPen = new Pen(Color.Black, 2);
                #endregion


                #region Step 2 Draw frame(for background) and add word img on frame
                // Draw a rectangle on the bitmap (for background color)
                graphics.FillRectangle(Brushes.Yellow, 0, 0, frameWidth, frameHeight);
                using (var g = Graphics.FromImage(bitmap))
                {
                    int margintop = 10;
                    int marginleft = 20;
                    int maxheight = 0;
                    int lineNo = 0;
                    int index = 0;
                    int count = 0;
                    foreach (WordInfo wordDetail in wordList.OrderBy(w => w.IncorrectWord))
                    {

                        foreach (var image in files)
                        {
                            if (image.Contains("content")) continue;

                            try
                            {
                                if (wordDetail.IncorrectWord != null && wordDetail.IncorrectWord != "")
                                {
                                    if (Path.GetFileNameWithoutExtension(image) == wordDetail.WordImageName)
                                    {
                                        var bmp = Image.FromFile(image);
                                        wordDetail.ImgWidth = bmp.Width;
                                        wordDetail.ImgHeight = bmp.Height;
                                        wordDetail.FileNo = fileno;
                                        maxheight = Math.Max(maxheight, bmp.Height);

                                        if (marginleft == 20)
                                        {
                                            g.DrawString("NEWLINE", new Font("Arial Bold", 25, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, marginleft, margintop);
                                            marginleft += 150;
                                        }

                                        //condition for new line
                                        if (marginleft + bmp.Width + 20 > frameWidth) // extra 20 for right margin.
                                        {
                                            lineNo++;
                                            index = 0;
                                            margintop = maxheight + margintop + 10;  // extra 10 for line margin.
                                            marginleft = 20;
                                            //maxheight = 0;
                                            g.DrawString("NEWLINE", new Font("Arial Bold", 25, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, marginleft, margintop);
                                            marginleft += 150;
                                            maxheight = Math.Max(0, bmp.Height);
                                        }
                                        //condition for new img 
                                        if (margintop + bmp.Height + 10 > frameHeight)
                                        {
                                            bitmap.Save(Path.Combine(Directory.GetParent(imgDir).FullName, $"img{fileno}.png"), System.Drawing.Imaging.ImageFormat.Png);
                                            fileno++;
                                            lineNo = 0;
                                            index = 0;
                                            margintop = 10;
                                            marginleft = 20;
                                            graphics.FillRectangle(Brushes.Yellow, 0, 0, frameWidth, frameHeight);
                                            g.DrawString("NEWLINE", new Font("Arial Bold", 25, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, marginleft, margintop);
                                            marginleft += 150;
                                            maxheight = Math.Max(0, bmp.Height);
                                        }

                                        wordDetail.PosX = marginleft;
                                        wordDetail.PosY = margintop;
                                        wordDetail.LineNo = lineNo;
                                        wordDetail.index = index;
                                        g.DrawImage(bmp, marginleft, margintop, bmp.Width, bmp.Height);
                                        marginleft += 20 + bmp.Width;
                                        index++;
                                        count++;
                                        break;
                                    }
                                }
                                
                            }
                            catch
                            {
                                throw;
                            }

                        }

                    }
                    _logger.Log.Information($"Merge img count : {count}");
                    bitmap.Save(Path.Combine(Directory.GetParent(imgDir).FullName, $"img{fileno}.png"), System.Drawing.Imaging.ImageFormat.Png);
                    var wordListJson = JsonConvert.SerializeObject(wordList.OrderBy(x => x.IncorrectWord), Formatting.Indented);
                    File.WriteAllText(Path.Combine(Directory.GetParent(imgDir).FullName, "WordDetails.json"), wordListJson);
                }
                #endregion
                return true;

            }
            catch (Exception ex)
            {
                _logger.Log.Error($" {this.GetType()}: {ex.ToString()}");
                MessageBox.Show($" {this.GetType()}: {ex.Message}");
            }

            return false;
        }

      
        //update list as per Ocrtext (mapping)
        public List<WordInfo> UpdateList(List<WordDetail> wordList, string imgDir)
        {

            var  ocroutputfile = Directory.GetFiles(Path.Combine(imgDir, "OcrResults"),"*.json");
            string ocrResult = string.Empty;
            foreach (var ocroutput in ocroutputfile)
            {
                var text = File.ReadAllText(ocroutput);
                ocrResult += text;
            }

            string listFilePath = Path.Combine(imgDir, "WordDetails.Json");
            string jsonText = File.ReadAllText(listFilePath);
           
            List<WordInfo> imgWordList = JsonConvert.DeserializeObject<List<WordInfo>>(jsonText);
            var ocrObj = JsonConvert.DeserializeObject<List<ReadResult>>(ocrResult);
            List<Word> ocrWords = new List<Word>();

            foreach (ReadResult page in ocrObj)
            {
                foreach (var line in page.Lines)
                {
                    ocrWords.AddRange(line.Words);
                }
            }

          
            int index = 0;
            int lineNo = -1;
            try
            {
                foreach (var word in ocrWords)
                {
                    if (word.Text.ToLower() == "newline")
                    {
                        index = 0;
                        lineNo++;
                        continue;
                    }

                    foreach (var img in imgWordList)
                    {
                        if (img.LineNo == lineNo && img.index == index && img.ImgWidth > 0)
                        {
                            img.OcrText = word.Text;
                            img.Confidence = word.Confidence;
                            index++;
                            break;
                        }
                    }
                }
               
                File.WriteAllText(Path.Combine(imgDir, "newWordDetails.Json"), JsonConvert.SerializeObject(imgWordList));
                return imgWordList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($" {this.GetType().FullName}: {ex.ToString()}");
            }
            return null;
        }

        #region ocr related methods
        public void OcrOperation(string imgUrl, string destinationPath,int index)
        {
            try
            {
                ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

                ReadFileUrl(client, imgUrl, destinationPath,index);
                

            }
            catch (Exception ex)
            {
                _logger.Log.Error(ex.ToString());
            }
        }

        public  Task ReadFileUrl(ComputerVisionClient client, string urlFile, string destinationPath,int index)
        {

            _logger.Log.Information("READ FILE FROM URL");
            try
            {

                var textHeaders = client.ReadAsync(urlFile);
                // After the request, get the operation location (operation ID)
                string operationLocation = textHeaders.Result.OperationLocation;
                Thread.Sleep(2000);
                

                // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
                // We only need the ID and not the full URL
                const int numberOfCharsInOperationId = 36;
                string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

                // Extract the text
                ReadOperationResult results;
                _logger.Log.Information($"Extracting text from URL file {Path.GetFileName(urlFile)}...");

                do
                {
                    results = client.GetReadResultAsync(Guid.Parse(operationId)).Result;
                }
                while ((results.Status == OperationStatusCodes.Running ||
                    results.Status == OperationStatusCodes.NotStarted));


                var textUrlFileResults = results.AnalyzeResult.ReadResults;
                var jsonResult = JsonConvert.SerializeObject(textUrlFileResults, Formatting.Indented);
                var result = Path.Combine(destinationPath, "OcrResults");
                if (!Directory.Exists(result))
                    Directory.CreateDirectory(result);
                File.WriteAllText(Path.Combine(result, $"OcrResult_{index}.json"), jsonResult);

                //foreach (ReadResult page in textUrlFileResults)
                //{
                //    var str = JsonConvert.SerializeObject(page, Formatting.Indented);
                //}
                _logger.Log.Information($"ocr result file path : {destinationPath}\\OcrResult.json");
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($" {this.GetType()}: {ex.Message}");
            }
            return Task.CompletedTask;
        }

        public ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        #endregion

        private List<WordInfo> ConvertListToWordInfo(List<WordDetail> wordList)
        {
            try
            {
                List<WordInfo> imgWordList = new List<WordInfo>();
                foreach (WordDetail word in wordList)
                {
                    if (string.IsNullOrEmpty(word.IncorrectWord)) continue;
                    imgWordList.Add(new WordInfo
                    {
                        CorrectWord = word.CorrectWord,
                        IncorrectWord = word.IncorrectWord,
                        WordImageName = word.WordImageName,
                    });
                }
                return imgWordList;
            }
            catch
            {
                throw;
            }

        }
    }
}

using EPubGenerator.Command;
using EPubGenerator.Process;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using EPubGenerator.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using EPubGenerator.Provider;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using Serilog.Sinks.File;
using System.Windows.Markup.Localizer;

namespace EPubGenerator.ViewModel
{
    public class MainWindowVM : Observable
    {
        private Logger logger;
        private readonly string _hotFolderEXEPath = @"C:\Program Files (x86)\ABBYY FineReader 15\HotFolder.exe";
        private readonly string _hftSourcePath = @"E:\EpubGenerator\HotFolder";
        private readonly string _hftOutputPath = @"E:\EpubGenerator\HotFolder\OutPut";
        private FileSystemWatcher watcher;
        Stopwatch stopwatch;


        #region Constructor 
        public MainWindowVM()
        {
            logger = new Logger(@"D:\EPubGenerator\log.txt");
            ProcessCommand = new RelayCommand(EpubProcess);
        }

        #endregion

        #region Commands
        public ICommand ProcessCommand { get; set; }

        #endregion

        #region Properties
        private string _epubInputPath = @"E:\EpubGenerator\Pdfs";
        public string EpubInputPath
        {
            get { return _epubInputPath; }
            set
            {
                if (_epubInputPath == value) return;
                _epubInputPath = value;
                OnPropertyChanged(nameof(EpubInputPath));
            }
        }

        #endregion

        public async void EpubProcess(Object obj)
        {
            //string fileName = Path.GetFileNameWithoutExtension(EpubInputPath);
            //string tempPath = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), fileName)).FullName;

            watcher = new FileSystemWatcher();
            try
            {
                MessageBox.Show("Process Start.", "Info");

                if (!IsHotFolderRunning())
                    HotFolderProcessing();
                else
                    logger.Log.Information("HotFolder.exe is already running.");

                await FileWatch();

                ///temp code for loop
                var files = Directory.GetFiles(_epubInputPath);
                // foreach (var item in files)
                // {
                var item = files.First();
                string fileName = Path.GetFileNameWithoutExtension(item);
                string tempPath = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), fileName)).FullName;
                stopwatch = new Stopwatch();
                stopwatch.Start();
                await PreProcessing(fileName, item, tempPath);
                logger.Log.Information($"file : {fileName} , Pre-Process Time : {stopwatch.Elapsed}");
                // await PostProcessing();


                //string newFileName = $"{fileName}_output.pdf";
                // File.Copy(Path.Combine(tempPath, newFileName), Path.Combine(_hftSourcePath, newFileName));

                // }


            }
            catch (Exception ex)
            {
                logger.Log.Error(ex.ToString());
                MessageBox.Show($" {this.GetType().FullName}: {ex.Message}");
            }

        }

        public Task PreProcessing(string fileName, string filePath, string tempPath)
        {
            try
            {
                logger.Log.Information("Pre-Process Start.");
                logger.Log.Information($"FileName : {fileName}");
                PreProcessor preProcessor = new PreProcessor(logger, _hftSourcePath);
                preProcessor.RemoveWatermarkPage(filePath, tempPath);
                logger.Log.Information("Pre-Process End.");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void HotFolderProcessing()
        {
            try
            {
                logger.Log.Information("Hotfolder-Process Start.");
                //logger.Log.Information($"FileName : {fileName}");

                HotFolder process = new HotFolder(logger, _hftSourcePath, _hftOutputPath);
                process.EditHotFolderTask();
                logger.Log.Information("Hotfolder-Process End.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsHotFolderRunning()
        {

            string FileName = Path.GetFileNameWithoutExtension(_hotFolderEXEPath).ToLower();
            bool isRunning = false;

            System.Diagnostics.Process[] pList = System.Diagnostics.Process.GetProcessesByName(FileName);

            foreach (System.Diagnostics.Process p in pList)
            {
                if (p.MainModule.FileName.StartsWith(_hotFolderEXEPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    isRunning = true;
                    break;
                }
            }

            return isRunning;
        }

        public Task PostProcessing(string fileName)
        {
            try
            {


                logger.Log.Information("Post-Process Start.");
                //logger.Log.Information($"FileName : {fileName}");
                PostProcessor postProcessor = new PostProcessor(logger);
                postProcessor.GenerateBook();
                //AzureOperation azureOperation = new AzureOperation(logger);
                //azureOperation.UploadFile("E:\\EpubGenerator\\PostProcessRelated\\Output\\-6xYAAAAcAAJ\\OEBPS\\img1.png");
                //postProcessor.OcrOperation("https://batchformattertest.blob.core.windows.net/batchformattertest/imgwithbreak.png");
                //postProcessor.MeargeImages();
                logger.Log.Information("Post-Process End.");
                stopwatch.Stop();
                logger.Log.Information($"file : {fileName} , Pre-Process Time : {stopwatch.Elapsed}");

            }
            catch (Exception ex)
            {
                logger.Log.Error(ex.ToString());
                MessageBox.Show($" {this.GetType().FullName}: {ex.Message}");
            }
            return Task.CompletedTask;
        }



        public Task FileWatch()
        {
            watcher.Path = _hftOutputPath; // set the directory to monitor
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName; // only monitor changes to file content
            watcher.Filter = "*.epub"; // only monitor text files
            watcher.EnableRaisingEvents = true;  // enable events to be raised

            watcher.Created += new FileSystemEventHandler(OnCreated);
            return Task.CompletedTask;
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {


            try
            {
                logger.Log.Information($"file : {e.Name} creted. time : {stopwatch.Elapsed}");
                string filename = e.Name.Replace("_output.epub", "");
                string epubpath = Path.Combine(Path.GetTempPath(), filename, e.Name);
                var outputpath = @"E:\EpubGenerator\PostProcessRelated\Input\" + e.Name;

                //long fileSize = 0;
                //FileInfo currentFile = new FileInfo(e.FullPath);
                //while (fileSize < currentFile.Length)//check size is stable or increased
                //{
                //    fileSize = currentFile.Length;//get current size
                //    System.Threading.Thread.Sleep(500);//wait a moment for processing copy
                //    currentFile.Refresh();//refresh length value
                //}
                Task.Delay(10000).Wait();
                while (true)
                {
                    try
                    {
                        File.Copy(e.FullPath, outputpath, true);
                        break;
                    }
                    catch
                    {

                    }
                    
                }
               
                logger.Log.Information($"file : {e.Name} copy at {outputpath}; time : {stopwatch.Elapsed}");
                PostProcessing(e.Name);
                watcher.Created -= new FileSystemEventHandler(OnCreated);
            }
            catch (Exception ex)
            {
                logger.Log.Error(ex.Message);

            }



            //File.Delete(e.FullPath);
        }
    }
}

using Azure.Storage.Blobs;
using EPubGenerator.Infrastructure;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EPubGenerator.Provider
{
    public class AzureOperation
    {
        private const string StorageAccountName = "batchformattertest";
        private const string StorageAccountKey = "OeUdZus9N8aLhVIJy3XBrQYO+qQWT5pvE9776zZE9uVmxJr1WJcHWy4G7hHaGomq9RoyiBLy2LoUybCG2OCynQ==";
        private const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=" + StorageAccountName + ";AccountKey=" + StorageAccountKey;
        private const string subscriptionKey = "3ef39ab1c58c47dc92ec886d1aefee40";
        private const string endpoint = "https://textreaderapp.cognitiveservices.azure.com/";
        private Logger _logger;
        public AzureOperation(Logger logger)
        {
            _logger = logger;
        }


        public  string UploadFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new Exception($"{this.GetType().FullName} : file not found at {filePath}");
                var fileName = Path.GetFileName(filePath);
                BlobServiceClient Client = new BlobServiceClient(storageConnectionString);
                BlobContainerClient containerClient = Client.GetBlobContainerClient(StorageAccountName);
                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                var uploadResult = blobClient.UploadAsync(filePath, true).Result;
                

                //while (!uploadResult.GetRawResponse().IsError)
                //{
                //    Task.Delay(2000).Wait();
                //}

                if (!uploadResult.GetRawResponse().IsError)
                {
                    var url = blobClient.Uri.ToString();
                    return url;
                }
            }
            catch (Exception ex)
            {
                _logger.Log.Error(ex.ToString());
            }
            return null;
        }

        public void GetOcrResult(string imgUrl, string destinationPath)
        {
            try
            {
                ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

                ReadFileUrl(client, imgUrl, destinationPath);


            }
            catch (Exception ex)
            {
                _logger.Log.Error(ex.ToString());
            }
        }
        public Task ReadFileUrl(ComputerVisionClient client, string urlFile, string destinationPath)
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

                var fileName = Path.GetFileNameWithoutExtension(urlFile);
                var textUrlFileResults = results.AnalyzeResult.ReadResults;
                var jsonResult = JsonConvert.SerializeObject(textUrlFileResults, Formatting.Indented);
                File.WriteAllText(Path.Combine(destinationPath, $"{fileName}.json"), jsonResult);

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

        public void RemoveFile(string path)
        {
            try
            {
                var fileName = Path.GetFileName(path);
                BlobServiceClient Client = new BlobServiceClient(storageConnectionString);
                BlobContainerClient containerClient = Client.GetBlobContainerClient(StorageAccountName);
                containerClient.GetBlobClient(fileName).DeleteIfExists();
            }
            catch(Exception ex)
            {
                _logger.Log.Error(ex.ToString()); 
            }
           
        }
    }
}

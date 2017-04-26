using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;


namespace AzureBlobUploader
{
    class Program
    {
        static void Main(string[] args)
        {

            //引数が足りない場合終了
            if (args.Length != 5) {
                Console.WriteLine("Wrong number of arguments");
                HelpAndExit();
            }

            //パラメーター取得
            string accountName = args[0];
            string accessKey = args[1];
            string containerName = args[2];
            string localFilePath = args[3];
            string blobPath = args[4];

            //Upload
            AzureBlobUpload(accountName, accessKey, containerName, localFilePath, blobPath);
            
        }

        static void AzureBlobUpload(string accountName, string accessKey, string containerName, string localFilePath, string blobPath)
        {
            try
            {
                //Initialize
                var credential = new StorageCredentials(accountName, accessKey);
                var storageAccount = new CloudStorageAccount(credential, true);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            
                //Upload
                CloudBlockBlob blockBlob_upload = container.GetBlockBlobReference(blobPath);
                using (var fileStream = System.IO.File.OpenRead(localFilePath))
                {
                    blockBlob_upload.UploadFromStream(fileStream);
                }
            }
            catch (Exception ex)
            {
                //エラーメッセージ表示
                Console.WriteLine("Error: " + ex.Message);
                //エラーコード1を出力して終了
                Environment.Exit(1);
            }
            //正常終了時
            Console.WriteLine("File uploaded");
            Environment.Exit(0);

        }

        /// <summary>
        /// Help表示して終了
        /// </summary>
        static void HelpAndExit()
        {
            string help = @"
AzureBlobUploader.exe
https://github.com/from2001/AzureBlobUploader
AzureBlobUploader.exe accountName accessKey containerName localFilePath blobPath
accountName      Blob account name
accessKey        Blob accessKey
containerName    Blob container name
localFilePath    File path to upload 
blobPath         Blob path. (ie.  fol1/filename.ext for file upload)
";
            Console.WriteLine(help);

            //エラーコード2を出力して終了
            Environment.Exit(2);


        }


    }
}




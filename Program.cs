﻿using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace S3CLI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string url = "";
            string bucket = "";
            string key = "";
            string secret = "";
            string filepath = "";
            string s3path = "";

            int counter = 0;
            foreach (var arg in args)
            {
                switch (arg.ToUpper())
                {
                    case "-URL":
                        url = args[counter + 1];
                        break;
                    case "-BUCKET":
                        bucket = args[counter + 1];
                        break;
                    case "-KEY":
                        key = args[counter + 1];
                        break;
                    case "-SECRET":
                        secret = args[counter + 1];
                        break;
                    case "-FILE":
                        filepath = args[counter + 1];
                        break;
                    case "-S3PATH":
                        s3path = args[counter + 1];
                        break;
                    case "-HELP":
                        Console.Write(generateHelpOutput());
                        return;
                    default:
                        break;
                }
                counter++;
            }

            if (!url.Contains("https://"))
            {
                Console.WriteLine("URL must include https://");
                url = "";
            }

            if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(bucket) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(secret))
            {
                if (string.IsNullOrEmpty(s3path))
                {
                    s3path = "/";
                }

                try
                {
                    await UploadFileToS3(filepath, url, bucket, key, secret, s3path);
                    Console.WriteLine($"Uploaded File: {filepath}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
                
            }
            else
            {
                Console.WriteLine("Not all parameters were provided. Required: -url, -bucket, -key, -secret, -file\nUse -help for usage");
            }
        }

        private static string ToS3Path(string NTFSPath)
        {
            return NTFSPath.Replace("\\", "/").Replace("//", "/");
        }

        private static async Task UploadFileToS3(string filePath, string s3Host, string s3BucketName, string s3AccessKey, string s3SecureKey, string s3Path = "/")
        {
            var config = new AmazonS3Config { ServiceURL = s3Host };
            var s3Credentials = new BasicAWSCredentials(s3AccessKey, s3SecureKey);
            AmazonS3Client s3Client = new AmazonS3Client(s3Credentials, config);

            using (var objectDataStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                Debug.WriteLine($"{filePath}");

                PutObjectRequest request;
                request = new PutObjectRequest
                {
                    BucketName = s3BucketName,
                    Key = ToS3Path(string.Concat(s3Path, "/", Path.GetFileName(filePath))),
                    InputStream = objectDataStream
                };

                try
                {
                    await s3Client.PutObjectAsync(request);
                }
                catch (Exception putException)
                {
                    throw new Exception($"Upload Failed {filePath} ({putException.Message})");
                }
            }

        }

        private static string generateHelpOutput() {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\n\nS3CLI Help");
            sb.AppendLine("===========================");
            sb.AppendLine("This CLI utility can be used in automation systems to upload a given file to an S3 bucket");
            sb.AppendLine("This is an open source project. See https://github.com/c0der4t/S3CLI");
            sb.AppendLine();
            sb.AppendLine("Usage:");
            sb.AppendLine(".\\S3CLI.exe -bucket testbucket -url \"https://s3.eu-west-1.wasabisys.com\" -key aaaaaaaaaaaaa -secret bbbbbbbbbbbbbb -file \"C:\\temp\\test.txt\" [-s3Path testfolder/]");
            sb.AppendLine("\tbucket\tThe name of the bucket to target in the upload");
            sb.AppendLine("\turl\tThe endpoint of your S3 provider");
            sb.AppendLine("\tkey\tThe API user key for your s3 provider");
            sb.AppendLine("\tsecret\tThe API secret key for your s3 provider");
            sb.AppendLine("\tfile\tThe full path to the file you would like to upload (folders are not supported yet)");
            sb.AppendLine("\ts3Path\t[OPTIONAL] If you'd like to put the file in a specific location in S3, define it here. This defaults to '/'");
            sb.AppendLine("\n\n");
            return sb.ToString();
        }

    }


}

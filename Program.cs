using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;

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

              await UploadFileToS3(filepath, url, bucket, key, secret, s3path);
              Console.WriteLine("Uploaded File");
            }
            else
            {
                Console.WriteLine("Not all parameters were provided. Required: URL, BUCKET, KEY, SECRET, FILE");
            }
        }

        private static string ToS3Path(string NTFSPath)
        {
            return NTFSPath.Replace("\\", "/").Replace("//","/");
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

                    Debug.WriteLine($"Uploaded {filePath}");
                }
                catch (Exception putException)
                {
                    Debug.WriteLine($"Upload Failed {filePath}" +
                        $" ({putException.Message})");
                }
            }

        }

    }


}

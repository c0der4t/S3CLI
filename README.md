# S3CLI

This CLI utility can be used in automation systems to upload a given file to an S3 bucket

## ⚠️ Security Considerations

- Reference the "Usage" section: The API key and secret is passed to the S3 CLI utility in **PLAIN TEXT**. This is fine for on demand requests, however this presents issues if used in a system that logs the command in plain text as well.
- This utility does not log API credentials anywhere.
- This utility uses the external Amazon S3 SDK to interact with S3 storage.

## Usage

.\S3CLI.exe -bucket testbucket -url "https://s3.eu-west-1.wasabisys.com" -key aaaaaaaaaaaaa -secret bbbbbbbbbbbbbb -file "C:\temp\test.txt" [-s3Path testfolder/]

        bucket  The name of the bucket to target in the upload

        url     The endpoint of your S3 provider

        key     The API user key for your s3 provider

        secret  The API secret key for your s3 provider

        file    The full path to the file you would like to upload (folders are not supported yet)

        s3Path  [OPTIONAL] If you'd like to put the file in a specific location in S3, define it here. This defaults to '/'

## Prerequisites

- This application requires .NET Runtime 8.0. Download it [here](https://download.visualstudio.microsoft.com/download/pr/3980ab0a-379f-44a0-9be6-eaf74c07a3b3/bd1cc6107ff3d8fe0104d30f01339b74/dotnet-runtime-8.0.7-win-x64.exe)
- You must have S3 compatible storage such as Amazon S3 or Wasabi S3
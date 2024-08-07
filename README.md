# S3CLI

This CLI utility can be used in automation systems to upload a given file to an S3 bucket

Usage:
.\S3CLI.exe -bucket testbucket -url "https://s3.eu-west-1.wasabisys.com" -key aaaaaaaaaaaaa -secret bbbbbbbbbbbbbb -file "C:\temp\test.txt" [-s3Path testfolder/]
        bucket  The name of the bucket to target in the upload
        url     The endpoint of your S3 provider
        key     The API user key for your s3 provider
        secret  The API secret key for your s3 provider
        file    The full path to the file you would like to upload (folders are not supported yet)
        s3Path  [OPTIONAL] If you'd like to put the file in a specific location in S3, define it here. This defaults to '/'
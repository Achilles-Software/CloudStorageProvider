using Achilles.Acme.Storage.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Achilles.Acme.Storage.AWS
{
    public class AwsCloudStorageProvider : CloudStorageProvider
    {
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
        }

        public override string ContainerName
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CreateDirectory( string path )
        {
            throw new NotImplementedException();
        }

        public override bool DirectoryExists( string path )
        {
            throw new NotImplementedException();
        }

        public override void DeleteDirectory( string path, bool recursive )
        {
            throw new NotImplementedException();
        }

        public override void MoveDirectory( string newPath, string oldPath )
        {
            throw new NotImplementedException();
        }

        public override IO.DirectoryInfo[] GetDirectories( IO.DirectoryInfo dirInfo )
        {
            throw new NotImplementedException();
        }

        public override string[] GetDirectories( string path )
        {
            throw new NotImplementedException();
        }

        public override IO.FileInfo[] GetDirectoryFiles( IO.DirectoryInfo dirInfo )
        {
            throw new NotImplementedException();
        }

        public override string[] GetDirectoryFiles( string path )
        {
            throw new NotImplementedException();
        }

        public override IO.FileAttributes FileAttributes( string path )
        {
            throw new NotImplementedException();
        }

        public override bool FileExists( string path )
        {
            throw new NotImplementedException();
        }

        public override void DeleteFile( string path )
        {
            throw new NotImplementedException();
        }

        public override System.IO.Stream FileOpenRead( string path )
        {
            throw new NotImplementedException();
        }

        public override System.IO.Stream FileOpenWrite( string path )
        {
            throw new NotImplementedException();
        }

        public override void CopyFile( string sourcePath, string destPath )
        {
            throw new NotImplementedException();
        }

        public override void MoveFile( string sourceFileName, string destFileName )
        {
            throw new NotImplementedException();
        }
        public override void FileUploadFromStream( string path, System.IO.Stream inputStream )
        {
            throw new NotImplementedException();
        }
    }
}
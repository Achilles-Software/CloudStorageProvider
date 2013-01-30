#region Copyright Notice

// Copyright (c) 2013 by Achilles Software, http://achilles-software.com
//
// The source code contained in this file may not be copied, modified, distributed or
// published by any means without the express written agreement by Achilles Software.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com
//
// All rights reserved.

//
// Thanks to Richard Parker for his open source FTP to Azure Blob Storage Bridge.
// See: http://ftp2azure.codeplex.com
//

#endregion

#region Namespaces

using Achilles.Acme.Storage.IO;
using Achilles.Acme.Storage.Provider;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

#endregion

namespace Achilles.Acme.Storage.Azure
{
    /// <summary>
    /// Azure specific implementation for the CloudStorageProvider.
    /// </summary>
    public class AzureCloudStorageProvider : CloudStorageProvider
    {
        #region Fields

        private string _appName;
        private string _containerName;
        private string _azureConnectionString;
        private CloudBlobContainer _container;
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        
        #endregion

        #region Properties

        public override string ContainerName { get { return _containerName; } }
        public override string ApplicationName { get { return _appName; } }

        #endregion

        #region Constructor/Initialization

        public AzureCloudStorageProvider()
        {
            // NOTE: Initialization of providers is done via invoker call to Initialize()
        }

        public override void Initialize( string name, NameValueCollection config )
        {
            if ( String.IsNullOrEmpty( name ) )
                name = "AzureCloudStorageProvider";

            if ( config == null )
                throw new ArgumentNullException( "config" );

            if ( String.IsNullOrEmpty( config["description"] ) )
            {
                config.Remove( "description" );
                config.Add( "description", "Azure Cloud Storage Provider" );
            }

            base.Initialize( name, config );

            this._appName = config["applicationName"];

            string connectionStringName = config["connectionStringName"];

            if ( string.IsNullOrEmpty( connectionStringName ) )
                throw new ProviderException( "Connection string name is not specified" );

            this._azureConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            if ( string.IsNullOrEmpty( this._azureConnectionString ) )
                throw new ProviderException( "Connection string is not found" );

            this._containerName = config["containerName"];

            if ( string.IsNullOrEmpty( this._containerName ) )
                throw new ProviderException( "Container name is not specified" );

            this._storageAccount = CloudStorageAccount.Parse( this._azureConnectionString );
            this._blobClient = this._storageAccount.CreateCloudBlobClient();
            this._container = this._blobClient.GetContainerReference( this.ContainerName );

            this._container.CreateIfNotExists();
        }

        #endregion

        #region Public "Directory" API Methods

        public override bool CreateDirectory( string path )
        {
            path = path.ToAzurePath();

            string blobName = String.Concat( path, "/$.dir" );

            try
            {
                CloudBlockBlob dirBlob = _container.GetBlockBlobReference( blobName );

                using ( var contentStream = new System.IO.MemoryStream( Encoding.UTF8.GetBytes( "" ) ) )
                {
                    dirBlob.UploadFromStream( contentStream );
                }

                BlobProperties props = dirBlob.Properties;
                props.ContentType = "text/plain";
                
                dirBlob.SetProperties();
            }
            catch ( Exception )
            {
                return false;
            }

            return true;
        }

        public override void DeleteDirectory( string path, bool recursive )
        {
            // TJT: Needs to support non-recursive directory deletion

            if ( path == null )
                throw new ArgumentNullException();

            // TJT: Fixme..

            //if ( !IsValidDirectory( path ) )
            //    throw new ArgumentException();

            // cannot delete root directory
            if ( path == "/" )
                throw new ArgumentException();

            string prefix = GetAzurePath( path );
            bool useFlatBlobListing = true;

            // Supports recursive enumeration
            IEnumerable<IListBlobItem> allFiles = _blobClient.ListBlobs( prefix, useFlatBlobListing );
            
            foreach (var file in allFiles) 
            {
                string uri = file.Uri.ToString();

                CloudBlockBlob b = _container.GetBlockBlobReference(uri);
                if (b != null)
                {
                    b.Delete();
                }
            }
        }
        
        public override bool DirectoryExists( string path )
        {
            if ( path == null )
                return false;

            // Important, when dirPath = "/", the behind HasDirectory(dirPath) will throw exceptions
            if ( path == "/" )
                return true;

            // error check
            if (!path.EndsWith(@"/"))
            {
                Trace.WriteLine(string.Format("Invalid parameter {0} for function IsValidDirectory", path ), "Error");
                return false;
            }

            // remove the first '/' char
            string blobDirPath = path.ToAzurePath();

            // get reference
            CloudBlobDirectory blobDirectory = _container.GetDirectoryReference(blobDirPath);

            // non-exist blobDirectory won't contain blobs
            if (blobDirectory.ListBlobs().Count() == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Gets a list of directory names within the directory specified by path.
        /// </summary>
        /// <param name="path">directory path</param>
        /// <returns>array of directory names</returns>
        public override string[] GetDirectories( string path )
        {
            string prefix = GetAzurePath( path );

            IEnumerable<CloudBlobDirectory> directories = _blobClient.ListBlobs( prefix ).OfType<CloudBlobDirectory>();

            string[] dirNames = directories.Select( r => r.Uri.AbsolutePath.ToString() ).ToArray().ToFileSystemPath( path );

            return dirNames;
        }

        public override DirectoryInfo[] GetDirectories( DirectoryInfo dirInfo )
        {
            string path = dirInfo.FullName;

            string prefix = GetAzurePath( dirInfo.FullName );

            List<DirectoryInfo> dirInfoList = new List<DirectoryInfo>();

            IEnumerable<CloudBlobDirectory> blobDirs = _blobClient.ListBlobs( prefix ).OfType<CloudBlobDirectory>();
            
            foreach ( var dir in blobDirs )
            {
                DirectoryInfo di = new DirectoryInfo( dir.Uri.AbsolutePath.ToFileSystemPath( path ) );

                dirInfoList.Add( di );
            }

            return dirInfoList.ToArray<DirectoryInfo>();
        }

        public override FileInfo[] GetDirectoryFiles( DirectoryInfo dirInfo )
        {
            string parentPath = dirInfo.FullName;
            
            string prefix = GetAzurePath( parentPath );

            List<FileInfo> result = new List<FileInfo>();

            IEnumerable<CloudBlockBlob> fileBlobs = _blobClient.ListBlobs( prefix ).OfType<CloudBlockBlob>();

            foreach ( CloudBlockBlob item in fileBlobs )
            {
                FileInfo fi = new FileInfo( item.Uri.AbsolutePath.ToFileSystemPath( parentPath ) );

                result.Add( fi );
            }

            return result.ToArray<FileInfo>();
        }

        public override string[] GetDirectoryFiles( string path )
        {
            string prefix = GetAzurePath( path );

            IEnumerable<CloudBlockBlob> fileBlobs = _blobClient.ListBlobs( prefix ).OfType<CloudBlockBlob>();

            string[] result = fileBlobs.Select( r => r.Uri.AbsolutePath.ToString() ).ToArray().ToFileSystemPath( path );

            return result;
        }

        public override void MoveDirectory( string sourcePath, string destPath ) 
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public "File" API Methods

        public override void DeleteFile( string path )
        {
            CloudBlockBlob blob = GetCloudBlockBlob( path );

            blob.DeleteIfExists();
        }

        public override bool FileExists( string path )
        {
            if ( path == null )
                return false;

            // TJT: Review needed
            if ( path.EndsWith( @"/" ) )
            {
                Trace.WriteLine( string.Format( "Invalid parameter {0} for method FileExists", path ), "Error" );
                return false;
            }

            // remove the first '/' char
            string fileBlobPath = path.ToAzurePath();

            try
            {
                CloudBlockBlob blob = _container.GetBlockBlobReference( fileBlobPath );
                blob.FetchAttributes();
            }
            catch ( StorageException )
            {
                return false;
            }

            return true;
        }

        public override FileAttributes FileAttributes( string path )
        {
            string fullAzurePath = GetAzurePath( path );

            CloudBlockBlob blob = _container.GetBlockBlobReference( AzureExtensions.ToAzurePath( path ) );

            blob.FetchAttributes();

            var props = new FileAttributes
                            {
                                Uri = blob.Uri,
                                LastModified = DateTime.Now,//TJT: Fixme blob.Properties.LastModified,
                                Length = blob.Properties.Length,
                                Path = path,
                                IsDirectory = false
                            };

            return props;
        }

        public override System.IO.Stream FileOpenWrite( string path )
        {
            CloudBlockBlob blob = GetCloudBlockBlob( path );

            if ( blob == null )
                return null;

            return blob.OpenWrite();
        }

        public override System.IO.Stream FileOpenRead( string path )
        {
            CloudBlockBlob blob = GetCloudBlockBlob( path );

            if ( blob == null )
                return null;

            System.IO.Stream stream = blob.OpenRead();
            
            stream.Position = 0;

            return stream;
        }

        /// <summary>
        /// Copy source blob to destination blob.
        /// </summary>
        /// <param name="sourcePath">path to source file</param>
        /// <param name="destPath">path to destination file</param>
        public override void CopyFile( string sourcePath, string destPath )
        {
            string sPath = sourcePath.ToAzurePath();
            string dPath = destPath.ToAzurePath();

            CloudBlockBlob sourceBlob = _container.GetBlockBlobReference( sourcePath.ToAzurePath() );
            CloudBlockBlob destBlob = _container.GetBlockBlobReference( destPath.ToAzurePath() );

            try
            {
                sourceBlob.FetchAttributes();
            }
            catch ( StorageException )
            {
            }

            try
            {
                destBlob.FetchAttributes();
            }
            catch ( StorageException )
            {
            }

            // Copy source blob to destination blob
            try
            {
                destBlob.StartCopyFromBlob( sourceBlob );
            }
            catch ( StorageException e )
            {
                Console.WriteLine( "Error code: " + e.Message );
            }
        }

        public override void FileUploadFromStream( string path, System.IO.Stream fileStream )
        {
            CloudBlockBlob blob = GetCloudBlockBlob( path );

            //blob.Properties.ContentType = contentType;
            blob.UploadFromStream( fileStream );
        }

        public override void MoveFile( string sourceFileName, string destFileName )
        {

            CloudBlockBlob sourceBlob = _container.GetBlockBlobReference( destFileName.ToAzurePath() );
            CloudBlockBlob destBlob = _container.GetBlockBlobReference( sourceFileName.ToAzurePath() );

            // TJT: Fixme..

            // Check if the original path exists on the provider.
            //if ( !IsValidFile( originalPath ) )
            //{
            //    throw new FileNotFoundException( "The path supplied does not exist on the storage provider.", originalPath );
            //}

            destBlob.StartCopyFromBlob( sourceBlob );

            try
            {
                // Make sure the destination file now exists
                destBlob.FetchAttributes();
                
                sourceBlob.Delete();
            }
            catch ( StorageException )
            {
                throw;
            }
        }

        #endregion

        #region Private/Internal Methods

        private CloudBlockBlob GetCloudBlockBlob( string path )
        {
            string blobPath = path.ToAzurePath();

            return _container.GetBlockBlobReference( blobPath );
        }
        
        /// <summary>
        /// Get the full path (as in URI) of a blob folder or file ( container name + path )
        /// </summary>
        /// <param name="path">a folder path or a file path, absolute path</param>
        /// <returns></returns>
        private string GetAzurePath( string path )
        {
            return ContainerName + path;
        }

        #endregion
    }
}
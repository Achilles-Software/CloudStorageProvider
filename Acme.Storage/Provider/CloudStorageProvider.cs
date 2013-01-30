#region Copyright Notice

// Copyright (c) 2013 by Achilles Software, http://achilles-software.com
//
// The source code contained in this file may not be copied, modified, distributed or
// published by any means without the express written agreement by Achilles Software.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com
//
// All rights reserved.

#endregion

#region Namespaces

using Achilles.Acme.Storage.IO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Web;

#endregion

namespace Achilles.Acme.Storage.Provider
{
    /// <summary>
    /// Cloud Storage Provide API class.
    /// </summary>
    public abstract class CloudStorageProvider : ProviderBase
    {
        protected CloudStorageProvider()
        {
        }
        
        #region ProvideBase API

        public abstract string ApplicationName { get; }
        public abstract string ContainerName { get; }

        #endregion

        #region Directory API

        public abstract bool CreateDirectory( string path );
        public abstract bool DirectoryExists( string path );
        public abstract void DeleteDirectory( string path, bool recursive );
        public abstract DirectoryInfo[] GetDirectories( DirectoryInfo dirInfo );
        public abstract string[] GetDirectories( string path );
        public abstract FileInfo[] GetDirectoryFiles( DirectoryInfo dirInfo );
        public abstract string[] GetDirectoryFiles( string path );
        public abstract void MoveDirectory( string sourcePath, string destPath );

        #endregion

        #region File API

        public abstract FileAttributes FileAttributes( string path );
        public abstract bool FileExists( string path );
        public abstract void DeleteFile( string path );
        public abstract System.IO.Stream FileOpenRead( string path );
        public abstract System.IO.Stream FileOpenWrite( string path );
        public abstract void CopyFile( string sourcePath, string destPath );
        public abstract void MoveFile( string sourceFileName, string destFileName );
        public abstract void FileUploadFromStream( string path, System.IO.Stream inputStream );

        #endregion 
    }
}
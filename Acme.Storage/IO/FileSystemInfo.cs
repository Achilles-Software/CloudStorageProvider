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

using Achilles.Acme.Storage.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#endregion

namespace Achilles.Acme.Storage.IO
{
    /// <summary>
    /// Common base class for cloud storage FileInfo and DirectoryInfo classes.
    /// </summary>
    public abstract class FileSystemInfo
    {
        #region Fields

        private static CloudStorageProvider _provider = CloudStorage.Provider;
        private string _path;

        #endregion

        #region Constructors

        public FileSystemInfo( string path )
        {
            this._path = path;
        }

        #endregion 
        
        #region Properties

        // FileAttributes Attributes { get; set; }
        
        public DateTime CreationTime { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public abstract bool Exists { get; }

        public string Extension
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string FullName
        {
            get { return this._path; }
        }
        
        public abstract string Name { get; }

        public DateTime LastAccessTime { get; set; }

        public DateTime LastAccessTimeUtc { get; set; }

        public DateTime LastWriteTime
        {
            get
            {
                // TODO: Store Attributes and refresh as necessary
                return _provider.FileAttributes( this.FullName ).LastModified;
            }
            set {}
        }
        
        public DateTime LastWriteTimeUtc 
        {
            get
            {
                return this.LastWriteTimeUtc.ToLocalTime();
            }
            set
            {
                this.LastWriteTimeUtc = value.ToUniversalTime();
            }
        }

        #endregion

        #region Methods

        public abstract void Delete();
        
        public void Refresh() 
        {
        }

        #endregion
    }
}
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
    /// Cloud Storage to System.IO Bridge: FileInfo class
    /// </summary>
    public class FileInfo : FileSystemInfo
    {
        #region Fields

        private static CloudStorageProvider _provider = CloudStorage.Provider;

        private string _name;

        #endregion

        #region Constructor(s)

        public FileInfo( string fileName )
            : base( fileName )
        {
            this._name = System.IO.Path.GetFileName( fileName );
        }

        #endregion

        #region Properties

        public DirectoryInfo Directory 
        {
            get { throw new NotImplementedException(); }
        }
        
        public string DirectoryName 
        {
            get { return System.IO.Path.GetDirectoryName( base.FullName ); }
        }

        public override bool Exists 
        { 
            get { throw new NotImplementedException(); }
        }
        
        public bool IsReadOnly 
        { 
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        
        /// <summary>
        /// File length in bytes.
        /// </summary>
        public long Length
        {
            get
            {
                // TODO: Store Attributes and refresh as necessary
                return _provider.FileAttributes( this.FullName ).Length;
            }
        }

        public override string Name
        {
            get { return _name; }
        }

        #endregion

        #region Methods

        public override void Delete()
        {
            throw new NotImplementedException();
        }
        
        public void MoveTo( string destFileName )
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
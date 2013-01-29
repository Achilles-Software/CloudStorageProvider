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
    /// Cloud Storage to System.IO Bridge DirectoryInfo class.
    /// </summary>
    public sealed class DirectoryInfo : FileSystemInfo
    {
        #region Fields

        private static CloudStorageProvider _provider = CloudStorage.Provider;
        private string _name;

        #endregion

        #region Constructor

        public DirectoryInfo( string path )
            : base( path )
        {
            if ( path == null )
            {
                throw new ArgumentNullException( "path" );
            }
        }
        
        #endregion

        #region Properties

        public override bool Exists 
        {
            get
            {
                return _provider.DirectoryExists( this.FullName );
            }
        }
        
        public override string Name 
        {
            get 
            {
                if ( base.FullName.Length > 3 )
                {
                    string path = base.FullName;

                    if ( path.EndsWith( "/" ) )
                    {
                        path = path.Substring( 0, path.Length - 1 );
                    }

                    return System.IO.Path.GetFileName( path );
                }

                return base.FullName;
            }
        }
        
        public DirectoryInfo Parent 
        { 
            get { throw new System.NotImplementedException(); }
        }
        
        public DirectoryInfo Root 
        { 
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region Methods

        public void Create()
        {
            throw new NotImplementedException();
        }

        public DirectoryInfo CreateSubdirectory( string path )
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }
        
        public void Delete( bool recursive )
        {
            throw new NotImplementedException();
        }

        public DirectoryInfo[] GetDirectories()
        {
            return _provider.GetDirectories( this );
        }
        
        public FileInfo[] GetFiles()
        {
            return _provider.GetDirectoryFiles( this );
        }

        public void MoveTo( string destDirName )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
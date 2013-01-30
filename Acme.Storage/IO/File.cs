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
    /// Cloud Storage to System.IO Bridge File class.
    /// </summary>
    public static class File
    {
        #region Fields

        private static CloudStorageProvider _provider = CloudStorage.Provider;

        #endregion

        #region Methods

        public static System.IO.Stream OpenRead( string path )
        {
            return _provider.FileOpenRead( path );
        }

        public static System.IO.Stream OpenWrite( string path )
        {
            return _provider.FileOpenWrite( path );
        }

        public static void Copy( string sourceFileName, string destFileName )
        {
            Copy( sourceFileName, destFileName, true );
        }

        public static void Copy( string sourceFileName, string destFileName, bool overwrite )
        {
            _provider.CopyFile( sourceFileName, destFileName );
        }

        public static void Delete( string path )
        {
            if ( path == null )
            {
                throw new ArgumentNullException("path");
            }

            _provider.DeleteFile( path );
        }
        
        public static bool Exists( string path )
        {
            return _provider.FileExists( path );
        }

        public static void Move( string sourceFileName, string destFileName )
        {
            _provider.MoveDirectory( sourceFileName, destFileName );
        }
        

        #endregion
    }
}
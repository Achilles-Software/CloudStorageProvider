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
    /// Cloud Storage to System.IO Bridge Directory class.
    /// </summary>
    public static class Directory
    {
        #region Fields

        private static CloudStorageProvider _provider = CloudStorage.Provider;

        #endregion

        public static DirectoryInfo CreateDirectory( string path )
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (path.Length == 0)
            {
                throw new ArgumentException("path empty");
            }
            
            return new DirectoryInfo( path);
        }

        public static void Delete( string path )
        {
            _provider.DeleteDirectory( path );
        }
        
        public static void Delete( string path, bool recursive )
        {
            _provider.DeleteDirectory( path );
        }

        public static bool Exists( string path )
        {
            return _provider.DirectoryExists( path );
        }

        public static DateTime GetCreationTime( string path )
        {
            throw new NotImplementedException();
        }
        
        public static string GetCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list of directory names with the specified directory path.
        /// </summary>
        /// <param name="path">directory path</param>
        /// <returns>array of directory names</returns>
        public static string[] GetDirectories( string path )
        {
            return _provider.GetDirectories( path );
        }

        public static string GetDirectoryRoot( string path )
        {
            throw new NotImplementedException();
        }

        public static string[] GetFiles( string sDirPath )
        {
            return _provider.GetDirectoryFiles( sDirPath );
        }
        
        public static DateTime GetLastAccessTime( string path )
        {
            throw new NotImplementedException();
        }

        public static DateTime GetLastWriteTime( string path )
        {
            throw new NotImplementedException();
        }

        public static DirectoryInfo GetParent( string path )
        {
            throw new NotImplementedException();
        }
        
        public static void SetCreationTime( string path, DateTime creationTime )
        {
            throw new NotImplementedException();
        }

        public static void SetCurrentDirectory( string path )
        {
            throw new NotImplementedException();
        }

        public static void SetLastAccessTime( string path, DateTime lastAccessTime )
        {
            throw new NotImplementedException();
        }

        public static void SetLastWriteTime( string path, DateTime lastWriteTime )
        {
            throw new NotImplementedException();
        }
    }
}
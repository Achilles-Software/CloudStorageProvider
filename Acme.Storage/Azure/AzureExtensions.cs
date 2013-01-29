#region Copyright Notice

// Copyright (c) 2013 by Achilles Software, http://achilles-software.com
//
// The source code contained in this file may not be copied, modified, distributed or
// published by any means without the express written agreement by Achilles Software.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com
//
// All rights reserved.

// Thanks to Richard Parker for his open source FTP to Azure Blob Storage Bridge.
// See: http://ftp2azure.codeplex.com
//

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#endregion

namespace Achilles.Acme.Storage.Azure
{
    public static class AzureExtensions
    {
        /// <summary>
        /// Convert a string instance of a URI to a relative virtual file system path.
        /// </summary>
        /// <param name="uri">Azure storage blob URI</param>
        /// <returns>file system path to directory or file</returns>
        public static string ToFileSystemPath( this string uri, string path )
        {
            bool isDir = uri.EndsWith(@"/");

            if (isDir)
            {
                // Note: append the directory end tag "/"
                return path + FileNameHelpers.GetDirectoryName(uri) + "/";
            }
            else
            {
                // The path is a file...
                return path + FileNameHelpers.GetFileName( uri );
            }
        }

        /// <summary>
        /// Convert an array of string URI paths to a string array of file system paths
        /// </summary>
        /// <param name="uris">Array of Azure blob URIs</param>
        /// <param name="path">path</param>
        /// <returns>Array of file system paths</returns>
        public static string[] ToFileSystemPath( this string[] uris, string path )
        {
            for (int i = 0; i < uris.Length; i++)
            {
                uris[i] = uris[i].ToFileSystemPath( path );
            }
            
            return uris;
        }

        /// <summary>
        /// Convert a file system path to an azure path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToAzurePath( this string path )
        {
            if ((path == null) || (path.Length <= 1) || (path[0] != '/'))
                throw new Exception("Invalid argument for function:ToAzurePath");
            
            return path.Remove(0, 1);
        }
    }
}
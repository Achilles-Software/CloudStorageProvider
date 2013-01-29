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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#endregion

namespace Achilles.Acme.Storage.Azure
{
    public class FileNameHelpers
    {
        /// <summary>
        /// Check if a blobname(blob or virtual directory) is valid
        /// </summary>
        /// <param name="sFileName">blob name</param>
        /// <returns></returns>
        public static bool IsValid( string sFileName )
        {
            //A blob name must be at least one character long and cannot be more than 1,024 characters long
            if ( ( sFileName.Length == 0 ) || sFileName.Length > 1024 )
                return false;

            //A blob name can contain any combination of characters, except reserved URL characters
            //Notice: '/' is a URL reserved char, but in Azure blob can be used for virtual dir
            string validSpecialChars = @"$-_.+!*'()/";

            foreach ( char c in sFileName )
            {
                if ( !( char.IsLetterOrDigit( c ) || validSpecialChars.IndexOf( c ) >= 0 ) )
                    return false;
            }

            return true;
        }

        /// <summary>
        /// append the final '/' char in the directory name (if not exists)
        /// </summary>
        /// <param name="sDirName">directory name</param>
        /// <returns></returns>
        public static string AppendDirTag( string sDirName )
        {
            int sLength = sDirName.Length;
            if ( sDirName[sLength - 1] != '/' )
                return sDirName + "/";
            else
                return sDirName;
        }

        /// <summary>
        /// Get the directory path of the specified file
        /// </summary>
        /// <param name="sFileName">file name, absolute path</param>
        /// <returns></returns>
        public static string GetDirectory( string sFileName )
        {
            // ToDo: error check
            int idx = sFileName.LastIndexOf( '/' );
            return sFileName.Remove( idx + 1 );
        }

        /// <summary>
        /// Get the fileName of the specified file
        /// </summary>
        /// <param name="filePath">file name, absolute path</param>
        /// <returns></returns>
        public static string GetFileName( string filePath )
        {
            if ( filePath.EndsWith( @"/" ) )
                throw new Exception( "Invalid argument for function:GetFileName" );

            int idx = filePath.LastIndexOf( '/' );
            return filePath.Substring( idx + 1 );
        }

        /// <summary>
        /// Get the directory name of the specified directory
        /// </summary>
        /// <param name="dirPath">directory absolute path</param>
        /// <returns>will not contain the final char '/'</returns>
        public static string GetDirectoryName( string dirPath )
        {
            if ( !dirPath.EndsWith( @"/" ) )
                throw new Exception( "Invalid argument for function:GetDirectoryName" );

            if ( dirPath == "/" )
                return "/";

            string noTagDirPath = dirPath.Remove( dirPath.Length - 1 );
           
            return GetFileName( noTagDirPath );
        }
    }
}
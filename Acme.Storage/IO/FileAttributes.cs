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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#endregion

namespace Achilles.Acme.Storage.IO
{
    public sealed class FileAttributes
    {
        #region Properties

        public DateTime LastModified { get; set; }
        public long Length { get; set; }
        public long Size { get; set; }
        public Uri Uri { get; set; } // not used
        public string Path { get; set; }
        public bool IsDirectory { get; set; }

        #endregion
    }
}
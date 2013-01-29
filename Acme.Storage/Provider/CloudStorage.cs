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
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

#endregion

namespace Achilles.Acme.Storage.Provider
{
    public static class CloudStorage
    {
        // TJT: Do we want this property - private static bool _enabled;

        private static bool _initialized = false;
        private static object _lock = new object();

        private static CloudStorageProvider _provider;
        private static CloudStorageProviderCollection _providers;

        #region Properties

        public static CloudStorageProvider Provider { get { return _provider; } }
        public static CloudStorageProviderCollection Providers { get { return _providers; } }

        public static string ContainerName
        {
            get
            {
                Initialize();
                return Provider.ContainerName;
            }
        }
        
        #endregion

        #region Constructor(s)

        static CloudStorage()
        {
            Initialize();
        }

        #endregion

        #region Private Methods

        private static void Initialize()
        {
            CloudStorageSection cloudStorageConfig = null;

            // don't initialize providers more than once
            if ( !_initialized )
            {
                // get the configuration section for the feature
                cloudStorageConfig = (CloudStorageSection)ConfigurationManager.GetSection( "cloudStorage" );

                if ( cloudStorageConfig == null )
                    throw new Exception( "CloudStorage is not configured for this application" );

                _providers = new CloudStorageProviderCollection();

                // use the ProvidersHelper class to call Initialize on each configured provider
                ProvidersHelper.InstantiateProviders( cloudStorageConfig.Providers, _providers, typeof( CloudStorageProvider ) );

                // set a reference to the default provider
                _provider = (CloudStorageProvider)_providers[ cloudStorageConfig.DefaultProvider];

                // set this feature as initialized
                _initialized = true;
            }
        }

        #endregion
    }
}
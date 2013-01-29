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
using System.Configuration;
using System.Linq;
using System.Web;

#endregion

namespace Achilles.Acme.Storage.Provider
{
    public class CloudStorageSection : ConfigurationSection
    {
        private readonly ConfigurationProperty defaultProvider = new ConfigurationProperty( "defaultProvider", typeof( string ), null );
        private readonly ConfigurationProperty containerName = new ConfigurationProperty( "containerName", typeof( string ), null );
        private readonly ConfigurationProperty connectionStringName = new ConfigurationProperty( "connectionStringName", typeof( string ), null );
        private readonly ConfigurationProperty providers = new ConfigurationProperty( "providers", typeof( ProviderSettingsCollection ), null );
        private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

        public CloudStorageSection()
        {
            properties.Add( providers );
            properties.Add( defaultProvider );
            properties.Add( containerName );
            properties.Add( connectionStringName );
        }

        [ConfigurationProperty( "defaultProvider" )]
        public string DefaultProvider
        {
            get { return (string)base[defaultProvider]; }
            set { base[defaultProvider] = value; }
        }

        [ConfigurationProperty( "connectionStringName" )]
        public string ConnectionStringName
        {
            get { return (string)base[connectionStringName]; }
            set { base[connectionStringName] = value; }
        }

        [ConfigurationProperty( "containerName" )]
        public string ContainerName
        {
            get { return (string)base[containerName]; }
            set { base[containerName] = value; }
        }

        [ConfigurationProperty( "providers" )]
        public ProviderSettingsCollection Providers
        {
            get { return (ProviderSettingsCollection)base[providers]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }
    }
}
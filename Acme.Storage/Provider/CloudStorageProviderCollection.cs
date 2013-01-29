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
using System.Configuration.Provider;
using System.Linq;
using System.Web;

#endregion

namespace Achilles.Acme.Storage.Provider
{
    public class CloudStorageProviderCollection : ProviderCollection
    {
        public override void Add( ProviderBase provider )
        {
            string providerTypeName;

            // make sure the provider supplied is not null
            if ( provider == null )
                throw new ArgumentNullException( "provider" );

            if ( provider as CloudStorageProvider == null )
            {
                providerTypeName = typeof( CloudStorageProvider ).ToString();
                throw new ArgumentException( "Provider must implement CloudStorageProvider type", providerTypeName );
            }
            base.Add( provider );
        }

        new public CloudStorageProvider this[string name]
        {
            get
            {
                return (CloudStorageProvider)base[name];
            }
        }
    }
}
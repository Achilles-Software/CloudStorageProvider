CloudStorageProvider
====================

Cloud Storage to System.IO Bridge Provider ( release 0.1 )

This is an early experimental implementation of an ASP.NET Provider (CloudStorage) for accessing cloud storage
by providing a set of System.IO API "bridge" classes. It utilizes the ASP.NET provider model for configuration
and invocation.

The main purpose of this implementation is to replace the System.IO API calls from within the CKFinder connector
for ASP.NET.

Please note that this implementation is at the experimentaion level or early alpha stage. The complete package
provides a MVC 4 test web application, an Azure cloud storage provider and a modified CKFinder connector.

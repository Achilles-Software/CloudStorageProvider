using CKFinder;
using CKFinder.Connector;
using CKFinder.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finder
{
    public class AzureConnector : Connector
    {
        
        protected override void OnLoad( EventArgs e )
        {
            // base.OnLoad( e );
            // Set the config file instance as the current one (to avoid singleton issues).
            ConfigFile.SetCurrent();

            // Load the config file settings.
            ConfigFile.SetConfig();

            // Load plugins.
            LoadPlugins();

#if (DEBUG)
            // For testing purposes, we may force the user to get the Admin role.
            // Session[ "CKFinder_UserRole" ] = "Admin";

            // Simulate slow connections.
            // System.Threading.Thread.Sleep( 2000 );
#endif
            CKFinder.Connector.CommandHandlers.CommandHandlerBase commandHandler = null;

            try
            {
                // Take the desired command from the querystring.
                string command = Request.QueryString["command"];

                if ( command == null )
                    ConnectorException.Throw( Errors.InvalidCommand );
                else
                {
                    CKFinderEvent.ActivateEvent( CKFinderEvent.Hooks.BeforeExecuteCommand, command, Response );

                    // Create an instance of the class that handles the
                    // requested command.
                    switch ( command )
                    {
                        case "Init":
                            commandHandler = new Finder.CKFinder.Connector.AzureStorageCommandHandlers.InitCommandHandler();
                            break;

                        case "LoadCookies":
                            commandHandler = new AzureStorageCommandHandlers.LoadCookiesCommandHandler();
                            break;

                        case "GetFolders":
                            commandHandler = new AzureStorageCommandHandlers.GetFoldersCommandHandler();
                            break;

                        case "GetFiles":
                            commandHandler = new AzureStorageCommandHandlers.GetFilesCommandHandler();
                            break;

                        case "Thumbnail":
                            commandHandler = new AzureStorageCommandHandlers.ThumbnailCommandHandler();
                            break;

                        case "CreateFolder":
                            commandHandler = new CommandHandlers.CreateFolderCommandHandler();
                            break;

                        case "RenameFolder":
                            commandHandler = new CommandHandlers.RenameFolderCommandHandler();
                            break;

                        case "DeleteFolder":
                            commandHandler = new CommandHandlers.DeleteFolderCommandHandler();
                            break;

                        case "FileUpload":
                            commandHandler = new CommandHandlers.FileUploadCommandHandler();
                            break;

                        case "QuickUpload":
                            commandHandler = new CommandHandlers.QuickUploadCommandHandler();
                            break;

                        case "DownloadFile":
                            commandHandler = new CommandHandlers.DownloadFileCommandHandler();
                            break;

                        case "RenameFile":
                            commandHandler = new CommandHandlers.RenameFileCommandHandler();
                            break;

                        case "DeleteFile":
                            commandHandler = new CommandHandlers.DeleteFileCommandHandler();
                            break;

                        case "CopyFiles":
                            commandHandler = new CommandHandlers.CopyFilesCommandHandler();
                            break;

                        case "MoveFiles":
                            commandHandler = new CommandHandlers.MoveFilesCommandHandler();
                            break;

                        default:
                            ConnectorException.Throw( Errors.InvalidCommand );
                            break;
                    }
                }

                // Send the appropriate response.
                if ( commandHandler != null )
                    commandHandler.SendResponse( Response );
            }
            catch ( ConnectorException connectorException )
            {
#if DEBUG
                // While debugging, throwing the error gives us more useful
                // information.
                throw connectorException;
#else
			    commandHandler = new CommandHandlers.ErrorCommandHandler( connectorException );
			    commandHandler.SendResponse( Response );
#endif
            }
        }
    }
}
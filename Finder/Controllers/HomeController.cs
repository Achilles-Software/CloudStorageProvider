using Achilles.Acme.Storage;
using Achilles.Acme.Storage.Provider;
using Finder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Finder.Controllers
{
    public class HomeController : Controller
    {
        #region Controller Actions

        public ActionResult Index()
        {
            CloudStorageProvider provider = CloudStorage.Provider;

            FinderViewModel model = new FinderViewModel();

            // relative to browser.html
            model.BrowserUrl = "../../connectors/aspx/connector.aspx";

            return View( model );
        }

        #endregion
    }
}

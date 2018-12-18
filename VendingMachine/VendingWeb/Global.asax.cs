using Ninject;
using Ninject.Web.Common.WebHost;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VendingService.Database;
using VendingService.File;
using VendingService;
using VendingService.Interfaces;
using VendingService.Mock;

namespace VendingWeb
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            // This section is if you want to run using a mock database in memory
            //kernel.Bind<IVendingService>().To<MockVendingDBService>();

            // This section is for deployment. You will need to add the vendinguser to your sql database first
            //string passwordCipher = ConfigurationManager.AppSettings["ConnectionPassword"];
            //string privateKeyPath = ConfigurationManager.AppSettings["EncryptionKeyPath"];
            //string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //string password = PasswordManager.DecryptCipherText(passwordCipher, privateKeyPath);
            //connectionString = connectionString.Replace("@PASSWORD", password);

            // This section is for running from Visual Studio
            string connectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;

            kernel.Bind<IVendingService>().To<VendingDBService>().WithConstructorArgument("connectionString", connectionString);
            
            // Bind Log Service
            kernel.Bind<ILogService>().To<LogFileService>();

            return kernel;
        }
    }
}

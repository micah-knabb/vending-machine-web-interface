using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Database;
using VendingService.File;
using VendingService;
using VendingService.Interfaces;
using VendingService.Mock;
using VendingService.Models;

namespace VendingMachineCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;
            var db = new VendingDBService(connectionString);
            var log = new LogFileService();
            VendingMachine vm = new VendingMachine(db, log);
            VendingMachineCLI cli = new VendingMachineCLI(vm);
            cli.Run();
        }
    }
}

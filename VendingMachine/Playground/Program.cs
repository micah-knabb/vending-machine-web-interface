using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            //PasswordManager.GenerateKeys(@"C:\VendingData\public.xml", @"C:\VendingData\private.xml");
            var text = PasswordManager.GetCipherText("vendinguser", @"C:\VendingData\public.xml");
            var plain = PasswordManager.DecryptCipherText("sg2ZUN14UsyOrk4imbSPd2FbgOzyUrXMxa7bO8YRwGsgHMfk2C2Jun5eXgPrUd5KHeA8St9HZuklkqp+n9xhBIQZ7FeDRQ7AVN7MQlHzm5pJ6zcUa99U7FWwYP3mVrn5YpSTrD1SIsS/bgonfsqUzo7H2RySqcSIlo5paTzS0xA=", @"C:\VendingData\private.xml");
            Console.WriteLine(text);
            Console.WriteLine(plain);
            Console.ReadKey();
        }
    }
}

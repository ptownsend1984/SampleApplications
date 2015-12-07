using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data;
using System.ServiceModel;
using WebServices.Calculator;

namespace WebServices
{
    class Program
    {
        static void Main(string[] args)
        {
            var ServiceHost = new ServiceHost(typeof(CalculatorService));
            ServiceHost.Open();

            var Client = new CalculatorServiceClient();
            try
            {
                //Client.Divide(1, 0);
                var Result = Client.WeighPickle(new Pickle { Name = "Bob", Bumps = 12 });
                Console.WriteLine(Result);
                Client.Close();
            }
            catch (Exception)
            {
                Client.Abort();
                throw;
            }


            Console.ReadLine();
        }
    }
}

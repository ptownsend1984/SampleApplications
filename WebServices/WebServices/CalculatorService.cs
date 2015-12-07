using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebServices.Contracts;
using System.ServiceModel;

namespace WebServices
{
    public class CalculatorService : ICalculatorService
    {


        public int Add(int x, int y)
        {
            return x + y;
        }

        public int Subtrace(int x, int y)
        {
            return x - y;
        }

        public int Multiply(int x, int y)
        {
            return x * y;
        }

        public int Divide(int x, int y)
        {
            try
            {
                return x / y;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }


        public Models.Pickle WeighPickle(Models.Pickle pickle)
        {
            return new Models.Pickle { Bumps = pickle.Bumps * pickle.Name.Length };
        }
    }
}
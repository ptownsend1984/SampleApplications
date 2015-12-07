using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using WebServices.Models;

namespace WebServices.Contracts
{
    [ServiceContract]
    public interface ICalculatorService
    {

        [OperationContract]
        int Add(int x, int y);
        [OperationContract]
        int Subtrace(int x, int y);
        [OperationContract]
        int Multiply(int x, int y);
        [OperationContract]
        int Divide(int x, int y);

        [OperationContract]
        Pickle WeighPickle(Pickle pickle);

    }
}
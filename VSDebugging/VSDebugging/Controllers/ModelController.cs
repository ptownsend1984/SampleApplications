using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VSDebugging.Models;

namespace VSDebugging.Controllers
{
    public class ModelController
    {

        public Company CreateCompany()
        {
            var Model = new VSDebugging.Models.Company();
            Model.ID = 1;
            Model.Location = "Twelve";
            Model.PhoneNumbers.Add("555-5555");
            return Model;
        }
        public Employee CreateEmployee()
        {
            return new Employee { ID = 4 };
        }
        public Buggy CreateBuggy()
        {
            return new Buggy();
        }

        public void ShowBuggy()
        {
            var Buggy = new ModelController().CreateBuggy();

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Console.WriteLine(Buggy.Value);
            }).Wait();
        }

    }
}
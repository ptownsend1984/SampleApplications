using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Reflection.Helper2.Types
{
    
    public class MarshalByRefModel : MarshalByRefObject, IMarshalByRefModel
    {
        public int Value { get; set; }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Throw()
        {
            throw new Exception("oh noes");
        }

        public string CurrentDomainName { get { return AppDomain.CurrentDomain.FriendlyName; } }

    }

    public interface IMarshalByRefModel
    {
        int Value { get; set; }
        void Throw();
        string CurrentDomainName { get; }
    }

    public class AssemblyProxy : MarshalByRefObject
    {
        public AssemblyProxy()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }
        ~AssemblyProxy()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            return appDomainAssemblies.FirstOrDefault(o => o.GetName().Name.Equals(args.Name, StringComparison.OrdinalIgnoreCase));
        }
        public void LoadAssembly(string assemblyPath)
        {
            try
            {
                Assembly.LoadFile(assemblyPath);
            }
            catch { return; }
        }
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

}
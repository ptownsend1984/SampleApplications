using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Helper2.Wrappers;
using Reflection.Helper2.Types;
using System.Reflection;

namespace Reflection.Tests
{
    [TestClass()]
    public class DemoTests
    {

        #region Test attributes

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        #region Test Methods

        [TestMethod]
        public void CreateObjectFromType()
        {
            var obj = new object();

            var constructor = typeof(object).GetConstructors().First();
            var instance = constructor.Invoke(new object[] { });

            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(object));
        }

        [TestMethod]
        public void EvaluateFields()
        {
            var model = new FieldsModel();
            model.internalInt = 10;
            model.publicInt = 5;

            var type = typeof(FieldsModel);
            var publicIntField = type.GetField("publicInt", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var internalIntField = type.GetField("internalInt", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var privateIntField = type.GetField("privateInt", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var privateStaticIntField = type.GetField("privateStaticInt", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Assert.IsNotNull(publicIntField);
            Assert.IsNotNull(internalIntField);
            Assert.IsNotNull(privateIntField);
            Assert.IsNotNull(privateStaticIntField);

            Assert.AreEqual(model.publicInt, publicIntField.GetValue(model));

            int internalIntValue = 3;
            int privateIntValue = 12;
            int privateStaticIntValue = 20;

            internalIntField.SetValue(model, internalIntValue);
            privateIntField.SetValue(model, privateIntValue);
            privateStaticIntField.SetValue(null, privateStaticIntValue);

            Assert.AreEqual(internalIntField.GetValue(model), internalIntValue);
            Assert.AreEqual(privateIntField.GetValue(model), privateIntValue);
            Assert.AreEqual(privateStaticIntField.GetValue(null), privateStaticIntValue);
        }

        [TestMethod]
        public void EvaluateFieldsBad()
        {
            var model = new FieldsModel();

            var type = typeof(FieldsModel);
            var publicIntField = type.GetField("publicInt", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            Assert.IsNotNull(publicIntField);

            try
            {
                publicIntField.SetValue(model, model);
            }
            catch (ArgumentException)
            {
            }

            try
            {
                publicIntField.SetValue(model, byte.MaxValue);
            }
            catch (ArgumentException)
            {
            }

            try
            {
                publicIntField.SetValue(model, true);
            }
            catch (ArgumentException)
            {
            }

            try
            {
                publicIntField.SetValue(model, (long)0);
            }
            catch (ArgumentException)
            {
            }

            try
            {
                publicIntField.SetValue(model, long.MaxValue);
            }
            catch (ArgumentException)
            {
            }

        }

        [TestMethod]
        public void EvaluateMethods()
        {
            var model = new MethodsModel();

            var point = model.CreatePoint();
            Assert.AreEqual(point.X, model.x);
            Assert.AreEqual(point.Y, model.y);

            var createPointMethod = typeof(MethodsModel).GetMethod("CreatePoint");
            Assert.IsNotNull(createPointMethod);

            var refPoint = (System.Drawing.Point)createPointMethod.Invoke(model, null);
            Assert.AreEqual(refPoint.X, model.x);
            Assert.AreEqual(refPoint.Y, model.y);

            try
            {
                typeof(MethodsModel).GetMethod("CreatePoint", BindingFlags.Instance | BindingFlags.NonPublic);
            }
            catch (AmbiguousMatchException) { }

            var createPoint2Method = typeof(MethodsModel).GetMethod("CreatePoint", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(int) }, null);
            var createPoint3Method = typeof(MethodsModel).GetMethod("CreatePoint", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(int), typeof(int) }, null);

            Assert.IsNotNull(createPoint2Method);
            Assert.IsNotNull(createPoint3Method);

            var refPoint2 = (System.Drawing.Point)createPoint2Method.Invoke(model, new object[] { 5 });
            Assert.AreEqual(refPoint2.X, 5);
            Assert.AreEqual(refPoint2.Y, model.y);
        }

        [TestMethod]
        public void EvaluateProperties()
        {
            var model = new PropertiesModel();
            var type = model.GetType();

            var xProperty = type.GetProperty("X");
            var yProperty = type.GetProperty("Y");
            var zProperty = type.GetProperty("Z");
            var wProperty = type.GetProperty("W");

            Assert.IsNotNull(xProperty);
            Assert.IsTrue(xProperty.CanRead);
            Assert.IsFalse(xProperty.CanWrite);

            Assert.IsNotNull(yProperty);
            Assert.IsTrue(yProperty.CanRead);
            Assert.IsTrue(yProperty.CanWrite);

            Assert.IsNotNull(zProperty);
            Assert.IsFalse(zProperty.CanRead);
            Assert.IsTrue(zProperty.CanWrite);

            Assert.IsNotNull(wProperty);
            Assert.IsTrue(wProperty.CanRead);
            Assert.IsTrue(wProperty.CanWrite);

            Assert.AreEqual(xProperty.GetValue(model), model.X);

            var xGetMethod = xProperty.GetGetMethod();
            Assert.IsNotNull(xGetMethod);
            Assert.AreEqual((int)xGetMethod.Invoke(model, null), model.X);

            zProperty.SetValue(model, 20);
            Assert.AreEqual(model.GetZ(), 20);

            var zSetMethod = zProperty.GetSetMethod();
            Assert.IsNotNull(zSetMethod);
            zSetMethod.Invoke(model, new object[] { 12 });
            Assert.AreEqual(model.GetZ(), 12);

            wProperty.SetValue(model, 99);
            Assert.AreEqual(model.W, 99);

            var indexerProperty = type.GetProperty("Item");
            Assert.IsNotNull(indexerProperty);
            Assert.IsTrue(indexerProperty.GetIndexParameters().Any());
            Assert.AreEqual(model[64], indexerProperty.GetValue(model, new object[] { 64 }));
        }

        [TestMethod]
        public void CreateWithGetType()
        {
            var noType = Type.GetType("Reflection.Helper2.Wrappers.TypeWrapper");
            Assert.IsNull(noType);
            
            var type = Type.GetType("Reflection.Helper2.Wrappers.TypeWrapper, Reflection.Helper2");
            Assert.IsNotNull(type);

            var instance = Activator.CreateInstance(type, new object[] { type });
            Assert.IsNotNull(instance);

            var allPropertiesProperty = type.GetProperty("AllProperties");
            var allProperties = (IEnumerable)allPropertiesProperty.GetValue(instance, null);
            Assert.IsTrue(allProperties.Cast<object>().Any());
        }        

        [TestMethod]
        public void CallInternalClass()
        {
            var type = Type.GetType("System.SR, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            Assert.IsNotNull(type);

            var arg_emptyOrNullString = type.GetField("Arg_EmptyOrNullString", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            Assert.IsNotNull(arg_emptyOrNullString);

            var getStringMethod = type.GetMethod("GetString", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, new Type[] { typeof(string) }, null);
            Assert.IsNotNull(getStringMethod);

            var msg = getStringMethod.Invoke(null, new object[] { arg_emptyOrNullString.GetValue(null) });
            Assert.IsInstanceOfType(msg, typeof(string));
            Assert.IsTrue(!string.IsNullOrEmpty((string)msg));
        }

        [TestMethod]
        public void EvaluateInterfaces()
        {
            var model = new InterfacesModel();
            model.Value = 255;
            var type = typeof(InterfacesModel);

            var allInterfaces = type.GetInterfaces();
            Assert.IsTrue(allInterfaces.Any());

            var icomparableInterface = type.GetInterface("IComparable");
            Assert.IsNotNull(icomparableInterface);

            var compareTo = (int)icomparableInterface.GetMethod("CompareTo").Invoke(model, new object[] { 0 });
            Assert.IsTrue(compareTo > 0);
            compareTo = (int)icomparableInterface.GetMethod("CompareTo").Invoke(model, new object[] { new System.Drawing.Rectangle() });
            Assert.AreEqual(compareTo, 0);

            var icomparableGenericInterface = type.GetInterface("IComparable`1");
            Assert.IsNotNull(icomparableGenericInterface);
            compareTo = (int)icomparableInterface.GetMethod("CompareTo").Invoke(model, new object[] { int.MaxValue });
            Assert.IsTrue(compareTo < 0);
        }

        [TestMethod]
        public void CreateGenerics()
        {
            var type = typeof(List<int>);
            var list1 = (List<int>)Activator.CreateInstance(type);
            Assert.IsNotNull(list1);

            var constructor = type.GetConstructor(new Type[] { });
            Assert.IsNotNull(constructor);
            var list2 = (List<int>)constructor.Invoke(new object[] { });
            Assert.IsNotNull(list2);

            type = Type.GetType("System.Collections.Generic.List`1");
            Assert.IsNotNull(type);
            var longListType = type.MakeGenericType(new Type[] { typeof(long) });
            constructor = longListType.GetConstructor(new Type[] { });
            var list3 = (List<long>)constructor.Invoke(new object[] { });
            Assert.IsNotNull(list3);
        }

        [TestMethod]
        public void EvaluateEvents()
        {
            var model = new EventsModel();
            var type = typeof(EventsModel);

            model.Value = 10;
            model.Promoted += model_Promoted;
            model.RaisePromoted();

            var promotedEvent = type.GetEvent("Promoted");
            Assert.IsNotNull(promotedEvent);

            var addMethod = promotedEvent.AddMethod;
            var removeMethod = promotedEvent.RemoveMethod;
            Assert.IsNotNull(addMethod);
            Assert.IsNotNull(removeMethod);

            removeMethod.Invoke(model, new object[] { (EventHandler)model_Promoted });
            addMethod.Invoke(model, new object[] { (EventHandler)model_Promoted2 });

            model.Value = 15;
            model.RaisePromoted();

            var eventField = type.GetField("Promoted", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(eventField);

            var eventHandler = eventField.GetValue(model);
            Assert.IsNotNull(eventHandler);

            var invokeMethod = eventHandler.GetType().GetMethod("Invoke");
            Assert.IsNotNull(invokeMethod);

            invokeMethod.Invoke(eventHandler, new object[] { model, EventArgs.Empty });

            var eventHandlerDelegate = ((Delegate)eventHandler);
            eventHandlerDelegate.DynamicInvoke(new object[] { model, EventArgs.Empty });

            var invokeList = eventHandlerDelegate.GetInvocationList();
            foreach (var item in invokeList)
            {
                item.DynamicInvoke(model, EventArgs.Empty);
            }
        }
        private void model_Promoted(object sender, EventArgs e)
        {
            var model = sender as EventsModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Value, 10);
        }
        private void model_Promoted2(object sender, EventArgs e)
        {
            var model = sender as EventsModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Value, 15);
        }

        [TestMethod]
        public void CreateAppDomain()
        {
            const string BadPath = @"..\..\..\Reflection.Helper.Bad\Reflection.Common.dll";
            const string BadPath2 = @"..\..\..\Reflection.Helper.Bad\Reflection.Helper.dll";
            var settings = new AppDomainSetup();

            settings.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            var domain = AppDomain.CreateDomain("Awesome", null, settings);
            try
            {
                var loadedAssemblies = domain.GetAssemblies();
                Assert.IsTrue(loadedAssemblies.Length == 1);

                domain.Load(typeof(FieldsModel).Assembly.GetName());
                loadedAssemblies = domain.GetAssemblies();
                Assert.IsTrue(loadedAssemblies.Length == 2);

                var intRef = domain.CreateInstance(typeof(int).Assembly.FullName, typeof(int).FullName, null);
                var intUnwrap = intRef.Unwrap();

                var modelRef = domain.CreateInstance(typeof(MarshalByRefModel).Assembly.FullName, typeof(MarshalByRefModel).FullName, null);
                var modelUnwrap = (MarshalByRefModel)modelRef.Unwrap();
                modelUnwrap.Value = 12;
                //modelUnwrap.Throw();

                var proxyRef = domain.CreateInstance(typeof(AssemblyProxy).Assembly.FullName, typeof(AssemblyProxy).FullName, null);
                var proxyUnwrap = (AssemblyProxy)proxyRef.Unwrap();
                proxyUnwrap.LoadAssembly(System.IO.Path.GetFullPath(BadPath));
                proxyUnwrap.LoadAssembly(System.IO.Path.GetFullPath(BadPath2));

                var helperRef = domain.CreateInstance("Reflection.Helper", "Reflection.Helper.HelperUtility", null);
                var helperUnwrap = (ICommonClass)helperRef.Unwrap();
                var helperString = helperUnwrap.ToString();
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }


        //[TestMethod]
        //public void CreateObjectFromTypeWrapper()
        //{
        //    var objectType = new TypeWrapper(typeof(object));
        //    var constructor = objectType.AllConstructors.First();
        //    var instance = constructor.Info.Invoke(new object[] { });

        //    Assert.IsNotNull(instance);
        //    Assert.IsInstanceOfType(instance, typeof(object));
        //}

        #endregion

    }

}
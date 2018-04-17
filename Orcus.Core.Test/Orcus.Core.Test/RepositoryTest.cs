using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orcus.Core.DataAccess.RepositoryPattern;
using Orcus.Core.DataAccess.UnitOfWork;
using Orcus.Core.Extension;
using Orcus.Core.Test.Repository;
using System;
using System.Data;
using System.Reflection;

namespace Orcus.Core.Test
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void UnitOfWork_Test()
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork(new NORTHWNDEntities()))
            {
                unitOfWork.BeginTransaction();
                unitOfWork.CommitTransaction();
            }
        }
        
        [TestMethod]
        public void UnitOfWork_Pattern_Test()
        {
            var context = new NORTHWNDEntities();
            using (IUnitOfWork unitOfWork = new UnitOfWork(context))
            {
                unitOfWork.BeginTransaction();
                var customerRepository = unitOfWork.Repository<Customers>();
                customerRepository.Insert(new Customers
                {
                    CustomerID = "YAMI1",
                    CompanyName = "Deneme - CompanyName"
                });
                unitOfWork.SaveChanges();
                unitOfWork.RollbackTransaction();

                unitOfWork.Dispose();
                var isDisposed = (bool)GetInstanceField(typeof(UnitOfWork), unitOfWork, "_disposed");
                Assert.IsTrue(isDisposed);

                unitOfWork.Dispose();
                context.Dispose();

                context.Dispose();
                unitOfWork.Dispose();
            }
        }

        [TestMethod]
        public void Repository_Pattern_GetData_Test()
        {
            var context = new NORTHWNDEntities();
            using (IUnitOfWork unitOfWork = new UnitOfWork(context))
            {
                IRepository<Customers> customerRepository = unitOfWork.Repository<Customers>();
                var retVal = customerRepository.GetFirstOrDefault();
                Assert.IsNotNull(retVal.Address);
            }
        }

        [TestMethod]
        public void UnitOfWork_Service_Pattern_Test()
        {
            var context = new NORTHWNDEntities();
            using (IUnitOfWork unitOfWork = new UnitOfWork(context))
            {
                unitOfWork.BeginTransaction();
                var customerService = new CustomerService(unitOfWork);
                customerService.Insert(new Customers
                {
                    CustomerID = "YAMI1",
                    CompanyName = "Deneme - CompanyName"
                });
                
                unitOfWork.SaveChanges();
                unitOfWork.RollbackTransaction();

                unitOfWork.Dispose();
                var isDisposed = (bool)GetInstanceField(typeof(UnitOfWork), unitOfWork, "_disposed");
                Assert.IsTrue(isDisposed);

                unitOfWork.Dispose();
                context.Dispose();

                context.Dispose();
                unitOfWork.Dispose();
            }
        }

        [TestMethod]
        public void Service_Pattern_GetData()
        {
            var context = new NORTHWNDEntities();
            using (IUnitOfWork unitOfWork = new UnitOfWork(context))
            {
                var customerService = new CustomerService(unitOfWork);
                var result = customerService.GetFirstOrDefault();
                Assert.IsNotNull(result.ResultObject.CompanyName);
            }
        }

        [TestMethod]
        public void Service_Pattern_GetData_CustomersByCompany()
        {
            var context = new NORTHWNDEntities();
            using (IUnitOfWork unitOfWork = new UnitOfWork(context))
            {
                var customerService = new CustomerService(unitOfWork);
                var result = customerService.CustomersByCompany("bon");
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void Customers_ToDataTable()
        {
            var context = new NORTHWNDEntities();
            using (IUnitOfWork unitOfWork = new UnitOfWork(context))
            {
                var customerService = new CustomerService(unitOfWork);
                var result = customerService.Get();
                var retVal = result.ResultObject;
                
                DataTable dtResults = new DataTable();
                dtResults = retVal.ToDataTable();

                Assert.IsNotNull(retVal.IsNullOrEmpty());
                dtResults.ToCsv(",", true, @"C:\_Projeler.Net","deneme");

                var dtList = dtResults.ToList<Customers>();
                var str = "Yakup KALEBAŞI";
                var left = str.Left(7);
                var right = str.Right(7);

                Assert.IsNotNull(result);

                Assert.IsTrue((string.Empty.IsNullOrEmptyOrWhiteSpace()));
                Assert.IsTrue(("   ".IsNullOrEmptyOrWhiteSpace()));
                Assert.IsFalse(("TestValue".IsNullOrEmptyOrWhiteSpace()));

                string sayi = "123";
                var sayiInt = sayi.ConvertTo<int>();
                //var sayiDateTime = sayi.To<DateTime>();

                string zipstring = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".CompressString();
                string unzipstring = zipstring.DecompressString();

                // regular parsing
                int i = "123".ConvertTo<int>();
                int? inull = "123".ConvertTo<int?>();
                DateTime d = "01/12/2008".ConvertTo<DateTime>();
                DateTime? dn = "01/12/2008".ConvertTo<DateTime?>();


                // null values
                string sample = null;
                int? k = sample.ConvertTo<int?>(); // returns null
                int l = sample.ConvertTo<int>();   // returns 0
                DateTime dd = sample.ConvertTo<DateTime>(); // returns 01/01/0001
                DateTime? ddn = sample.ConvertTo<DateTime?>(); // returns null

            }
        }

        private static object GetInstanceField(Type type, object instance, string fieldName)
        {
            const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var field = type.GetField(fieldName, bindFlags);
            return field != null ? field.GetValue(instance) : null;
        }
    }
}

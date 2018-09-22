using System;
using System.Linq;
using System.Data.Entity.SqlServer;

namespace Forever16.Models.DAL
{
    public class Sale
    {
        public string Store { get; set; }
    }

    public class SalesPerStore : Sale
    {
        public int Year { get; set; }
        public decimal Total { get; set; }
    }

    public class SalesPerProduct : Sale
    {
        public string Product { get; set; }
        public int Total { get; set; }
    }

    public class TotalSalesPerStore : Sale
    {
        public decimal TotalSales { get; set; }
    }

    public class SalesPerAgeAndGender : Sale
    {
        public string Gender { get; set; }
        public int Age { get; set; }
        public int Quantity { get; set; }
    }

    public class Repository
    {
        public SalesPerStore[] GetSalesPerStore(int initialYear, int endYear)
        {
            using (Forever16DBEntities DB = new Forever16DBEntities())
            {
                var query = from store in DB.Stores
                            join sale in DB.Sales on store.Id equals sale.StoreId
                            join item in DB.SaleItems on sale.Id equals item.SaleId
                            join product in DB.Products on item.ProductId equals product.Id
                            where sale.Date.Year >= initialYear && sale.Date.Year <= endYear
                            group product.Price * item.Quantity by new
                            {
                                Store = store.Name,
                                Year = sale.Date.Year
                            }
                                    into salesPerItem
                            select new SalesPerStore
                            {
                                Store = salesPerItem.Key.Store,
                                Year = salesPerItem.Key.Year,
                                Total = salesPerItem.Sum()
                            };

                return query.ToArray();
            }
        }

        private void VerifyPeriod(ref DateTime? begin, ref DateTime? end)
        {
            if (!begin.HasValue) { begin = DateTime.MinValue; }
            if (!end.HasValue) { end = DateTime.Now; }
        }

        public SalesPerProduct[] GetSalesPerProduct(int[] productList, DateTime? begin, DateTime? end)
        {
            VerifyPeriod(ref begin, ref end);

            using (Forever16DBEntities DB = new Forever16DBEntities())
            {
                var query = from sale in DB.Sales
                            join saleItem in DB.SaleItems on sale.Id equals saleItem.SaleId
                            join product in DB.Products on saleItem.ProductId equals product.Id
                            join store in DB.Stores on sale.StoreId equals store.Id
                            where productList.Contains(product.Id)
                               && sale.Date >= begin && sale.Date <= end
                            group saleItem.Quantity by new
                            {
                                Store = store.Name,
                                Product = product.Name
                            }
                             into salesPerProduct
                            select new SalesPerProduct
                            {
                                Store = salesPerProduct.Key.Store,
                                Product = salesPerProduct.Key.Product,
                                Total = salesPerProduct.Sum()
                            };

                return query.ToArray();
            }
        }

        public TotalSalesPerStore[] PercentageSalesPerStore(DateTime begin, DateTime end)
        {
            using (Forever16DBEntities DB = new Forever16DBEntities())
            {
                var query = from saleItem in DB.SaleItems
                            join sale in DB.Sales on saleItem.SaleId equals sale.Id
                            join product in DB.Products on saleItem.ProductId equals product.Id
                            join store in DB.Stores on sale.StoreId equals store.Id
                            where sale.Date >= begin && sale.Date <= end
                            group saleItem.Quantity * product.Price by new
                            {
                                Store = store.Name
                            }
                            into percentageSalesPerStore
                            select new TotalSalesPerStore
                            {
                                Store = percentageSalesPerStore.Key.Store,
                                TotalSales = percentageSalesPerStore.Sum()
                            };
                return query.ToArray();
            }
        }

        public SalesPerAgeAndGender[] GetSalesPerAgeAndGender(DateTime? begin, DateTime? end)
        {
            VerifyPeriod(ref begin, ref end);

            using (Forever16DBEntities DB = new Forever16DBEntities())
            {
                var query = from sale in DB.Sales
                            join item in DB.SaleItems on sale.Id equals item.SaleId
                            join store in DB.Stores on sale.StoreId equals store.Id
                            join client in DB.Clients on sale.ClientId equals client.Id
                            where sale.Date >= begin && sale.Date <= end
                            group item.Quantity by new
                            {
                                Store = store.Name,
                                Gender = client.gender,
                                Age = (int)(SqlFunctions.DateDiff("day", client.DateOfBirth, sale.Date) / 365)
                            }
                            into salesAgeGender
                            select new SalesPerAgeAndGender()
                            {
                                Age = (int)salesAgeGender.Key.Age,
                                Store = salesAgeGender.Key.Store,
                                Gender = salesAgeGender.Key.Gender,
                                Quantity = salesAgeGender.Sum()
                            };

                return query.ToArray();
            }
        }
    }
}
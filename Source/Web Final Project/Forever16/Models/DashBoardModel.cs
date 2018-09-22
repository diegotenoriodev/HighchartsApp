using Forever16.Models.Charts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Forever16.Models
{
    public class DashBoardModel
    {
        private DAL.Repository repo = new DAL.Repository();

        public BasicLine<decimal?> GetSalesPerStore(int initialYear, int endYear)
        {
            BasicLine<decimal?> chart = new BasicLine<decimal?>();

            DAL.SalesPerStore[] query
                = repo.GetSalesPerStore(initialYear, endYear);

            string[] stores = GetStoreList(query);

            //Getting the list of years
            int[] years = query
                .OrderBy(r => r.Year)
                .Select(r => r.Year)
                .Distinct().ToArray();

            chart.PointStart = years.FirstOrDefault();

            foreach (var store in stores)
            {
                Serie<decimal?> serie = new Serie<decimal?>()
                {
                    Name = store,
                    Data = new decimal?[years.Length]
                };

                for (int i = 0; i < years.Length; i++)
                {
                    decimal? value = query
                        .Where(r => r.Year == years[i] && r.Store == store)
                        .Select(r => r.Total)
                        .FirstOrDefault();

                    serie.Data[i] = value;
                }

                chart.Series.Add(serie);
            }

            return chart;
        }

        public BasicBar<int?> GetSalesPerProduct(int[] productList, DateTime? begin, DateTime? end)
        {
            BasicBar<int?> chart = new BasicBar<int?>();

            DAL.SalesPerProduct[] query
                = repo.GetSalesPerProduct(productList, begin, end);

            string[] stores = GetStoreList(query);

            //Getting the list of years
            string[] products = query
                .OrderBy(r => r.Product)
                .Select(r => r.Product)
                .Distinct().ToArray();

            chart.Categories = products;

            foreach (var store in stores)
            {
                Serie<int?> serie = new Serie<int?>()
                {
                    Name = store,
                    Data = new int?[products.Length]
                };

                for (int i = 0; i < products.Length; i++)
                {
                    int? value = query
                        .Where(r => r.Store == store && r.Product == products[i])
                        .Select(r => r.Total)
                        .FirstOrDefault();

                    serie.Data[i] = value;
                }

                chart.Series.Add(serie);
            }

            return chart;
        }

        public PieChart GetPercentageSalesPerStore(DateTime initialYear, DateTime endYear)
        {
            DAL.TotalSalesPerStore[] query = repo.PercentageSalesPerStore(initialYear, endYear);
            decimal totalSales = query.Sum(r => r.TotalSales);
            PieChart pie = new PieChart();

            //Getting list of all stores
            string[] stores = GetStoreList(query);
            pie.Data = new Data[stores.Length];

            for (int i = 0; i < stores.Length; i++)
            {
                Data data = new Data()
                {
                    Name = stores[i],
                    Y = query.FirstOrDefault(r => r.Store == stores[i]).TotalSales / totalSales * 100
                };

                pie.Data[i] = data;
            }

            return pie;
        }

        public Basic3DBar<int> GetSalesPerAgeAndGender(DateTime? begin, DateTime? end)
        {
            Basic3DBar<int> barchart = new Basic3DBar<int>();

            DAL.SalesPerAgeAndGender[] query
                = repo.GetSalesPerAgeAndGender(begin, end);

            string[] stores = GetStoreList(query);
            Dictionary<string, DAL.SalesPerAgeAndGender[]> groupByAge
                = new Dictionary<string, DAL.SalesPerAgeAndGender[]>()
                {
                    {"Less than 18", query.Where(r => r.Age < 18).ToArray() },
                    {"Between 18 and 30", query.Where(r => r.Age >= 18 && r.Age <= 30).ToArray() },
                    {"31 or More", query.Where(r => r.Age > 30).ToArray() },
                };

            string[] genders = query.Select(r => r.Gender).Distinct().ToArray();
            barchart.Categories = stores;
            foreach (var item in groupByAge)
            {
                string ageGroup = item.Key;

                foreach (var gender in genders)
                {
                    Serie3D<int> serie = new Serie3D<int>()
                    {
                        Name = gender + " - " + ageGroup,
                        Data = new int[stores.Length],
                        Stack = gender
                    };

                    barchart.Series.Add(serie);

                    for (int i = 0; i < stores.Length; i++)
                    {
                        var row = item.Value
                            .FirstOrDefault(
                                r =>
                                    r.Gender == gender &&
                                    r.Store == stores[i]
                                );

                        serie.Data[i] = row != null ? row.Quantity : 0;
                    }
                }
            }

            return barchart;
        }

        #region Helper Methods

        private string[] GetStoreList(DAL.Sale[] sales)
        {
            return sales.OrderBy(r => r.Store)
                      .Select(r => r.Store)
                      .Distinct().ToArray();
        }

        public ListItem[] GetProducts()
        {
            using (Forever16DBEntities DB = new Forever16DBEntities())
            {
                return DB.Products
                    .Select(r => new ListItem()
                    {
                        Text = r.Name,
                        Value = r.Id
                    })
                    .OrderBy(r => r.Text)
                    .ToArray();
            }
        }

        public int[] GetYears()
        {
            using (Forever16DBEntities DB = new Forever16DBEntities())
            {
                var query = from r in DB.Sales
                            select r.Date.Year;

                return query.Distinct().OrderBy(r => r).ToArray();
            }
        }

        #endregion
    }
}
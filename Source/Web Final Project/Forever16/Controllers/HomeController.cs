using Forever16.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Web.Mvc;

namespace Forever16.Controllers
{
    public class HomeController : Controller
    {
        private DashBoardModel model = new DashBoardModel();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetSalesPerStore()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetSalesPerProduct()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetPercentageSalesPerStore()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetSalesPerAgeAndGender()
        {
            return View();
         }

        private new JsonResult Json(object obj)
        {
            return base.Json(JsonConvert.SerializeObject(obj, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));

        }

        [HttpPost]
        [Route("Home/GetSalesPerStore")]
        public JsonResult GetSalesPerStore(int begin, int end)
        {
            return Json(model.GetSalesPerStore(begin, end));
        }

        [HttpPost]
        [Route("Home/GetSalesPerProduct")]
        public JsonResult GetSalesPerProduct(int[] products, DateTime? begin, DateTime? end)
        {
            return Json(model.GetSalesPerProduct(products, begin, end));
        }

        [HttpPost]
        [Route("Home/GetYears")]
        public JsonResult GetYears()
        {   
            return base.Json(model.GetYears());
        }

        [HttpPost]
        [Route("Home/GetAvailableProducts")]
        public JsonResult GetAvailableProducts()
        {
            return Json(model.GetProducts());
        }

        [HttpPost]
        [Route("Home/GetPercentageSalesPerStore")]
        public JsonResult GetPercentageSalesPerStore(DateTime begin, DateTime end)
        {
            return Json(model.GetPercentageSalesPerStore(begin, end));
        }

        [HttpPost]
        [Route("Home/GetSalesPerAgeAndGender")]
        public JsonResult GetSalesPerAgeAndGender(DateTime? begin, DateTime? end)
        {
            return Json(model.GetSalesPerAgeAndGender(begin, end));
        }
    }
}
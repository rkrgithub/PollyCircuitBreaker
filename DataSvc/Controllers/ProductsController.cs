using DataSvc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;

namespace DataSvc.Controllers
{
    public class ProductsController : ApiController
    {
        private Product[] Products()
        {
            //#region "For server Load - Searching in all folders in D: drive and reading specified file to get products data"
            //DirectoryInfo folder = new DirectoryInfo(@"D:\");
            //FileInfo[] files = folder.GetFiles("datafile.txt", SearchOption.AllDirectories);
            //if (files.Length > 0)
            //{
            //    using (StreamReader sr = new StreamReader(files[0].OpenRead()))
            //    {
            //        // Read the stream to a string, and write the string to the console.
            //        String text = sr.ReadToEnd();
            //        JavaScriptSerializer js = new JavaScriptSerializer();
            //        return js.Deserialize<Product[]>(text.Replace("\\", string.Empty));
            //    }
            //}
            ////return new Product[] { };
            /// to do
            //#endregion

            #region Read products from 1 MB file
            //using (XmlReader reader = XmlReader.Create(HostingEnvironment.MapPath(@"~/App_Data/1 MB_File.xml")))
            //{
            //    while (reader.Read())
            //    {
            //        if (reader.IsStartElement())
            //        {
            //            switch (reader.Name.ToString())
            //            {
            //                case "ProductData":
            //                    JavaScriptSerializer js = new JavaScriptSerializer();
            //                    return js.Deserialize<Product[]>(reader.ReadString().Replace("\\", string.Empty));
            //            }
            //        }
            //    }
            //}
            #endregion  

            return new Product[] { new Product() { ProductId = 1111, Title = "Fitness Nutrition Specialization (FNS)", Price = 499.00, ReleaseDate = DateTime.Now },
                new Product() { ProductId = 2222, Title = "Weight Loss Specialization (WLS)", Price = 299.00, ReleaseDate = DateTime.Now },
                new Product() { ProductId = 3333, Title = "Behavior Change Specialization (BCS)", Price = 699.00, ReleaseDate = DateTime.Now },
                new Product() { ProductId = 4444, Title = "Group Personal Training Specialization (GPTS)", Price = 549.00, ReleaseDate = DateTime.Now }
            };
        }

        /// <summary>
        /// Get products with given delay
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Product> GetdelayedProducts(int delay = 5000)
        {
            System.Threading.Thread.Sleep(delay);
            return Products();
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return Products();
        }

        /// <summary>
        /// Get an exception
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetException()
        {
            Product[] res = null;
            try
            {
                int a = 1;
                int g = a / 0;
                res = Products();
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "My custom error message");
            }
            return Request.CreateResponse(HttpStatusCode.OK, res); ;

        }
    }
}

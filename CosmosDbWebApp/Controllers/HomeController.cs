using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using CosmosDbWebApp.Models;
using Newtonsoft.Json;

namespace CosmosDbWebApp.Controllers
{
    
    public class HomeController : Controller
    {
        string EndpointUrl;
        private string PrimaryKey;
        private DocumentClient client;
        public HomeController()
        {
            EndpointUrl = "https://celebalcosmos.documents.azure.com:443/";
            PrimaryKey = "dv6RNF4JfyHQYYSQhc6Lcx0zzfsvNFIW6Jk2dPccaZVvlPDWnVDlzbZjDN8kkdYgGpw9SArfSTfnACDbs3cNOg==";

            client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
        }
        public ActionResult Index()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Employee> emp = client.CreateDocumentQuery<Employee>(UriFactory.CreateDocumentCollectionUri("EmployeeDb", "Container1"), queryOptions);
            return View(emp);
        }
        [HttpPost]
        public ActionResult AddUpdateEmployee(string name, string id)
        {
            string result = "";
            if (id != "0")
            {
                dynamic Employee = new
                {
                    name = name,
                    id=id
                };
                var document1 = client.UpsertDocumentAsync(
                UriFactory.CreateDocumentCollectionUri("EmployeeDb", "Container1"),
                Employee);
                result = "Employee Updated Successfully!";
            }
            else
            {
                dynamic Employee = new
                {
                    name=name
                };
                var document1 = client.CreateDocumentAsync(  
                UriFactory.CreateDocumentCollectionUri("EmployeeDb", "Container1"),  
                Employee);
                result = "Employee Added Successfully!";
            }
            //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoConnection"].ConnectionString))
            //{
            //    connection.Open();
            //    if (id > 0)
            //    {
            //        using (SqlCommand command = new SqlCommand("Update Employee set Name='" + name + "' ,Gender='" + gender + "' ,Location='" + location + "' where id='" + id + "'", connection))
            //        {
            //            int rowsAffected = command.ExecuteNonQuery();
            //            result = rowsAffected > 0 ? "Employee Updated" : "Error";
            //        }
            //    }
            //    else
            //    {
            //        using (SqlCommand command = new SqlCommand("INSERT INTO Employee(Name,Gender,Location) VALUES('" + name + "','" + gender + "','" + location + "')", connection))
            //        {
            //            int rowsAffected = command.ExecuteNonQuery();
            //            result = rowsAffected > 0 ? "Employee Added" : "Error";
            //        }
            //    }                
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetEmployeeById(string id)
        {
            var response = client.CreateDocumentQuery
 (UriFactory.CreateDocumentCollectionUri("EmployeeDb", "Container1"),
     "select * from c where c.id='"+id+"'").ToList();
            var document = response.First();
            //DataTable dt = new DataTable();
            //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoConnection"].ConnectionString))
            //{
            //    connection.Open();
            //    using (SqlCommand command = new SqlCommand("SELECT * FROM Employee where Id='" + id + "'", connection))
            //    using (SqlDataReader reader = command.ExecuteReader())
            //    {
            //        dt.Load(reader);
            //    }
            //}
            return Json(JsonConvert.SerializeObject(document), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteEmployeeById(string id)
        {
            string result = "";
            var docUri = UriFactory.CreateDocumentUri("EmployeeDb", "Container1",id);

            var result1 = client.DeleteDocumentAsync(docUri, new RequestOptions { PartitionKey = new PartitionKey(id) });
            //    Document doc=client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri("EmployeeDb", "Container1"))
            //.Where(d => d.Id == id)
            //.AsEnumerable()
            //.FirstOrDefault();
            //    client.DeleteDocumentAsync(doc.SelfLink);
            //dynamic Employee = new
            //{
            //    id = doc.Id
            //};
            //var document1 = client.DeleteDocumentAsync(
            //    UriFactory.CreateDocumentCollectionUri("EmployeeDb", "Container1"),
            //    Employee);
            result = "Employee Deleted Successfully!";
            //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoConnection"].ConnectionString))
            //{
            //    connection.Open();
            //    using (SqlCommand command = new SqlCommand("delete Employee where id='" + id + "'", connection))
            //    {
            //        int rowsAffected = command.ExecuteNonQuery();
            //        result = rowsAffected > 0 ? "Employee Deleted" : "Error";
            //    }
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Employees()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Employee> emp = client.CreateDocumentQuery<Employee>(UriFactory.CreateDocumentCollectionUri("EmployeeDb", "Container1"), queryOptions);
            return View(emp);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
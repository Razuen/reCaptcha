using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.OleDb;

namespace recaptcha.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FormSubmit(String name, String age, String M, String F, String number)
        {
            //Validate Google recaptcha here
            var response = Request["g-recaptcha-response"];
            string secretKey = "";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            String gender = null;
            if (M == null)
                gender = "Female";
            else if (M.Equals("on"))
                gender = "Male";




            try
            {
                if (status)
                {
                    OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='E:\\Documents\\sem6\\platform technology\\Captacha\\Recaptcha.accdb'");
                    OleDbCommand cmd = new OleDbCommand();
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO Captcha VALUES(@name,@age,@gender,@number);";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@number", number);
                    cmd.ExecuteNonQuery();




                }
                //ViewBag.Message = status ? age : "No Value";
                ViewBag.Message = status ? "Captcha validation success" : "Captcha validation Failed";
            }
            catch (Exception exc)
            {
                ViewBag.Message = exc.Message;
            }
            //When you will post form for save data, you should check both the model validation and google recaptcha validation
            //EX.
            /* if (ModelState.IsValid && status)
            {

            }*/

            //String n = name;
            //Console.WriteLine(n);

            //Here I am returning to Index page for demo perpose, you can use your view here
            return View("Index");
        }
    }
}
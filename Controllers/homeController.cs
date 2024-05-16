using Curd__Practice.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Curd__Practice.Controllers
{
    public class homeController : Controller
    {
        DBLayer db=new DBLayer();

        //[checkSession]
        public ActionResult Index()
        {
            SqlParameter[] aa = new SqlParameter[]
            {
                new SqlParameter("@action",1)
            };
          DataTable data= db.ExecuteSelect("sp_stdInfo",aa);
          ViewBag.data = data;
          return View();
            
        }
        //[checkSession]
        public ActionResult about()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(STDManager ss)
        {
            SqlParameter[] aa = new SqlParameter[]
            {
                new SqlParameter("@name",ss.sname), 
                new SqlParameter("@addr",ss.address), 
                new SqlParameter("@mob",ss.mob), 
                new SqlParameter("@dob",ss.dob), 
                new SqlParameter("@spic",ss.spic.FileName),
                new SqlParameter("@action",2) 
            };
            int data = db.ExecuteIUD("sp_stdInfo", aa);
            if (data > 0)
            {
                ss.spic.SaveAs(Server.MapPath("~/Content/images/" + ss.spic.FileName));
                return Content("<script>alert('Yes');location.href='/home/index';</script>");
            }
            else
            {
                return Content("<script>alert('No');location.href='/home/index';</script>");
            }
            
           
        }
        public ActionResult delete(int ?id)
        {
            SqlParameter[] aa = new SqlParameter[]
           {
                new SqlParameter("@id",id),
                new SqlParameter("@action",4)
           };
            db.ExecuteIUD("sp_stdInfo", aa);
            return RedirectToAction("Index");
        
        }
        public ActionResult update(int ?id)
        {
            SqlParameter[] b = new SqlParameter[]
         {
                new SqlParameter("@id",id),
                new SqlParameter("@action",5)
         };
            DataTable data =db.ExecuteSelect("sp_stdInfo",b);
            ViewBag.data = data;
            return View();
         
        }

        [HttpPost]
        public ActionResult update(STDManager ss ,int? id)
        {
                SqlParameter[] param = new SqlParameter[]
          {
                new SqlParameter("@id",id),
                new SqlParameter("@name",ss.sname),
                new SqlParameter("@addr",ss.address),
                new SqlParameter("@mob",ss.mob),
                new SqlParameter("@dob",ss.dob),
                new SqlParameter("@spic",ss.spic.FileName),
                new SqlParameter("@action",3)
          };
            int data = db.ExecuteIUD("sp_stdInfo", param);
            if (data > 0)
            {
                return Content("<script>alert('Data Updated ');location.href='/home/index';</script>");
            }
            else
            {
                return Content("<script>alert('Data No Updated ');location.href='/home/index';</script>");
            }
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(string mob, string dob)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@mob",mob),
                new SqlParameter("@dob",dob),

                };
            DataTable dt = db.ExecuteSelect("sp_login", param);
            if (dt.Rows.Count > 0)
            {
                Session["username"] = mob;
                return RedirectToAction("index", "home");
            }
            else
            {
                return Content("<script>alert('Login Failed.');location.href='/home/login'</script>");
            }
        }
        public ActionResult logout()
        {
            if (Session["username"] != null)
            {
                Session.Remove("username");
            }
            return RedirectToAction("about", "home");
        }
        class checkSession : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {

                HttpSessionStateBase session = filterContext.HttpContext.Session;
                if (session["username"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                    {
                        {"Controller","Home" },{"Action","login"}
                    });
                }
            }
        }
        // send email --------
        public ActionResult EmailLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EmailLogin(string remail)
        {
            if (ModelState.IsValid)
            {
                var senderemail = new MailAddress("satyaprakash9919346535@gmail.com", "satya");

                {

                }
                var recieveremail = new MailAddress(remail, "reciever");

                var sub = "check OTP";
                var body = "Done";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderemail.Address, "lkoo oftd xrbk dxna")
                };
                using (var message = new MailMessage(senderemail, recieveremail)
                {
                    Subject = sub,
                    Body = body,
                })
                {
                    smtp.Send(message);
                }

                return Content("<script>alert('Mail send')</script>");
            }
            return View();
        }
    }
    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveYouSeenMe.Models;
using HaveYouSeenMe.Models.Business;
using HaveYouSeenMe.DAO;
using Microsoft.Security.Application;

using System.Net.Mail;

namespace HaveYouSeenMe.Controllers
{
    public class MessageController : Controller
    {
        private IPetDao Dao;
        PetManagement PetManager;

        public MessageController()
        {
            Dao = new PetDao();
            PetManager = new PetManagement(Dao);
        }

        public MessageController(IPetDao petDao)
        {
            Dao = petDao;
            PetManager = new PetManagement(Dao);
        }

        //
        // GET: /Message/

        public ActionResult Send()
        {
            var name = (string)RouteData.Values["id"];
            ViewBag.PetName = name;
            ViewBag.IsSent = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(MessageModel model)
        {
            //viewbag data needed for any request back
            ViewBag.IsSent = false;
            ViewBag.PetName = model.PetName;

            if (ModelState.IsValid)
            {
                model.Message = Sanitizer.GetSafeHtmlFragment(model.Message);

                //retreive owner e-mail                
                try
                {
                    model.To = GetOwnerEmail(model.PetName);
                }
                catch 
                {
                    ModelState.AddModelError("", "Could not find pet owner's e-mail");
                    return View(model);
                }

                if (SendGoogleMail(model))
                {
                    return RedirectToAction("ThankYou");
                }
                else
                { 
                    //Something went wrong with the e-mail
                    ModelState.AddModelError("", "There was a problem trying to send the e-mail message, try later");
                    return View(model);
                }
            }

            ModelState.AddModelError("", "One or more errors were found");            
            return View(model);
        }

        public PartialViewResult PartialThankYou()
        {
            return PartialView("ThankYou");
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        private string GetOwnerEmail(string PetName)
        {
            //get pet from data
            var pet = PetManager.GetByName(PetName);

            //retreive owner and owner e-mail
            return pet.UserProfile.Email;
        }

        private bool SendGoogleMail(MessageModel model) 
        {
            try
            {
                using(var mail = new MailMessage())
                {
                    mail.To.Add(model.To);
                    mail.From = new MailAddress("rbrito@tekmexico.com");
                    mail.Subject = "From " + model.From + 
                        "(" + model.Email + ") - " + model.Subject;
                    string Body = model.Message;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 25; // 587 or 25(local server)
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential
                        (mail.From.Address, "idril3");// Enter seders User name and password  
                    smtp.EnableSsl = true;

                    smtp.Send(mail);
                }
            }
            catch
            {                
                return false;
            }

            //everything went OK
            return true;
        }

    }
}

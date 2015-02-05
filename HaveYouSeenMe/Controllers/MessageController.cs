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
        private IPetDao petDao;
        PetManagement PetManager;

        private IMessageDao messageDao;
        MessageManagement MessageManager;

        public MessageController()
        {
            petDao = new PetDao();
            PetManager = new PetManagement(petDao);

            messageDao = new MessageDao();
            MessageManager = new MessageManagement(messageDao);
        }

        public MessageController(IPetDao petDao, IMessageDao messageDao)
        {
            this.petDao = petDao;
            PetManager = new PetManagement(this.petDao);

            this.messageDao = messageDao;
            MessageManager = new MessageManagement(this.messageDao);
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

                //retreive owner e-mail and ID
                try
                {
                    UserProfile usr = GetPetOwner(model.PetName);
                    model.To = usr.Email;
                    model.UserID = usr.UserId;
                }
                catch 
                {
                    ModelState.AddModelError("", "Could not find pet owner's e-mail");
                    return View(model);
                }

                ////try sending an e-mail message
                //if (SendGoogleMail(model))
                //{
                //    return RedirectToAction("ThankYou");
                //}
                //else
                //{ 
                //    //Something went wrong with the e-mail
                //    ModelState.AddModelError("", "There was a problem trying to send the message, please try later");
                //    return View(model);
                //}

                //convert the view model to data model
                Message message = model.ToDataModel();

                //try sending new message
                try 
                {
                    message = MessageManager.SendNew(message);
                }
                catch (ApplicationException Ex)
                {
                    //could not persist new data
                    ModelState.AddModelError("", Ex.Message);
                    return View(model);
                }
                catch (Exception Ex)
                {
                    ModelState.AddModelError("", "Unknown Error");
                    return View(model);
                }

                //succeed
                return RedirectToAction("ThankYou");
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

        private UserProfile GetPetOwner(string PetName)
        {
            //get pet from data
            var pet = PetManager.GetByName(PetName);

            //retreive owner
            return pet.UserProfile;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveYouSeenMe.Models;
using Microsoft.Security.Application;

namespace HaveYouSeenMe.Controllers
{
    public class MessageController : Controller
    {
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
            if (ModelState.IsValid)
            {
                model.Message = Sanitizer.GetSafeHtmlFragment(model.Message);
                return RedirectToAction("ThankYou");
            }

            ModelState.AddModelError("", "One or more errors were found");
            return View(model);
        }

        public PartialViewResult ThankYou()
        {
            return PartialView();
        }

    }
}

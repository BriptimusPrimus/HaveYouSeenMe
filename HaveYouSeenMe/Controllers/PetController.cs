﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveYouSeenMe.Models;
using HaveYouSeenMe.Models.Business;
using System.IO;
using HaveYouSeenMe.DAO;

namespace HaveYouSeenMe.Controllers
{
    public class PetController : Controller
    {
        private IPetDao Dao;
        PetManagement PetManager;

        public PetController()
        {
            Dao = new PetDao();
            PetManager = new PetManagement(Dao);
        }

        public PetController(IPetDao petDao)
        {
            Dao = petDao;
            PetManager = new PetManagement(Dao);
        }

        //
        // GET: /Pet/

        public ActionResult Index()
        {
            return View();
        }

        //[Authorize(Users = "rbrito")]
        [Authorize]
        public ActionResult Display()
        {
            var name = (string)RouteData.Values["id"];
            var model = PetManager.GetByName(name);

            if (model == null)
                return RedirectToAction("NotFound");

            return View(model);
        }

        public ActionResult MissingPets()
        {
            //get the list of pets from business layer
            IEnumerable<Pet> missingPets = PetManager.GetMissing();

            //convert items of the list to viewmodel objects
            List<PetModel> list = new List<PetModel>();
            foreach (var pet in missingPets)
            {
                list.Add(
                    new PetModel
                    {
                        PetID = pet.PetID,
                        PetName = pet.PetName,
                        PetAgeYears = pet.PetAgeYears,
                        PetAgeMonths = pet.PetAgeMonths,
                        StatusID = pet.StatusID,
                        LastSeenOn = pet.LastSeenOn,
                        LastSeenWhere = pet.LastSeenWhere,
                        Notes = pet.Notes,
                        UserId = pet.UserId
                    }
                );
            }

            return View(list);
        }

        public FileResult DownloadPetPicture()
        {
            var name = (string)RouteData.Values["id"];
            var picture = "/Content/Uploads/" + name + ".jpg";
            var contentType = "image/jpg";
            return File(picture, contentType);
        }

        public ActionResult PictureUpload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PictureUpload(PictureModel model)
        {
            if (model.PictureFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(model.PictureFile.FileName);
                var filePath = Server.MapPath("/Content/Uploads");
                string savedFileName = Path.Combine(filePath, fileName);
                model.PictureFile.SaveAs(savedFileName);
                PetManagement.CreateThumbnail(fileName, filePath, 100, 100, true);
            }

            return View(model);
        }

        public ActionResult GetPhoto()
        {
            var name = (string)RouteData.Values["id"];
            ViewBag.Photo = string.Format("/Content/Uploads/{0}.jpg", name);

            return PartialView();
        }

        public JsonResult GetInfo()
        {
            var name = (string)RouteData.Values["id"];
            Pet pet = PetManager.GetByName(name);

            return Json(pet, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public HttpStatusCodeResult UnauthorizedError()
        {
            return new HttpUnauthorizedResult("Custom Unauthorized Error");
        }

        public ActionResult NotFoundError()
        {
            return HttpNotFound("Nothing here...");
        }

    }
}
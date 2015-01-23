using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HaveYouSeenMe.Models;
using HaveYouSeenMe.Models.Business;
using System.IO;
using HaveYouSeenMe.DAO;
using HaveYouSeenMe.Utilities;

namespace HaveYouSeenMe.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private IPetDao Dao;
        PetManagement PetManager;

        public MembersController()
        {
            Dao = new PetDao();
            PetManager = new PetManagement(Dao);
        }

        public MembersController(IPetDao petDao)
        {
            Dao = petDao;
            PetManager = new PetManagement(Dao);
        }

        //
        // GET: /Members/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Members/PetsOwned

        public ActionResult PetsOwned()
        {
            //Get current user
            var usrName = User.Identity.Name;

            //get the list of owner's pets from business layer
            IEnumerable<Pet> missingPets = PetManager.GetPetsFromOwner(usrName);

            //convert items of the list to viewmodel objects
            IEnumerable<PetModel> list =
                ModelsConverter.ConvertList<PetModel>(missingPets);

            return View(list);
        }


        //
        // GET: /Members/Details/5

        public ActionResult Details(int id = 0)
        {
            var pet = PetManager.GetById(id);
            if (pet == null)
            {
                return RedirectToAction("NotFound");
            }
            
            //convert to view model
            PetModel model = ModelsConverter.ConvertModel<PetModel>(pet);

            ViewBag.PetsOwned = true;
            return View("../Pet/Display", model);
        }

        //
        // GET: /Members/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Members/Create

        [HttpPost]
        public ActionResult Create(PetModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //convert the view model to data model
            Pet pet = model.ToDataModel();

            //instruct the business layer to persist the new object
            try
            {
                PetManager.CreateNew(pet, User.Identity.Name);
            }
            catch(ApplicationException Ex)
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
            return RedirectToAction("PetsOwned");            
        }

        //
        // GET: /Members/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var pet = PetManager.GetById(id);
            if (pet == null)
            {
                return RedirectToAction("NotFound");
            }

            //convert to view model
            PetModel model = ModelsConverter.ConvertModel<PetModel>(pet);

            return View(model);        
        }

        //
        // POST: /Members/Edit/5

        [HttpPost]
        public ActionResult Edit(PetModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //convert the view model to data model
            Pet pet = model.ToDataModel();

            //instruct the business layer to persist the new object
            try
            {
                PetManager.Update(pet);
            }
            catch (ApplicationException Ex)
            {
                //could not persist updated data
                ModelState.AddModelError("", Ex.Message);
                return View(model);
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", "Unknown Error");
                return View(model);
            }

            //succeed
            return RedirectToAction("PetsOwned");             
        }

        //
        // GET: /Movies/Members/5

        public ActionResult Delete(int id = 0)
        {
            var pet = PetManager.GetById(id);
            if (pet == null)
            {
                return RedirectToAction("NotFound");
            }

            //convert to view model
            PetModel model = ModelsConverter.ConvertModel<PetModel>(pet);

            return View(model); 
        }

        //
        // POST: /Members/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //instruct the business layer to persist the new object
            try
            {
                PetManager.Delete(id);
            }
            catch (ApplicationException Ex)
            {
                //could not delete data
                ModelState.AddModelError("", Ex.Message);
                return View();
            }
            catch(Exception Ex)
            {
                ModelState.AddModelError("", "Unknown Error");
                return View();            
            }

            //succeed
            return RedirectToAction("PetsOwned"); 
        }

        //
        // POST: /Members/PictureUpload/picture.jpg

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PictureUpload(PictureModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NotFound");
            }

            try
            {
                if (model.PictureFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(model.PictureFile.FileName);
                    var filePath = Server.MapPath("/Content/Uploads");
                    string savedFileName = Path.Combine(filePath, model.PetName + ".jpg");
                    model.PictureFile.SaveAs(savedFileName);
                    PetManagement.CreateThumbnail(model.PetName + ".jpg", filePath, 100, 100, true);
                }
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", Ex.Message);
                return View("Edit", new PetModel { PetName = model.PetName });
                //return RedirectToAction("Edit", new { id = model.PetID });
            }

            return RedirectToAction("Details", new { id = model.PetID });
        }

        public ActionResult NotFound()
        {
            return View();
        }

    }
}

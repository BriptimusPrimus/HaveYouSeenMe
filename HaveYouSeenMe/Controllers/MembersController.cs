using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HaveYouSeenMe.Models;
using HaveYouSeenMe.Models.Business;
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

        public ActionResult Index()
        {
            return View();
        }

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

        public ActionResult Details(int id = 0)
        {
            var pet = PetManager.GetById(id);

            if (pet == null)
                return RedirectToAction("NotFound");

            PetModel model = ModelsConverter.ConvertModel<PetModel>(pet);

            ViewBag.PetsOwned = true;
            return View("../Pet/Display", model);
        }

        public ActionResult Create()
        {
            return View();
        }

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
            catch(Exception Ex)
            {            
                //could not persist new data
                ModelState.AddModelError("", Ex.Message);
                return View(model);          
            }

            //succeed
            return RedirectToAction("PetsOwned");            
        }

        public ActionResult NotFound()
        {
            return View();
        }

    }
}

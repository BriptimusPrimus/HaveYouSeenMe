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
        private IPetDao petDao;
        PetManagement PetManager;

        private IMessageDao messageDao;
        MessageManagement MessageManager;

        public MembersController()
        {
            petDao = new PetDao();
            PetManager = new PetManagement(petDao);

            messageDao = new MessageDao();
            MessageManager = new MessageManagement(messageDao);
        }

        public MembersController(IPetDao petDao, IMessageDao messageDao)
        {
            this.petDao = petDao;
            PetManager = new PetManagement(petDao);

            this.messageDao = messageDao;
            MessageManager = new MessageManagement(this.messageDao);
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
                pet = PetManager.CreateNew(pet, User.Identity.Name);
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
            return RedirectToAction("Edit", new { id = pet.PetID });           
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

            ViewBag.StatusList = PetManager.StatusList();

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
            //instruct the business layer to delete the object
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
                ModelState.AddModelError("", "Could not upload image, try later");
                return View("Edit", new PetModel { PetName = model.PetName });
                //return RedirectToAction("Edit", new { id = model.PetID });
            }

            return RedirectToAction("Details", new { id = model.PetID });
        }

        //
        // GET: /Members/Messages

        public ActionResult Messages()
        {
            //Get current user
            var usrName = User.Identity.Name;

            //get the list of user messages from business layer
            IEnumerable<Message> messages = MessageManager.GetUserMessages(usrName)
                .OrderByDescending(x => x.MessageDate);

            //convert items of the list to viewmodel objects
            IEnumerable<MessageModel> list =
                ModelsConverter.ConvertList<MessageModel>(messages);

            return View(list);
        }

        [HttpPost, ActionName("DeleteMessage")]
        public ActionResult DeleteMessageConfirmed(int id)
        {
            //instruct the business layer to persist the new object
            try
            {
                MessageManager.Delete(id);
            }
            catch (ApplicationException Ex)
            {
                //could not delete data
                ModelState.AddModelError("", Ex.Message);
                return View();
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", "Unknown Error");
            }

            //succeed or not
            return RedirectToAction("Messages");
        }

        public ActionResult PetsSinglePage()
        {
            //set an empty list of owner's pets          
            IEnumerable<PetModel> list = new PetModel[]{};

            return View(list);
        }

        //[AllowAnonymous]
        public ActionResult PetsOwnedAjaxHandler(jQueryDataTableParamModel param)
        {
            //Get current user
            var usrName = User.Identity.Name;

            //get the list of owner's pets from business layer
            IEnumerable<Pet> missingPets = PetManager.GetPetsFromOwner(usrName); 

            //filtering
            IEnumerable<Pet> filteredPets;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredPets = missingPets
                               .Where(p => p.PetName.IndexOf(param.sSearch, StringComparison.OrdinalIgnoreCase) >= 0
                                  || (p.LastSeenWhere != null && p.LastSeenWhere.IndexOf(param.sSearch, StringComparison.OrdinalIgnoreCase) >= 0)
                                  || (p.LastSeenOn != null && String.Format("{0:dd/MM/yyyy}", p.LastSeenOn).Contains(param.sSearch)));
            }
            else
            {
                filteredPets = missingPets;
            }

            //sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Pet, string> orderingFunction = 
                (p => sortColumnIndex == 1 ? p.PetName :
                      sortColumnIndex == 2 ? p.PetAgeYears + " - " + p.PetAgeMonths :
                      sortColumnIndex == 3 ? String.Format("{0:yyyyMMdd}", p.LastSeenOn) : 
                      sortColumnIndex == 4 ? p.LastSeenWhere : p.StatusID.ToString());

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredPets = filteredPets.OrderBy(orderingFunction);
            else
                filteredPets = filteredPets.OrderByDescending(orderingFunction);

            //pagination
            //convert items of the list to viewmodel objects
            IEnumerable<PetModel> displayedPets =
                ModelsConverter.ConvertList<PetModel>(filteredPets
                    .Skip(param.iDisplayStart).Take(param.iDisplayLength));

            var result = from p in displayedPets
                         select new[] { "", p.PetName, p.PetAge, String.Format("{0:dd/MM/yyyy}", p.LastSeenOn), 
                                        p.LastSeenWhere, p.StatusDescription, "" };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = missingPets.Count(),
                iTotalDisplayRecords = filteredPets.Count(),
                aaData = displayedPets
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditAjaxHandler(PetModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    error = "Invalid data"
                }, JsonRequestBehavior.AllowGet);
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
                return Json(new
                {
                    error = Ex.Message
                }, JsonRequestBehavior.AllowGet);                
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", "Unknown Error");
                return Json(new
                {
                    error = "Unknown Error"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                message = "Pet updated"
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PictureUploadAjaxHandler(HttpPostedFileBase model, string petName, string petId)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    error = "Invalid data"
                }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                if (model.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(model.FileName);
                    var filePath = Server.MapPath("/Content/Uploads");
                    string savedFileName = Path.Combine(filePath, petName + ".jpg");
                    model.SaveAs(savedFileName);
                    PetManagement.CreateThumbnail(petName + ".jpg", filePath, 100, 100, true);
                }
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", "Could not upload image, try later");
                return Json(new
                {
                    error = "Could not upload image, try later"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                message = "Pet updated"
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateAjaxHandler(PetModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {
                    error = "Invalid data"
                }, JsonRequestBehavior.AllowGet);
            }

            //convert the view model to data model
            Pet pet = model.ToDataModel();

            //instruct the business layer to persist the new object
            try
            {
                pet = PetManager.CreateNew(pet, User.Identity.Name);
            }
            catch (ApplicationException Ex)
            {
                //could not persist new data
                ModelState.AddModelError("", Ex.Message);
                return Json(new
                {
                    error = Ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", "Unknown Error");
                return Json(new
                {
                    error = "Unknown Error"
                }, JsonRequestBehavior.AllowGet);
            }

            //succeed, transform to pet model and returned via JSON
            PetModel data = ModelsConverter.ConvertModel<PetModel>(pet);
            return Json(new
            {
                model = data
            }, JsonRequestBehavior.AllowGet);            
        }

        public ActionResult NotFound()
        {
            return View();
        }

    }
}

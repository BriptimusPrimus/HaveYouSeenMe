using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using HaveYouSeenMe;
using HaveYouSeenMe.Controllers;
using HaveYouSeenMe.Models.Business;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using HaveYouSeenMe.DAO;
using HaveYouSeenMe.Models;

namespace HaveYouSeenMe.Tests.Controllers
{
    [TestClass]
    public class PetControllerTest
    {
        private PetController _controller;

        public PetControllerTest()
        {
            Mock<IPetDao> _repository = new Mock<IPetDao>();
            _repository.Setup(x => x.GetPetByName(It.Is<string>(y => y == "Fido")))
                        .Returns(
                                    new Models.Pet
                                    {
                                        PetName = "Fido",
                                        Status = new Status { Description = "lost" },
                                        UserProfile = new UserProfile { UserName = "rbrito" }
                                    }
                        );

            _repository.Setup(x => x.GetPetsWithStatus(It.Is<string>(y => y == "lost")))
                        .Returns(
                            new [] 
                            {
                                    new Models.Pet
                                    {
                                        PetName = "Fido",
                                        Status = new Status { Description = "lost" },
                                        UserProfile = new UserProfile { UserName = "rbrito" }
                                    },
                                    new Models.Pet
                                    {
                                        PetName = "Connie",
                                        Status = new Status { Description = "lost" },
                                        UserProfile = new UserProfile { UserName = "rbrito" }
                                    }   
                            }       
                        );

            _controller = new PetController(_repository.Object);
        }

        private void SetControllerContext(string petName)
        {
            RouteData routeData = new RouteData();
            routeData.Values.Add("id", petName);
            ControllerContext context = new ControllerContext { RouteData = routeData, };
            _controller.ControllerContext = context;
        }

        [TestMethod]
        public void Index_NoInputs_ReturnsDefaultViewResult()
        {
            // Arrange
            PetController controller = new PetController();

            // Act
            ViewResult result = (ViewResult)controller.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void Display_ExistingPet_ReturnPartialView()
        {
            // Arrange
            string petName = "Fido";
            SetControllerContext(petName);

            // Act
            PartialViewResult result = (PartialViewResult)_controller.Display();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(HaveYouSeenMe.Models.PetModel));
            Assert.AreEqual(petName, ((HaveYouSeenMe.Models.PetModel)result.Model).PetName);
        }

        [TestMethod]
        public void Display_NonExistingPet_ReturnNotFoundPartialView()
        {
            // Arrange
            string petName = "Barney";
            SetControllerContext(petName);

            // Act
            var result = _controller.Display() as PartialViewResult;

            // Assert
            // The action method returned an action result
            Assert.IsNotNull(result);
            // The redirection actually happened
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            // It was redirected to the NotFound action method
            Assert.AreEqual("NotFound", result.ViewName);
        }

        [TestMethod]
        public void NotFoundError_ReturnsHttp404()
        {
            // Arrange


            // Act
            var result = _controller.NotFoundError() as HttpStatusCodeResult;

            // Assert
            // The action method returned an action result
            Assert.IsNotNull(result);
            // The action result is an HttpStatusCodeResult object
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            // The HTTP code is 404 (not found)
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void MissingPets_ReturnView()
        {
            // Arrange
            

            // Act
            ViewResult result = (ViewResult)_controller.MissingPets();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(IEnumerable<PetModel>));

            string[] names = { "Fido", "Connie" };
            int index = 0;
            IEnumerable<PetModel> list = (IEnumerable<PetModel>)result.Model;
            foreach (var item in list)
            {
                Assert.AreEqual(names[index++], item.PetName);
            }

        }

        [TestMethod]
        public void GetInfo_ExistingPet_ReturnJsonResult()
        {
            // Arrange
            string petName = "Fido";
            SetControllerContext(petName);

            // Act
            JsonResult result = (JsonResult)_controller.GetInfo();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.AreEqual(petName, ((PetModel)result.Data).PetName);
        }

        [TestMethod]
        public void GetInfo_NonExistingPet_ReturnEmptyJsonResult()
        {
            // Arrange
            string petName = "Barney";
            SetControllerContext(petName);

            // Act
            JsonResult result = (JsonResult)_controller.GetInfo();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.IsNull(result.Data);
        }

    }
}

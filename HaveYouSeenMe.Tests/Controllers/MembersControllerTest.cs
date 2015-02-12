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

using System.Web;
using System.Security.Principal;

namespace HaveYouSeenMe.Tests.Controllers
{
    [TestClass]
    public class MembersControllerTest
    {
        private MembersController _controller;

        public MembersControllerTest() 
        {
            Mock<IPetDao> _pet_repo = new Mock<IPetDao>();
            Mock<IMessageDao> _msg_repo = new Mock<IMessageDao>();

            _pet_repo.Setup(x => x.GetPetsFromOwner(It.Is<string>(y => y == "rbrito")))
                        .Returns(
                            new[] 
                            {
                                    new Models.Pet
                                    {
                                        PetID = 1,
                                        PetName = "Fido",
                                        Status = new Status { Description = "lost" },
                                        UserProfile = new UserProfile { UserName = "rbrito" }
                                    },
                                    new Models.Pet
                                    {
                                        PetID = 2,
                                        PetName = "Connie",
                                        Status = new Status { Description = "lost" },
                                        UserProfile = new UserProfile { UserName = "rbrito" }
                                    }   
                            }
                        );

            _pet_repo.Setup(x => x.GetPetById(It.Is<int>(y => y == 1)))
                        .Returns(
                                    new Models.Pet
                                    {
                                        PetID = 1,
                                        PetName = "Fido",
                                        Status = new Status { Description = "lost" },
                                        UserProfile = new UserProfile { UserName = "rbrito" }
                                    }
                        );

            _controller = new MembersController(_pet_repo.Object, _msg_repo.Object);

        }

        private void SetControllerContext(string userName, int petId = 0)
        {
            //for [Authorize] marked controllers, 
            //we need to mock HttpContextBase and ControllerContext
            //so we can hardcode our user sesion

            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity(userName);
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            
            _controller.ControllerContext = controllerContext.Object;

            //add route data to the controller context
            RouteData routeData = new RouteData();
            routeData.Values.Add("id", petId);
            _controller.ControllerContext.RouteData = routeData;            
        }

        [TestMethod]
        public void PetsOwned_ReturnView()
        {
            // Arrange
            string userName = "rbrito";
            SetControllerContext(userName);

            //Act
            ViewResult result = (ViewResult)_controller.PetsOwned();

            //Assert
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
        public void Details__ExistingPet_ReturnView() 
        {
            // Arrange
            string userName = "rbrito";
            int petId = 1;
            SetControllerContext(userName, petId);

            // Act
            var result = _controller.Details(petId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("../Pet/Display", result.ViewName);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(HaveYouSeenMe.Models.PetModel));
            Assert.AreEqual(petId, ((HaveYouSeenMe.Models.PetModel)result.Model).PetID);
        }

        [TestMethod]
        public void Details_NonExistingPet_ReturnNotFoundView()
        {
            // Arrange
            string userName = "rbrito";
            int petId = 9;
            SetControllerContext(userName, petId);

            // Act
            var result = _controller.Details(petId) as RedirectToRouteResult;

            //Assert
            // The action method returned an action result
            Assert.IsNotNull(result);
            // The redirection actually happened
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            // It was redirected to the NotFound action method
            Assert.AreEqual("NotFound", result.RouteValues["action"]);            
        }
    }
}

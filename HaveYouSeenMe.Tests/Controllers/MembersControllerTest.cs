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

            _controller = new MembersController(_pet_repo.Object, _msg_repo.Object);

        }

        private void SetControllerContext(string userName)
        {
            var user = new System.Web.Security.RolePrincipal
            (
                new System.Web.Security.FormsIdentity 
                ( 
                    new System.Web.Security.FormsAuthenticationTicket
                    (
                        userName, true, -1
                    )
                )
            );

            ////this is not working, can not test methods that require authetication.
            //_controller.User = user;
        }

        [TestMethod]
        public void PetsOwned_ReturnView()
        {
            // Arrange


            //Act
            ViewResult result = (ViewResult)_controller.PetsOwned();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(IEnumerable<PetModel>));

        }
    }
}

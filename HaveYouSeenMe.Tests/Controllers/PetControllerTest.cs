using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaveYouSeenMe;
using HaveYouSeenMe.Controllers;
using HaveYouSeenMe.Models.Business;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using HaveYouSeenMe.DAO;

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
                        .Returns(new Models.Pet { PetName = "Fido" });
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
        public void Display_ExistingPet_ReturnView()
        {
            // Arrange
            string petName = "Fido";
            SetControllerContext(petName);

            // Act
            ViewResult result = (ViewResult)_controller.Display();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("", result.ViewName);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(HaveYouSeenMe.Models.Pet));
            Assert.AreEqual(petName, ((HaveYouSeenMe.Models.Pet)result.Model).PetName);
        }

        [TestMethod]
        public void Display_NonExistingPet_ReturnNotFoundView()
        {
            // Arrange
            string petName = "Barney";
            SetControllerContext(petName);

            // Act
            var result = _controller.Display() as RedirectToRouteResult;

            // Assert
            // The action method returned an action result
            Assert.IsNotNull(result);
            // The redirection actually happened
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            // It was redirected to the NotFound action method
            Assert.AreEqual("NotFound", result.RouteValues["action"]);
        }

    }
}

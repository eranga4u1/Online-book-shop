using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Online_book_shop.Controllers;
using System.Web.Mvc;
using System.Linq;
namespace OnlineBookshopTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            HomeController controller = new HomeController();
           
            //Act
            ViewResult results= controller.About() as ViewResult;

            //Assert
            Assert.AreNotEqual(results,"");

        }
    }
}

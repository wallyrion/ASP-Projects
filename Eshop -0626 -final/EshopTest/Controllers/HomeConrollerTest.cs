using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Eshop.Controllers;

namespace EshopTest.Controllers
{
    [TestClass]
    public class HomeConrollerTest
    {
        public void Category()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);

        }
    }
}

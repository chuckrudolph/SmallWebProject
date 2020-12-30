using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class CalculatorController : Controller
    {
        // GET: Calculator
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]

        //It creates a calculator object to perform the math operation specified and returns the result in a string
        public JsonResult Calculate(string firstNumber, string secondNumber, string mathOperation)
        {
            double firstValue, secondValue;

            try //if the numbers are not valid, return an empty string
            {
                firstValue = Convert.ToDouble(firstNumber);
                secondValue = Convert.ToDouble(secondNumber);
            }
            catch (FormatException e)
            {
                return Json(" ", JsonRequestBehavior.AllowGet);
            }

            return Json((new Calculator(firstValue, secondValue, mathOperation)).result, JsonRequestBehavior.AllowGet);
        }
    }
}
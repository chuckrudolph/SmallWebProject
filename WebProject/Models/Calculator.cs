using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public class Calculator
    {
        public string result { get; }

        public Calculator(double firstNumber, double secondNumber, string symbol)
        {
            switch(symbol)
            {
                case "+":
                    result = $"{firstNumber + secondNumber}";
                    break;
                case "-":
                    result = $"{firstNumber - secondNumber}";
                    break;
                case "*":
                    result = $"{firstNumber * secondNumber}";
                    break;
                case "/":
                    result = $"{firstNumber / secondNumber}";
                    break;
                default: result = " ";
                    break;
            }
        }
    }
}
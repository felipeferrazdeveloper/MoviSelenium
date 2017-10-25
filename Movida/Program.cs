using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movida
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Url = "https://www.movida.com.br";
            driver.Manage().Window.Maximize(); //É necessário maximizar a janela para o chat do facebook não ficar minimizado
            var tempo = TimeSpan.FromSeconds(20);            
            driver.Manage().Timeouts().ImplicitWait = tempo;



            driver.Close();
        }
    }
}

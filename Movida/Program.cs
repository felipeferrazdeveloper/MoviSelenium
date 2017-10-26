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
            driver.Manage().Window.Maximize(); 
            var tempo = TimeSpan.FromSeconds(20);
            driver.Manage().Timeouts().ImplicitWait = tempo;

            driver.Navigate().GoToUrl("http://www.movida.com.br"); //Não sei porquê, mas precsa navegar até essa URL antes para a próxima funcionar
            //driver.Navigate().GoToUrl("http://www.movida.com.br/parcerias"); 

            driver.FindElement(By.ClassName("cd-dropdown-trigger")).Click();             
            //driver.FindElement(By.XPath("//div[@class='cd-dropdown-wrapper']//nav[@class='cd-dropdown']//ul[@class='cd-dropdown-content']//li[@class='has-children']//a[@title='A Movida']")).Click();
            driver.Close();
        }
    }
}

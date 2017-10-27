using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
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
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Window.Maximize(); 
            var tempo = TimeSpan.FromSeconds(20);
            driver.Manage().Timeouts().ImplicitWait = tempo;

            driver.Navigate().GoToUrl("http://www.movida.com.br"); 
            
            //Clica no Menu principal
            driver.FindElement(By.ClassName("cd-dropdown-trigger")).Click();

            Actions mouseMove = new Actions(driver);
            mouseMove.MoveToElement(driver.FindElement(By.XPath("//div[@class= 'cd-dropdown-wrapper']/*/ul//a[@title='A Movida']"))).Build().Perform();
            driver.FindElement(By.XPath("//div[@class= 'cd-dropdown-wrapper']//ul//a[@title='A Movida']/following-sibling::ul//a[@title='Parcerias']")).Click();

            driver.Close();
        }
    }
}

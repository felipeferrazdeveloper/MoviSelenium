using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using System.Threading;

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

            driver.Navigate().GoToUrl("http://www.movida.com.br"); 
            
            //Clica no Menu principal
            driver.FindElement(By.ClassName("cd-dropdown-trigger")).Click();

            aguardarVisibilidadeDoElemento("//ul[@class='cd-dropdown-content']", driver);

            Actions mouseMove = new Actions(driver);
            mouseMove.MoveToElement(driver.FindElement(By.XPath("//div[@class= 'cd-dropdown-wrapper']/*/ul//a[@title='A Movida']"))).Build().Perform();

            aguardarVisibilidadeElemento("//div[@class= 'cd-dropdown-wrapper']//ul//a[@title='A Movida']/following-sibling::ul//a[@title='Parcerias']", driver).Click();
            
            var parcerias = driver.FindElements(By.XPath("//div[@class='row parcerias-list']//div")).Count;       
            for(int i =1; i<=parcerias; i++)
            {
                var xpath = String.Format("//div[@class='row parcerias-list']//div[{0}]", i);
                driver.FindElement(By.XPath(xpath)).Click();
                driver.FindElement(By.XPath("//div[contains(@class, 'parcerias')]//a[@class='selclicksubmit']")).Click();

                setOrigem("JUIZ DE FORA", "03/11/2017", "08", "30", driver);
                setDestino("JUIZ DE FORA", "06/11/2017", "08", "30", driver);

                driver.FindElement(By.XPath("//a[@class='nextForm']")).Click();


            }     
            driver.Close();
        }
        public static void setOrigem(string loja, string data, string hora, string minuto, IWebDriver driver)
        {

            driver.FindElement(By.Id("LojaR")).Clear();
            driver.FindElement(By.Id("LojaR")).SendKeys(loja);

            aguardarVisibilidadeDoElemento("//li[@class='ui-menu-item']", driver);
            
            driver.FindElement(By.Id("LojaR")).SendKeys(Keys.Enter);

            driver.FindElement(By.Id("R_Data")).Clear();
            driver.FindElement(By.Id("R_Data")).SendKeys(data);
            driver.FindElement(By.Id("R_Data")).SendKeys(Keys.Tab);
            
            var horaInput = new SelectElement(driver.FindElement(By.XPath("//Select[@name='R_Hora']")));
            horaInput.SelectByText(hora);

            var minutoInput = new SelectElement(driver.FindElement(By.XPath("//Select[@name='R_Minuto']")));
            minutoInput.SelectByText(minuto);
        }
       

        public static void setDestino(string loja, string data, string hora, string minuto, IWebDriver driver)
        {
            driver.FindElement(By.Id("LojaD")).Clear();
            driver.FindElement(By.Id("LojaD")).SendKeys(loja);

            aguardarVisibilidadeDoElemento("//li[@class='ui-menu-item']", driver);
            driver.FindElement(By.Id("LojaD")).SendKeys(Keys.Enter);

            driver.FindElement(By.Id("D_Data")).Clear();
            driver.FindElement(By.Id("D_Data")).SendKeys(data);
            driver.FindElement(By.Id("D_Data")).SendKeys(Keys.Tab);

            var horaInput = new SelectElement(driver.FindElement(By.XPath("//Select[@name='D_Hora']")));
            horaInput.SelectByText(hora);

            var minutoInput = new SelectElement(driver.FindElement(By.XPath("//Select[@name='D_Minuto']")));
            minutoInput.SelectByText(minuto);
        }

        public static void aguardarVisibilidadeDoElemento(string xPath, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
        }

        public static IWebElement aguardarVisibilidadeElemento(string xPath, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));

            return element;
        }
    }
    
}


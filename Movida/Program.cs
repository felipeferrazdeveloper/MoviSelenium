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
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize(); 
            var tempo = TimeSpan.FromSeconds(20);
            driver.Manage().Timeouts().ImplicitWait = tempo;

            driver.Navigate().GoToUrl("http://www.movida.com.br"); 
            
            //Clica no Menu principal
            driver.FindElement(By.ClassName("cd-dropdown-trigger")).Click();
            
            Actions mouseMove = new Actions(driver);
            mouseMove.MoveToElement(driver.FindElement(By.XPath("//div[@class= 'cd-dropdown-wrapper']/*/ul//a[@title='A Movida']"))).Build().Perform();            
            driver.FindElement(By.XPath("//div[@class= 'cd-dropdown-wrapper']//ul//a[@title='A Movida']/following-sibling::ul//a[@title='Parcerias']")).Click();
            
            var parcerias = driver.FindElements(By.XPath("//div[@class='row parcerias-list']//div")).Count;       
            for(int i =1; i<=parcerias; i++)
            {
                var xpath = String.Format("//div[@class='row parcerias-list']//div[{0}]", i);
                driver.FindElement(By.XPath(xpath)).Click();
                driver.FindElement(By.XPath("//div[contains(@class, 'parcerias')]//a[@class='selclicksubmit']")).Click();

                setOrigem("JUIZ DE FORA", "03/11/2017", 9, "30", driver);
                setDestino("JUIZ DE FORA", "06/11/2017", "08", "30", driver);

                driver.FindElement(By.XPath("//a[@class='nextForm']")).Click();


            }     
            driver.Close();
        }
        public static void setOrigem(string loja, string data, int hora, string minuto, IWebDriver context)
        {
            context.FindElement(By.Id("LojaR")).SendKeys(loja);
            context.FindElement(By.Id("LojaR")).SendKeys(Keys.Enter);

            context.FindElement(By.Id("R_Data")).SendKeys(data);
            context.FindElement(By.Id("R_Data")).SendKeys(Keys.Enter);

            context.FindElement(By.XPath("//Select[@id='R_Hora']/option[" +hora+ "]")).Click();
            context.FindElement(By.Id("R_Hora")).SendKeys(Keys.Tab);

            context.FindElement(By.Id("R_Minuto")).SendKeys(minuto);
            context.FindElement(By.Id("R_Minuto")).SendKeys(Keys.Enter);
        }
        public static void setDestino(string loja, string data, string hora, string minuto, IWebDriver context)
        {
            context.FindElement(By.Id("LojaD")).SendKeys(loja);
            context.FindElement(By.Id("LojaD")).SendKeys(Keys.Enter);

            context.FindElement(By.Id("D_Data")).SendKeys(data);
            context.FindElement(By.Id("D_Data")).SendKeys(Keys.Enter);

            context.FindElement(By.Id("D_Hora")).SendKeys(hora);
            context.FindElement(By.Id("D_Hora")).SendKeys(Keys.Enter);

            context.FindElement(By.Id("D_Minuto")).SendKeys(minuto);
            context.FindElement(By.Id("D_Minuto")).SendKeys(Keys.Enter);
        }
    }
    
}


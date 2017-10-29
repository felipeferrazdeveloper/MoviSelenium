using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
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
        static IWebDriver driver = new ChromeDriver();
        static WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));

        static void Main(string[] args)
        {
            inicializar("http://www.movida.com.br");
            navegarParaParcerias();

            var parcerias = new List<Parceiro>();
            var qtidadeParcerias = contarNumeroElementos("//div[@class='row parcerias-list']//div");
            for (int i = 1; i <= qtidadeParcerias; i++)
            {
                preencherFormulario("JUIZ DE FORA", "03/11/2017", "08", "30", "03/11/2017", "15", "30", i);

                if (!driver.Url.Equals("https://www.movida.com.br/") && driver.Url.Contains("movida.com.br") && !driver.Url.Contains("entidades-de-classe"))
                {
                    aguardarVisibilidadeDoElemento("//div[@id='DIV_Escolha']");
                    var nomeParceiro = capturarTexto("//div[contains(@class, 'flags-header')]//div//span[@class='semi-bold mrs']");

                    var parceiro = new Parceiro();
                    parceiro.nome = nomeParceiro;

                    var carros = contarNumeroElementos("//div[contains(@class, 'reserva')]//div[@id='DIV_Escolha']//div//div[contains(@class, 'valorDiaria')]");
                    for (int cont = 1; cont <= carros; cont++)
                    {
                        var nomeCarro = capturarTexto(string.Format("//div[@id='DIV_Escolha']//div[{0}]//div//h2//following-sibling::b[1]", cont));
                        nomeCarro = removerCaracterTexto(nomeCarro, ")", "(");

                        var valorCarro = capturarTexto(string.Format("//div[@id='DIV_Escolha']//div[{0}]//div[@class='valor']", cont));

                        var carro = new Carro();
                        carro.modelo = nomeCarro;
                        carro.preco = valorCarro;
                        parceiro.adicionarCarro(carro);
                    }
                    parceiro.calcularMediaPreco();
                    parcerias.Add(parceiro);
                    //Console.WriteLine(parceiro.nome + " ----- R$" + parceiro.media);
                    inicializar("http://www.movida.com.br");
                }
                navegarParaParcerias();
            }
            exibirTabela(parcerias);
            exibirOrdem(parcerias);
            ordenarMelhoresParceiros(parcerias);
            driver.Close();
        }

        public static void inicializar(string site)
        {
            var tempo = TimeSpan.FromSeconds(5);
            driver.Manage().Timeouts().ImplicitWait = tempo;

            driver.Navigate().GoToUrl(site);
        }

        public static void setOrigem(string loja, string data, string hora, string minuto)
        {

            driver.FindElement(By.Id("LojaR")).Clear();
            driver.FindElement(By.Id("LojaR")).SendKeys(loja);

            aguardarVisibilidadeDoElemento("//li[@class='ui-menu-item']");
            
            driver.FindElement(By.Id("LojaR")).SendKeys(Keys.Enter);

            driver.FindElement(By.Id("R_Data")).Clear();
            driver.FindElement(By.Id("R_Data")).SendKeys(data);
            driver.FindElement(By.Id("R_Data")).SendKeys(Keys.Tab);
            
            var horaInput = new SelectElement(driver.FindElement(By.XPath("//Select[@name='R_Hora']")));
            horaInput.SelectByText(hora);

            var minutoInput = new SelectElement(driver.FindElement(By.XPath("//Select[@name='R_Minuto']")));
            minutoInput.SelectByText(minuto);
        }
       

        public static void setDestino(string loja, string data, string hora, string minuto)
        {
            driver.FindElement(By.Id("LojaD")).Clear();
            driver.FindElement(By.Id("LojaD")).SendKeys(loja);

            aguardarVisibilidadeDoElemento("//li[@class='ui-menu-item']");
            driver.FindElement(By.Id("LojaD")).SendKeys(Keys.Enter);

            driver.FindElement(By.Id("D_Data")).Clear();
            driver.FindElement(By.Id("D_Data")).SendKeys(data);
            driver.FindElement(By.Id("D_Data")).SendKeys(Keys.Tab);

            var horaInput = new SelectElement(driver.FindElement(By.XPath("//Select[@name='D_Hora']")));
            horaInput.SelectByText(hora);

            var minutoInput = new SelectElement(driver.FindElement(By.XPath("//Select[@name='D_Minuto']")));
            minutoInput.SelectByText(minuto);
        }

        public static void preencherFormulario(string loja, string dataR, string horaR, string minutoR, string dataD, string horaD, string minutoD, int i = 1)
        {
            var fail = false;
            var xpath = String.Format("//div[@class='row parcerias-list']//div[{0}]", i);
            driver.FindElement(By.XPath(xpath)).Click();

            if (!driver.Url.Contains("entidades-de-classe"))
            {
                if (driver.FindElements(By.XPath("//a[contains(text(), 'Para acumular pontos')]")).Count == 0)
                {
                    driver.FindElement(By.XPath("//div[contains(@class, 'parcerias')]//a[@class='selclicksubmit']")).Click();
                    fail = true;
                }
                else
                {
                    driver.FindElement(By.XPath("//a[contains(text(), 'Para acumular pontos')]")).Click();
                    driver.Close();
                    driver.SwitchTo().Window(driver.WindowHandles.Last());
                    aguardarPaginaCarregar();
                    if (driver.Url.Contains("movida.com.br/reserva"))
                    {
                        fail = true;
                    }
                }
            }

            if (fail && driver.Url.Contains("movida.com.br"))
            {
                setOrigem(loja, dataR, horaR, minutoR);
                setDestino(loja, dataD, horaD, minutoD);
                driver.FindElement(By.XPath("//a[@class='nextForm']")).Click();
            }            
        }

        public static int contarNumeroElementos(string xpath)
        {
            return driver.FindElements(By.XPath(xpath)).Count;
        }

        public static string capturarTexto(string xpath)
        {
            return driver.FindElement(By.XPath(xpath)).Text;
        }

        public static void navegarParaParcerias()
        {
            if (!driver.Url.Contains("movida.com.br"))
                inicializar("http://www.movida.com.br");
            driver.FindElement(By.ClassName("cd-dropdown-trigger")).Click();

            aguardarVisibilidadeDoElemento("//ul[@class='cd-dropdown-content']");

            Actions mouseMove = new Actions(driver);
            mouseMove.MoveToElement(driver.FindElement(By.XPath("//div[@class= 'cd-dropdown-wrapper']/*/ul//a[@title='A Movida']"))).Build().Perform();

            aguardarVisibilidadeElemento("//div[@class= 'cd-dropdown-wrapper']//ul//a[@title='A Movida']/following-sibling::ul//a[@title='Parcerias']").Click();
        }

        public static string removerCaracterTexto(string texto, string caracter1 = null, string caracter2 = null)
        {
           return texto.Replace(caracter1, String.Empty).Replace(caracter2, String.Empty);
        }

        public static void aguardarVisibilidadeDoElemento(string xPath)
        {
            
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
        }

        public static void aguardarClicabilidadeDoElemento(string xPath)
        {

            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xPath)));
        }

        public static IWebElement aguardarVisibilidadeElemento(string xPath)
        {            
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));

            return element;
        }

        public static void aguardarPaginaCarregar()
        {
            IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30.00));
            wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static void ordenarMelhoresParceiros(List<Parceiro> parceiros)
        {
            List<Parceiro> listaOrdenada = parceiros.OrderByDescending(o => o.media).ToList();

            Console.WriteLine("\n\nLista de parceiros ordenada do menos rentável para o mais rentável: \n");
            foreach (var order in listaOrdenada)
            {
                if (order.media != null)
                    Console.WriteLine("-> " + order.nome + " (média: " + order.media + ")\n");
                else
                    Console.WriteLine(order.nome + " :::: Error! Consulta falhou.");
            }
        }

        public static void exibirOrdem(List<Parceiro> parceiros)
        {
            Console.WriteLine("\n\nLista de parceiros: \n");
            foreach (var order in parceiros)
            {
                if(order.media != null)
                    Console.WriteLine("-> " + order.nome + " (média: " + order.media + ")\n");
                else
                    Console.WriteLine(order.nome + " :::: Error! Consulta falhou.");
            }
        }

        public static void exibirTabela(List<Parceiro> parceiros)
        {
            string text = string.Empty;
            foreach (var parceiro in parceiros)
            {
                Console.WriteLine("\n\n------ " + parceiro.nome + " ------\n");
                foreach (var carro in parceiro.carros)
                {
                    Console.WriteLine("-> " + carro.modelo + " = " + carro.preco + "\n");
                }
            }
        }
    }
    
}


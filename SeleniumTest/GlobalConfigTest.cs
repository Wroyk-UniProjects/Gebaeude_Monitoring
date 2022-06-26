namespace SeleniumTest
{
    public class ClobalConfigTest
    {
        String Url = "https://localhost:7036/";
        IWebDriver driver;

        [SetUp]
        public void Start_Browser()
        {
            var chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments("--headless");
            chromeOptions.AddArguments("--disable-gpu");
            chromeOptions.AddArguments("--window-size=1280,800");
            chromeOptions.AddArguments("--allow-insecure-localhost");
            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test_Main_Page()
        {
            driver.Url = Url;
            Thread.Sleep(10000); // wait time for site to load

            // click on global

            var GlobalConfigButton = driver.FindElement(By.XPath("//li[text()=\"Global Config\"]/.."));
            GlobalConfigButton.Click();
            Thread.Sleep(3000);

            // check header

            bool LogoVisible = driver.FindElement(By.XPath("//*[@id=\"app\"]/div/header/div/h1/img")).Displayed;
            bool HeadlineVisible = driver.FindElement(By.XPath("//a[text()=\"Building Monitoring\"]")).Displayed;
            bool GlobalConfigVisible = driver.FindElement(By.XPath("//li[text()=\"Global Config\"]")).Displayed;
            bool IndividualConfigVisible = driver.FindElement(By.XPath("//li[text()=\"Individual Config\"]")).Displayed;

            // check setting options
            // following-sibling::div//button
            String XPathGlobal = "//h1[text()=\"Global Settings\"]";
            String XPathTargetTemp = "//p[text()=\"Target Temperature:\"]";
            String XPathTargetHumid = "//p[text()=\"Target Humidity:\"]";
            String XPathUpdateRate = "//p[text()=\"Update Rate:\"]";
            String XPathTolerance = "//p[text()=\"Tolerance:\"]";
            bool GlobalHeadLine = driver.FindElement(By.XPath(XPathGlobal)).Displayed;
            bool TargetTempText = driver.FindElement(By.XPath(XPathTargetTemp)).Displayed;
            bool TargetHumidText = driver.FindElement(By.XPath(XPathTargetHumid)).Displayed;
            bool UpdateRateText = driver.FindElement(By.XPath(XPathUpdateRate)).Displayed;
            bool ToleranceText = driver.FindElement(By.XPath(XPathTolerance)).Displayed;

            String MinusButton = "[@class = \"minus-btn\"]";
            String PlusButton = "[@class = \"plus-btn\"]";
            String Values = "/following-sibling::div//p[@class=\"settings-values\" and text()]";

            Assert.Multiple(() =>
            {
               
            });
        }

        [TearDown]
        public void Close_Browser()
        {
            driver.Quit();
        }
    }
}
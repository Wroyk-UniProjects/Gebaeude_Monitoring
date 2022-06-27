namespace SeleniumTest
{
    public class ClobalConfigTest
    {
        String Url = "https://172.17.0.3:7036/";
        IWebDriver driver;

        [SetUp]
        public void Start_Browser()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");
            chromeOptions.AddArguments("--disable-gpu");
            chromeOptions.AddArguments("--window-size=1280,800");
            chromeOptions.AddArguments("--allow-insecure-localhost");
            chromeOptions.AddArguments("--no-sandbox");
            chromeOptions.AddArguments("--disable-dev-shm-usage");
            chromeOptions.AddArguments("--ignore-ssl-errors=yes");
            chromeOptions.AddArguments("--ignore-certificate-errors");
            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test_Main_Page()
        {
            driver.Url = "https://www.google.de";
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

            String MinusButton = "/following-sibling::div/button[@class = \"minus-btn\"]";
            String PlusButton = "/following-sibling::div/button[@class = \"plus-btn\"]";
            String Values = "/following-sibling::div//p[@class=\"settings-values\" and text()]";

            bool TargetTempBtnM = driver.FindElement(By.XPath(XPathTargetTemp + MinusButton)).Displayed;
            bool TargetTempBtnP = driver.FindElement(By.XPath(XPathTargetTemp + PlusButton)).Displayed;
            bool TargetTempValues = driver.FindElement(By.XPath(XPathTargetTemp + Values)).Displayed;

            bool TargetHumidBtnM = driver.FindElement(By.XPath(XPathTargetHumid + MinusButton)).Displayed;
            bool TargetHumidBtnP = driver.FindElement(By.XPath(XPathTargetHumid + PlusButton)).Displayed;
            bool TargetHumidValues = driver.FindElement(By.XPath(XPathTargetHumid + Values)).Displayed;

            bool UpdateRateBtnM = driver.FindElement(By.XPath(XPathUpdateRate + MinusButton)).Displayed;
            bool UpdateRateBtnP = driver.FindElement(By.XPath(XPathUpdateRate + PlusButton)).Displayed;
            bool UpdateRateValues = driver.FindElement(By.XPath(XPathUpdateRate + Values)).Displayed;

            bool RoleranceBtnM = driver.FindElement(By.XPath(XPathTolerance + MinusButton)).Displayed;
            bool ToleranceBtnP = driver.FindElement(By.XPath(XPathTolerance + PlusButton)).Displayed;
            bool ToleranceValues = driver.FindElement(By.XPath(XPathTolerance + Values)).Displayed;

            bool SaveBtn = driver.FindElement(By.XPath("//button[text()=\"Save\"]")).Displayed;
            bool CloseBtn = driver.FindElement(By.XPath("//button[text()=\"Cancel\"]")).Displayed;

            Assert.Multiple(() =>
            {
                Assert.That(LogoVisible, Is.True);
                Assert.That(HeadlineVisible, Is.True);
                Assert.That(GlobalConfigVisible, Is.True);
                Assert.That(IndividualConfigVisible, Is.True);
                Assert.That(GlobalHeadLine, Is.True);
                Assert.That(TargetTempText, Is.True);
                Assert.That(TargetHumidText, Is.True);
                Assert.That(UpdateRateText, Is.True);
                Assert.That(ToleranceText, Is.True);
                Assert.That(TargetTempBtnM, Is.True);
                Assert.That(TargetTempBtnP, Is.True);
                Assert.That(TargetTempValues, Is.True);
                Assert.That(TargetHumidBtnM, Is.True);
                Assert.That(TargetHumidBtnP, Is.True);
                Assert.That(TargetHumidValues, Is.True);
                Assert.That(UpdateRateBtnM, Is.True);
                Assert.That(UpdateRateBtnP, Is.True);
                Assert.That(UpdateRateValues, Is.True);
                Assert.That(RoleranceBtnM, Is.True);
                Assert.That(ToleranceBtnP, Is.True);
                Assert.That(ToleranceValues, Is.True);

            });
        }

        [TearDown]
        public void Close_Browser()
        {
            driver.Quit();
        }
    }
}
namespace SeleniumTest
{
    public class MainPageTest
    {
        String Url = "https://localhost:7036/";
        IWebDriver driver;

        [SetUp]
        public void Start_Browser()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");
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

            // check header

            bool LogoVisible = driver.FindElement(By.XPath("//*[@id=\"app\"]/div/header/div/h1/img")).Displayed;
            bool HeadlineVisible = driver.FindElement(By.XPath("//a[text()=\"Building Monitoring\"]")).Displayed;
            bool GlobalConfigVisible = driver.FindElement(By.XPath("//li[text()=\"Global Config\"]")).Displayed;
            bool IndividualConfigVisible = driver.FindElement(By.XPath("//li[text()=\"Individual Config\"]")).Displayed;

            // check rooms are displayed
            bool RoomOneVisible = driver.FindElement(By.XPath("//div[contains(@class, \"justify-evenly\")]//p[text()=\"Room 1\"]/../..")).Displayed;
            bool RoomTwoVisible = driver.FindElement(By.XPath("//div[contains(@class, \"justify-evenly\")]//p[text()=\"Room 2\"]/../..")).Displayed;
            bool RoomThreeVisible = driver.FindElement(By.XPath("//div[contains(@class, \"justify-evenly\")]//p[text()=\"Room 3\"]/../..")).Displayed;
            bool RoomFourVisible = driver.FindElement(By.XPath("//div[contains(@class, \"justify-evenly\")]//p[text()=\"Room 4\"]/../..")).Displayed;
            bool RoomFiveVisible = driver.FindElement(By.XPath("//div[contains(@class, \"justify-evenly\")]//p[text()=\"Room 5\"]/../..")).Displayed;

            bool FloorImageRoomOne = driver.FindElement(By.XPath("//div[contains(@class, \"justify-evenly\")]//p[text()=\"Room 1\"]/../..//img")).Displayed;
            bool FloorImageRoomTWo = driver.FindElement(By.XPath("//div[contains(@class, \"justify-evenly\")]//p[text()=\"Room 2\"]/../..//img")).Displayed;
            bool FloorImageRoomThree = driver.FindElement(By.XPath("//div[contains(@class, \"justify-evenly\")]//p[text()=\"Room 3\"]/../..//img")).Displayed;
            bool FloorImageRoomFour = driver.FindElement(By.XPath("//div[contains(@class, \"justify-evenly\")]//p[text()=\"Room 4\"]/../..//img")).Displayed;
            bool FloorImageRoomFive = driver.FindElement(By.XPath("//div[contains(@class, \"justify-evenly\")]//p[text()=\"Room 5\"]/../..//img")).Displayed;

            Assert.Multiple(() =>
            {
                Assert.That(LogoVisible, Is.True);
                Assert.That(HeadlineVisible, Is.True);
                Assert.That(GlobalConfigVisible, Is.True);
                Assert.That(IndividualConfigVisible, Is.True);
                Assert.That(RoomOneVisible, Is.True);
                Assert.That(RoomTwoVisible, Is.True);
                Assert.That(RoomThreeVisible, Is.True);
                Assert.That(RoomFourVisible, Is.True);
                Assert.That(RoomFiveVisible, Is.True);
                Assert.That(FloorImageRoomOne, Is.True);
                Assert.That(FloorImageRoomTWo, Is.True);
                Assert.That(FloorImageRoomThree, Is.True);
                Assert.That(FloorImageRoomFour, Is.True);
                Assert.That(FloorImageRoomFive, Is.True);
            });
        }

        [TearDown]
        public void Close_Browser()
        {
            driver.Quit();
        }
    }
}
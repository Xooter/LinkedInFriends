using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using LinkedInFriends.Helpers;

namespace LinkedInFriends.Models;

public class LinkedIn
{
    ChromeDriver driver;

    Config config;

    public LinkedIn()
    {
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--window-size=400,800");
        driver = new ChromeDriver(options: options);

        driver.Navigate().GoToUrl("https://www.linkedin.com/mynetwork/");

        SecretManager secretManager = new SecretManager("config.json");

        config = secretManager.LoadConfig() ?? new Config();
    }

    public void Login()
    {

        string usr = config.user;
        string pwd = config.pwd;

        driver.FindElement(By.Name("session_key")).SendKeys(usr);
        var pwdBtn = driver.FindElement(By.Name("session_password"));

        pwdBtn.SendKeys(pwd);
        pwdBtn.SendKeys(Keys.Enter);

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1000));

        wait.Until(driver => driver.FindElements(By.ClassName("discover-sections-list__item")).Count > 0);

        CloseMessageDialog();
    }

    public void FollowUserInDialog()
    {
        OpenFollowDialog();

        // modal de usuarios sugeridos
        IWebElement modal = driver.FindElements(By.CssSelector("div.artdeco-modal__content")).First();

        int follows = 0;
        while (follows < 1000)
        {
            WebDriverWait waitToLoad = new WebDriverWait(driver, TimeSpan.FromSeconds(1000));

            waitToLoad.Until(driver => driver.FindElements(By.ClassName("discover-sections-list__item")).Count > 0);

            waitToLoad.Until(driver =>
            {
                var btns = driver.FindElements(By.CssSelector("div.artdeco-modal__content ul.discover-fluid-entity-list button"));
                return btns.Count / 2 != follows;
            });

            var followBtns = driver.FindElements(By.CssSelector("div.artdeco-modal__content ul.discover-fluid-entity-list button"));
            Console.WriteLine($"FollowBtns: {followBtns.Count}");

            foreach (var btn in followBtns)
            {
                if (btn.Text == "Follow" || btn.Text == "Connect")
                {
                    Thread.Sleep(300);
                    Console.WriteLine($"Follow: {btn.GetAttribute("aria-label")}");
                    btn.Click();
                    DriverManager.ScrollToElement(driver, modal);
                    follows++;
                }
            }
            DriverManager.ScrollDivDown(driver, modal, 500);
        }
    }

    private void OpenFollowDialog()
    {
        var allBtns = driver.FindElements(By.CssSelector("button.artdeco-button"));
        foreach (var btn in allBtns)
        {
            if (btn.Text == "See all")
            {
                DriverManager.ScrollDown(driver, 200);
                DriverManager.ScrollToElement(driver, btn);
                btn.Click();
                break;
            }
        }
    }

    private void CloseMessageDialog()
    {
        IWebElement msg = driver.FindElements(By.CssSelector("button.msg-overlay-bubble-header__control"))[1];
        msg.Click();
    }

}

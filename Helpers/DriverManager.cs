using OpenQA.Selenium;

namespace LinkedInFriends.Helpers;

public static class DriverManager
{

    public static void ScrollToElement(IWebDriver driver, IWebElement element, int Threshold = 0)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'nearest' });", element);
        ScrollDown(driver, Threshold);
    }

    public static void ScrollDivDown(IWebDriver driver, IWebElement divPadre, int pixels)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].scrollTop += arguments[1];", divPadre, pixels);
    }

    public static void ScrollDown(IWebDriver driver, int pixels)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"window.scrollBy(0, {pixels});");
    }
}

using System.Configuration;

namespace apistation.owin
{
    public class ApiOptions
    {
        public static string AppStartup { get { return ConfigurationManager.AppSettings["owin: AppStartup"]; } }

        public static string Url { get { return ConfigurationManager.AppSettings["api:Url"]; } }
    }
}
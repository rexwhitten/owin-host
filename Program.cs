using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace apistation.owin
{
    public static class Extensions
    {
        public static string ReadAsString(this Stream stream)
        {
            // convert stream to string
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static async Task<string> ReadAsStringAsync(this Stream stream)
        {
            // convert stream to string
            StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public static JObject ToJObject(this Stream stream)
        {
            return JObject.Parse(stream.ReadAsString());
        }


    }

    public class Program
    {
        public static IDictionary<string, string> Options { get; set; }

        /// <summary>
        /// Main Program Entry Points
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            #region load config

            var options = new StartOptions();
            options.AppStartup = ApiOptions.AppStartup;
            options.Urls.Add(ApiOptions.Url); ;

            #endregion load config

            using (WebApp.Start<ApiStartup>(options))
            {
                Console.WriteLine("Ready.");
                Console.WriteLine(ApiOptions.Url);
                Console.ReadKey();
            }
        }
    }
}
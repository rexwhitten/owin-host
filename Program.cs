using System;
using Microsoft.Owin.Hosting;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public static Task<string> ReadAsStringAsync(this Stream stream)
        {
            // convert stream to string
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEndAsync();
        }

        public static JObject ToJObject(this Stream stream)
        {
            return JObject.Parse(stream.ReadAsString());
        }
    }

    public class Program
    {
        public static IDictionary<string,string> Options { get; set; }

        /// <summary>
        /// Main Program Entry Points
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            #region load config
            var options = new StartOptions();
            options.AppStartup = "ApiStartup";
            options.Urls.Add("http://127.0.0.1:9980");
            Options = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText("server.json"));
            #endregion

            if (args.Length > 0)
            {
                #region custom arguments (input minimization)
                options.Urls.Add(args[0]); // owin http://127.0.0.1:9980
                options.AppStartup = args[1]; // owin http://127.0.0.1:9980 ApiStartup
                                              // owin http://127.0.0.1:9980 ApiStartup server.config

                #endregion
            }

            using (WebApp.Start<ApiStartup>("http://127.0.0.1:9980"))
            {
                Console.WriteLine("Ready.");
                Console.ReadKey();
            }
        }
    }
}

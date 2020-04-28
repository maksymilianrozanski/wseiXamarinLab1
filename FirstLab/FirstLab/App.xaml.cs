using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FirstLab.db;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace FirstLab
{
    public partial class App : Application
    {
        public static string ApiKey;
        private static DatabaseHelper _database;

        public App()
        {
            InitializeComponent();
            var assembly = Assembly.GetExecutingAssembly();
            ReadApiKey(assembly);

            MainPage = new NavigationPage(new MainTabbedPage());
        }

        public static DatabaseHelper Database => _database ?? (_database = new DatabaseHelper());

        private void ReadApiKey(Assembly assembly)
        {
            var devApiResourceName = assembly.GetManifestResourceNames()
                .SingleOrDefault(it => it.EndsWith("apiKey.json"));
            if (devApiResourceName == null)
                throw new Exception(
                    "Did you forget to include apiKey.json? apiKey.json placed in project root should contain api key: '{\n  \"key\": \"your api key\"\n}'.");
            var apiKeyJson = ReadResource(assembly, devApiResourceName);
            ApiKey = JObject.Parse(apiKeyJson)["key"].Value<string>();
        }

        private string ReadResource(Assembly assembly, string resourceName)
        {
            using (var reader = new StreamReader(
                assembly.GetManifestResourceStream(resourceName) ??
                throw new Exception("Resource not found, resource name: " + resourceName)))
            {
                return reader.ReadToEnd();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
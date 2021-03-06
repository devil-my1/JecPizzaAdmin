using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace JecPizza
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new App();
            app.InitializeComponent();
            try
            {
                app.Run();

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        public static IHostBuilder GetHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseContentRoot(Environment.CurrentDirectory).ConfigureAppConfiguration(
                (h, cnfg) =>
                {
                    cnfg.AddJsonFile("AppConfiguration.json", true, true);
                    cnfg.SetBasePath(Environment.CurrentDirectory);
                }).ConfigureServices(App.ConfigureServices);
    }
}
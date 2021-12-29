using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using JecPizza.Infostructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JecPizza
{

    public partial class App : Application
    {
        private static IHost _host;

        public static bool IsDesignMode { get; set; } = true;
        public static Window GetActiveWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);
        public static IHost Host => _host ??= Program.GetHostBuilder(Environment.GetCommandLineArgs()).Build();
        private static string GetSourceCodePath([CallerFilePath] string Path = null) => Path;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var h = Host;
            IsDesignMode = false;

            base.OnStartup(e);

            await h.StartAsync().ConfigureAwait(false);

        }

        protected override async void OnExit(ExitEventArgs e)
        {
            var h = Host;
            base.OnExit(e);

            await h.StopAsync().ConfigureAwait(false);
            h.Dispose();
            _host = null;
        }

        public static string CurrentDirectory => IsDesignMode
            ? Path.GetDirectoryName(GetSourceCodePath())
            : Environment.CurrentDirectory;

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection service)
        {
            service.RegisterViewModels();
            service.RegisterServices();
        }
    }
}

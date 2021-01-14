using ReactiveUI;
using Serilog;
using SpectraMixtureCombineTool.View;
using SpectraMixtureCombineTool.ViewModel;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SpectraMixtureCombineTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(@"Logs\AlchemyApplicationLogs.log")
                .CreateLogger();

            Locator.CurrentMutable.Register(() => new MainWindow(), typeof(IViewFor<MainWindowViewModel>));
            Locator.CurrentMutable.Register(() => new SpectraFileView(), typeof(IViewFor<SpectraFileViewModel>));

            var window = new MainWindow();
            window.Show();
        }
    }
}

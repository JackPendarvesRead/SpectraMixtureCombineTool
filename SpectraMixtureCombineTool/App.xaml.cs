using ReactiveUI;
using Serilog;
using SpectraMixtureCombineTool.WPF.View;
using SpectraMixtureCombineTool.WPF.ViewModel;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SpectraMixtureCombineTool.WPF
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
                .WriteTo.File(@"Logs\FormulaCheckApplicationLogs.log")
                .CreateLogger();

            Locator.CurrentMutable.Register(() => new MainWindow(), typeof(IViewFor<MainWindowViewModel>));
            Locator.CurrentMutable.Register(() => new SpectraFileView(), typeof(IViewFor<SpectraFileViewModel>));
            Locator.CurrentMutable.Register(() => new SettingsViewModel(), typeof(IViewFor<SettingsWindow>));
            Locator.CurrentMutable.Register(() => new HelpViewModel(), typeof(IViewFor<HelpWindow>));

            var settingsManager = new SettingsManager<UserSettings>("FormulaCheckUserSettings.json", "Alchemy");
            Locator.CurrentMutable.RegisterConstant<SettingsManager<UserSettings>>(settingsManager);

            var window = new MainWindow();
            window.Show();
        }
    }
}

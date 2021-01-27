using DynamicData;
using ReactiveUI;
using SpectraMixtureCombineTool.Logic.Infrastructure;
using SpectraMixtureCombineTool.Logic.Workflow;
using SpectraMixtureCombineTool.Service;
using SpectraMixtureCombineTool.WPF.View;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectraMixtureCombineTool.WPF.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        public ReactiveCommand<Unit, Unit> AddSpectraFileCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }
        public ReactiveCommand<Unit, Unit> ClearCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SettingsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> HelpCommand { get; set; }

        private readonly SpectraFileCache cache = new SpectraFileCache();
        private readonly ReadOnlyObservableCollection<SpectraFileViewModel> _files;
        public ReadOnlyObservableCollection<SpectraFileViewModel> Files => _files;

        public MainWindowViewModel()
        {
            cache.Connect()
                .Transform(x => new SpectraFileViewModel(x))
                .Bind(out _files)
                .Subscribe();

            AddSpectraFileCommand = ReactiveCommand.Create(AddSpectraFileImpl);
            SaveCommand = ReactiveCommand.Create(SaveImpl);
            ClearCommand = ReactiveCommand.Create(ClearImpl);
            SettingsCommand = ReactiveCommand.Create(SettingCmdImpl);
            HelpCommand = ReactiveCommand.Create(HelpCmdImpl);
        }

        private void HelpCmdImpl()
        {
            var window = new HelpWindow();
            window.ShowDialog();
        }

        private void SettingCmdImpl()
        {
            var window = new SettingsWindow();
            window.ShowDialog();
        }

        private void ClearImpl()
        {
            cache.Clear();
        }

        private void SaveImpl()
        {
            try
            {
                var settingsManager = Locator.Current.GetService<SettingsManager<UserSettings>>();
                var settings = settingsManager.LoadSettings();
                var fillerCount = Files.Where(x => x.FileType == SpectraFileType.Filler).Count();
                var ingredientCount = Files.Where(x => x.FileType == SpectraFileType.Ingredient).Count();
                if (settings.GenerateVariation && (fillerCount == 0 || ingredientCount == 0))
                {
                    var msg = MessageBox.Show(
                        "There was either no ingredient or no filler selected. Do you want to continue without adding any?", 
                        "Missing Ingredient Types", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Warning);
                    if(msg != DialogResult.Yes)
                    {
                        return;
                    }                
                }

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "JCAMP|*.jcm";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {                        
                        var workflow = new Workflow();
                        workflow.Execute(sfd.FileName, GetSpectraFiles(), settings.PercentageChange, settings.GenerateVariation ? settings.NumberOfIterations : 0);
                        MessageBox.Show("Save successful.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private IEnumerable<SpectraFile> GetSpectraFiles()
        {
            foreach(var file in Files)
            {
                yield return new SpectraFile
                {
                    FilePath = file.FilePath,
                    FileType = file.FileType,
                    Coefficient = float.Parse(file.Coefficient),
                    Ingredient = file.Ingredient
                };
            }
        }

        private void AddSpectraFileImpl()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Filter = "Foss Spectra File|*.nir";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach(var file in ofd.FileNames)
                    {
                        cache.AddFile(file);
                    }
                }
            }
        }
    }
}

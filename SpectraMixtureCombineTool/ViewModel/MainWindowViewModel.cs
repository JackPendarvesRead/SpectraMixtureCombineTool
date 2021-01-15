﻿using DynamicData;
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
                var fillerCount = Files.Where(x => x.FileType == SpectraFileType.Filler).Count();
                var ingredientCount = Files.Where(x => x.FileType == SpectraFileType.Ingredient).Count();
                if (fillerCount == 0 || ingredientCount == 0)
                {
                    var msg = MessageBox.Show(
                        "There was either no ingredient or no filler selected. Do you want to continue without adding any?", 
                        "Missing Ingredient Types", 
                        MessageBoxButtons.OKCancel, 
                        MessageBoxIcon.Warning);
                    if(msg != DialogResult.OK)
                    {
                        return;
                    }                
                }

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "JCAMP|*.jcm";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        var settingsManager = Locator.Current.GetService<SettingsManager<UserSettings>>();
                        var settings = settingsManager.LoadSettings();
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
                ofd.Filter = "Foss Spectra File|*.nir";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    cache.AddFile(ofd.FileName);
                }
            }
        }
    }
}

using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using System.Windows;

namespace SpectraMixtureCombineTool.WPF.ViewModel
{
    public class SettingsViewModel : ReactiveObject, IValidatableViewModel
    {
        public ReactiveCommand<Unit, bool> OkCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; private set; }

        private bool generateVariation;
        public bool GenerateVariation
        {
            get => generateVariation;
            set => this.RaiseAndSetIfChanged(ref generateVariation, value);
        }

        private int numberOfIterations;
        public int NumberOfIterations
        {
            get => numberOfIterations;
            set => this.RaiseAndSetIfChanged(ref numberOfIterations, value);
        }

        private float percentageChange;
        public float PercentageChange
        {
            get => percentageChange;
            set => this.RaiseAndSetIfChanged(ref percentageChange, value);
        }

        public ValidationContext ValidationContext { get; } = new ValidationContext(ImmediateScheduler.Instance);

        private readonly SettingsManager<UserSettings> settingsManager;
        private readonly UserSettings settings;

        public SettingsViewModel()
        {
            settingsManager = Locator.Current.GetService<SettingsManager<UserSettings>>();
            settings = settingsManager.LoadSettings();

            NumberOfIterations = settings.NumberOfIterations;
            PercentageChange = settings.PercentageChange;
            GenerateVariation = settings.GenerateVariation;

            //Commands
            OkCommand = ReactiveCommand.Create<Unit, bool>(_ =>
            {
                try
                {
                    settings.GenerateVariation = GenerateVariation;
                    settings.PercentageChange = PercentageChange;
                    settings.NumberOfIterations = NumberOfIterations;
                    settingsManager.SaveSettings(settings);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }

                return true;
            },
            this.WhenAnyValue(x => x.ValidationContext.IsValid));
            OkCommand.ThrownExceptions.Subscribe(ex =>
            {
                MessageBox.Show(ex.Message);
            });

            CancelCommand = ReactiveCommand.Create(() =>
            {
                //Do Nothing
            });
            CancelCommand.ThrownExceptions.Subscribe(ex =>
            {
                MessageBox.Show(ex.Message);
            });

            var validGeneration = this.WhenAnyValue(
                vm => vm.GenerateVariation,
                vm => vm.NumberOfIterations, 
                vm => vm.PercentageChange,
                (generate, iterations, percentageChange) =>  !generate || (iterations > 0 && percentageChange > 0));
            //this.ValidationRule(x => x.percentageChange, x => float.TryParse(x, out float f));
            this.ValidationRule(validGeneration, "Generation settings are invalid");
        }
    }
}

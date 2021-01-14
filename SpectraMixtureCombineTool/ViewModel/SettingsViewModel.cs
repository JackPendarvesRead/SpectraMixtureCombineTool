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

        private int variationNumber;
        public int VariationNumber
        {
            get => variationNumber;
            set => this.RaiseAndSetIfChanged(ref variationNumber, value);
        }

        public ValidationContext ValidationContext { get; } = new ValidationContext(ImmediateScheduler.Instance);

        private readonly SettingsManager<UserSettings> settingsManager;
        private readonly UserSettings settings;

        public SettingsViewModel()
        {
            settingsManager = Locator.Current.GetService<SettingsManager<UserSettings>>();
            settings = settingsManager.LoadSettings();

            VariationNumber = settings.VariationNumber;
            GenerateVariation = settings.GenerateVariation;

            //Commands
            OkCommand = ReactiveCommand.Create<Unit, bool>(_ =>
            {
                try
                {
                    settings.GenerateVariation = GenerateVariation;
                    settings.VariationNumber = VariationNumber;
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

            var validGeneration = this.WhenAnyValue(vm => vm.GenerateVariation, vm => vm.VariationNumber, (generate, number) =>  !generate || number > 0);
            this.ValidationRule(validGeneration, "Generation settings are invalid");
        }
    }
}

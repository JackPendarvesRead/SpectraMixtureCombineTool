using ReactiveUI;
using SpectraMixtureCombineTool.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpectraMixtureCombineTool.WPF.View
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : ReactiveWindow<SettingsViewModel>
    {
        private List<int> numbers = new List<int> { 1, 2, 3, 4, 5 }; 

        public SettingsWindow()
        {
            InitializeComponent();
            ViewModel = new SettingsViewModel();
            NumberGeneratedComboBox.ItemsSource = numbers;
            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.GenerateVariation, view => view.GenerateVariationCheckBox.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.VariationNumber, view => view.NumberGeneratedComboBox.SelectedItem).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.OkCommand, view => view.OkButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.CancelCommand, view => view.CancelButton).DisposeWith(disposables);

                if (this.NumberGeneratedComboBox.SelectedItem == null)
                    this.NumberGeneratedComboBox.SelectedIndex = numbers[0];

                ViewModel.OkCommand.Subscribe(success =>
                {
                    if (success)
                        Close();
                });
                ViewModel.CancelCommand.Subscribe(_ =>
                {
                    Close();
                });

                this.WhenAnyValue(x => x.GenerateVariationCheckBox.IsChecked)
                    .Subscribe(ticked =>
                    {
                        if ((bool)ticked)
                            this.GenerateVariationSettingsPanel.Visibility = Visibility.Visible;
                        else
                            this.GenerateVariationSettingsPanel.Visibility = Visibility.Collapsed;
                    }); 
            });
        }
    }
}

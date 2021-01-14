using ReactiveUI;
using SpectraMixtureCombineTool.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpectraMixtureCombineTool.WPF.View
{
    /// <summary>
    /// Interaction logic for SpectraFileView.xaml
    /// </summary>
    public partial class SpectraFileView : ReactiveUserControl<SpectraFileViewModel>
    {
        public SpectraFileView()
        {
            InitializeComponent();
            this.WhenActivated((disposables) =>
            {
                this.OneWayBind(ViewModel,
                    vm => vm.FilePath,
                    view => view.FilePath.Text
                    ).DisposeWith(disposables);

                this.Bind(ViewModel,
                    vm => vm.Ingredient,
                    view => view.Ingredient.Text
                    ).DisposeWith(disposables);

                this.Bind(ViewModel,
                    vm => vm.Coefficient,
                    view => view.Coefficient.Text
                    ).DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                   vm => vm.SpectraFileTypes,
                   view => view.SpectraFileType.ItemsSource
                   ).DisposeWith(disposables);

                this.Bind(ViewModel,
                   vm => vm.FileType,
                   view => view.SpectraFileType.SelectedItem
                   ).DisposeWith(disposables);

                SpectraFileType.SelectedItem = SpectraMixtureCombineTool.Logic.Infrastructure.SpectraFileType.Ingredient;

                this.Coefficient.PreviewKeyDown += Coefficient_PreviewKeyDown;
                this.Coefficient.PreviewTextInput += Coefficient_PreviewTextInput;
            });
        }

        private static readonly Regex allowedCharRegex = new Regex(@"[0-9.-]", RegexOptions.Compiled);
        private void Coefficient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = (TextBox)sender;
            if (!allowedCharRegex.IsMatch(e.Text))
            {
                e.Handled = true;
            }

            if (e.Text == "-" && (tb.CaretIndex > 0 || tb.Text.Contains("-")))
            {
                e.Handled = true;
            }

            if (e.Text == "." && tb.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void Coefficient_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}

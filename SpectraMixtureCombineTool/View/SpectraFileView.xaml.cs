using ReactiveUI;
using SpectraMixtureCombineTool.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpectraMixtureCombineTool.View
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
                    vm => vm.Constituent,
                    view => view.Constituent.Text
                    ).DisposeWith(disposables);

                this.Bind(ViewModel,
                    vm => vm.Coefficient,
                    view => view.Coefficient.Text
                    ).DisposeWith(disposables);
            });
        }
    }
}

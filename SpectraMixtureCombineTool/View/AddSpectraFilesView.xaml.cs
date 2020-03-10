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
    /// Interaction logic for AddSpectraFilesView.xaml
    /// </summary>
    public partial class AddSpectraFilesView : ReactiveUserControl<AddSpectraFilesViewModel>
    {
        public AddSpectraFilesView()
        {
            InitializeComponent();
            this.WhenActivated((disposables) =>
            {
                this.OneWayBind(ViewModel,
                    vm => vm.Files,
                    view => view.SpectraFilesListBox.ItemsSource
                    ).DisposeWith(disposables);
            });
        }
    }    
}

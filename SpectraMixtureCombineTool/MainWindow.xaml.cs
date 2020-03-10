using ReactiveUI;
using SpectraMixtureCombineTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpectraMixtureCombineTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            this.WhenActivated((disposables) =>
            {
                this.OneWayBind(ViewModel,
                   vm => vm.Files,
                   view => view.SpectraFilesListBox.ItemsSource
                   ).DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.AddSpectraFileCommand,
                    view => view.AddButton
                    ).DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.SaveCommand,
                    view => view.SaveButton
                    ).DisposeWith(disposables);
            });
        }
    }
}

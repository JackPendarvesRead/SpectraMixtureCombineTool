using ReactiveUI;
using ReactiveUI.Validation.Extensions;
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
    public partial class HelpWindow : ReactiveWindow<HelpViewModel>
    {
        public HelpWindow()
        {
            InitializeComponent();
            ViewModel = new HelpViewModel();
            //NumberGeneratedComboBox.ItemsSource = numbers;
            //this.WhenActivated(disposables =>
            //{
            //}; 
        }  
    }
}

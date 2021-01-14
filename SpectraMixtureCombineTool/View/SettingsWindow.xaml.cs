﻿using ReactiveUI;
using SpectraMixtureCombineTool.WPF.ViewModel;
using System;
using System.Collections.Generic;
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
        public SettingsWindow()
        {
            InitializeComponent();
            ViewModel = new SettingsViewModel();
            this.WhenActivated(disposables =>
            {
            });
        }
    }
}

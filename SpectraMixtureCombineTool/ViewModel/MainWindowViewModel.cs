using DynamicData;
using ReactiveUI;
using SpectraMixtureCombineTool.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectraMixtureCombineTool.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        public ReactiveCommand<Unit, Unit> AddSpectraFileCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }

        private readonly SpectraFileCache cache = new SpectraFileCache();
        private readonly ReadOnlyObservableCollection<SpectraFileViewModel> _files;
        public ReadOnlyObservableCollection<SpectraFileViewModel> Files => _files;

        public MainWindowViewModel()
        {
            AddSpectraFileCommand = ReactiveCommand.Create(AddSpectraFileImpl);
            SaveCommand = ReactiveCommand.Create(SaveImpl);

            cache.Connect()
                .Transform(x => new SpectraFileViewModel(x))
                .Bind(out _files)
                .Subscribe();
        }

        private void SaveImpl()
        {
            using(var sfd = new SaveFileDialog())
            {
                if (ValidateCoefficients())
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var f in Files)
                        {
                            MessageBox.Show($"Name={f.Name}, Co={f.Coefficient}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sum of coefficients must be equal to 1.0");
                }
               
            }
        }

        private void AddSpectraFileImpl()
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    cache.AddFile(ofd.FileName);
                }
            }
        }

        private bool ValidateCoefficients()
        {
            float sum = 0.0f;
            foreach(var f in Files)
            {
                sum += float.Parse(f.Coefficient);
            }
            if(sum == 1.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

using DynamicData;
using ReactiveUI;
using SpectraMixtureCombineTool.Model;
using SpectraMixtureCombineTool.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "JCAMP|*.jcm";
                    if (ValidateCoefficients())
                    {
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            var files = Files.ToArray();
                            var converter = new SpectrumConverter();
                            var weighted = converter.GetWeightedSpectra(files);
                            var writer = new FileWriter();
                            writer.WriteFile(sfd.FileName, weighted);
                            writer.WriteTxtFile(sfd.FileName, files);
                            MessageBox.Show("Save successful.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sum of coefficients must be equal to 1.0");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddSpectraFileImpl()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Foss Spectra File|*.nir";
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

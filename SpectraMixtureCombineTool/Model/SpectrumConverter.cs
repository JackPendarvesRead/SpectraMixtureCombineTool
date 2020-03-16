using Aunir.SpectrumAnalysis2.FossConnector;
using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.JcampConnector;
using SpectraMixtureCombineTool.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class SpectrumConverter
    {
        public ISpectrumData GetWeightedSpectra(IEnumerable<SpectraFileViewModel> files)
        {
            var spectra = ReadFiles(files);
            return AggregateSpectra(spectra);
        }

        private IEnumerable<ISpectrumData> ReadFiles(IEnumerable<SpectraFileViewModel> files)
        {
            var reader = new FossSpectraReader();
            foreach (var file in files)
            {
                using (var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Delete))
                {
                    var spectrum = reader.ReadStream(stream).First();
                    for (var i = 0; i < spectrum.Data.Count; i++)
                    {
                        spectrum.Data[i] *= float.Parse(file.Coefficient);
                    }
                    spectrum.SpectrumInformation.Add(file.Name, file.Coefficient);
                    yield return spectrum;
                }
            }
        }

        private ISpectrumData AggregateSpectra(IEnumerable<ISpectrumData> spectra)
        {
            var data = spectra.Select(x => x.Data)
                .Aggregate((acc, next) => acc.Zip(next, (a, b) => a + b).ToList());

            var wavelengths = spectra.Select(x => x.Wavelengths)
                .First();

            var dic = spectra.Select(x => x.SpectrumInformation)
                .Merge();

            return new SpectrumData
            {
                Data = data,
                Wavelengths = wavelengths,
                SpectrumInformation = dic
            };
        }
    }
}

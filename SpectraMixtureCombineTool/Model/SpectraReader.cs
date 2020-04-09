using Aunir.SpectrumAnalysis2.FossConnector;
using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.Interfaces.Constants;
using SpectraMixtureCombineTool.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class SpectraReader
    {
        public IEnumerable<SpectrumData> Read(IEnumerable<SpectraFileViewModel> files, string sampleReference)
        {
            var aaaa = ReadFiles(files, sampleReference);
            ValidateSpectra(aaaa);
            return aaaa;
        }

        private IEnumerable<SpectrumData> ReadFiles(IEnumerable<SpectraFileViewModel> files, string sampleReference)
        {
            var reader = new FossSpectraReader();
            foreach (var file in files)
            {
                using (var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Delete))
                {
                    ISpectrumData spectrum = reader.ReadStream(stream).First();
                    spectrum.SpectrumInformation.Add(file.Ingredient, file.Coefficient);
                    spectrum.SpectrumInformation[InformationConstants.SampleReference] = sampleReference;
                    var data = new List<float>();
                    for (var i = 0; i < spectrum.Data.Count; i++)
                    {
                        data.Add(spectrum.Data[i] * float.Parse(file.Coefficient));
                    }
                    var weightedSpectrum = new SpectrumData
                    {
                        Data = data,
                        SpectrumInformation = spectrum.SpectrumInformation,
                        Wavelengths = spectrum.Wavelengths
                    };
                    weightedSpectrum.coefficient = float.Parse(file.Coefficient);
                    weightedSpectrum.Name = file.Ingredient;
                    weightedSpectrum.FileType = file.FileType;
                    yield return weightedSpectrum;
                }
            }
        }

        private void ValidateSpectra(IEnumerable<ISpectrumData> data)
        {
            var wavelengths = data.Select(x => x.Wavelengths);

            wavelengths.Aggregate((acc, next) =>
            {
                if (acc.Count != next.Count)
                    throw new Exception("Wavelength counts do not match in files");

                for (var i = 0; i < acc.Count; i++)
                {
                    if (acc[i] != next[i])
                        throw new Exception("Wavelengths do not match. Check resolution or start/end wavelengths in files.");
                }
                return acc;
            });
        }
    }
}

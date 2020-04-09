using Aunir.SpectrumAnalysis2.FossConnector;
using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.Interfaces.Constants;
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
        public IEnumerable<ISpectrumData> GetWeightedSpectra(IEnumerable<SpectraFileViewModel> files, string sampleReference)
        {
            var spectra = ReadFiles(files, sampleReference);
            ValidateSpectra(spectra);

            var constantCount = files.Where(x => x.FileType == SpectraFileType.Constant).Count();
            var fillerCount = files.Where(x => x.FileType == SpectraFileType.Filler).Count();
            var ingredientCount = files.Where(x => x.FileType == SpectraFileType.Ingredient).Count();

            int percentageChange = 10;
            for (var i = -percentageChange; i <= percentageChange; i++)
            {
                float coefficient = (float)i / 100f + 1f;
                yield return AggregateSpectra(spectra, coefficient);
            }            
        }

        private IEnumerable<SpectrumData> ReadFiles(IEnumerable<SpectraFileViewModel> files, string sampleReference)
        {
            var reader = new FossSpectraReader();
            foreach (var file in files)
            {
                using (var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Delete))
                {
                    SpectrumData spectrum = (SpectrumData)reader.ReadStream(stream).First();
                    spectrum.SpectrumInformation.Add(file.Constituent, file.Coefficient);
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
                    weightedSpectrum.Name = file.Constituent;
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

                for(var i = 0; i < acc.Count; i++)
                {
                    if (acc[i] != next[i])
                        throw new Exception("Wavelengths do not match. Check resolution or start/end wavelengths in files.");
                }
                return acc;
            });
        }

        private ISpectrumData AggregateSpectra(IEnumerable<ISpectrumData> spectra, float coefficient)
        {
            var data = GetWeightedData(spectra, coefficient).ToList();

            var wavelengths = spectra.Select(x => x.Wavelengths)
                .First();

            var dic = spectra.Select(x => x.SpectrumInformation)
                .Merge();
            dic[InformationConstants.SampleReference] = dic[InformationConstants.SampleReference] + coefficient.ToString("0.00");

            return new SpectrumData
            {
                Data = data,
                Wavelengths = wavelengths,
                SpectrumInformation = dic
            };
        }

        private IEnumerable<float> GetWeightedData(IEnumerable<ISpectrumData> spectra, float coefficient)
        {
            var aggregated = spectra.Select(x => x.Data)
                .Aggregate((acc, next) => acc.Zip(next, (a, b) => a + b).ToList());

            foreach(var a in aggregated)
            {
                yield return a * coefficient;
            }
        }
    }
}

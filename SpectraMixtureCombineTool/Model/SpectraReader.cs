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
        public List<SpectrumData> Read(IEnumerable<SpectraFileViewModel> files, string sampleReference)
        {
            var aaaa = ReadFiles(files, sampleReference);
            ValidateSpectra(aaaa);
            return aaaa.ToList();
        }

        private IEnumerable<SpectrumData> ReadFiles(IEnumerable<SpectraFileViewModel> files, string sampleReference)
        {
            var reader = new FossSpectraReader();
            foreach (var file in files)
            {
                using (var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Delete))
                {                    
                    ISpectrumData spectrum = reader.ReadStream(stream).First();
                    spectrum.SpectrumInformation.Add("Ingredient:" + file.Ingredient, file.Coefficient);
                    spectrum.SpectrumInformation[InformationConstants.SampleReference] = sampleReference;
                    var spectrumData = new SpectrumData
                    {
                        Data = spectrum.Data,
                        SpectrumInformation = spectrum.SpectrumInformation,
                        Wavelengths = spectrum.Wavelengths,
                        RatioValue = float.Parse(file.Coefficient),
                        Name = file.Ingredient,
                        FileType = file.FileType
                    };
                    yield return spectrumData;
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

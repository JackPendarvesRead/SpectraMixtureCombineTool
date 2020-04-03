//using Aunir.SpectrumAnalysis2.FossConnector;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace SpectraMixtureCombineTool.Model
//{
//    public class SpectraReader
//    {
//        public IEnumerable<Ingredient> ImportIngredients()
//        {
//            var spectra = ReadFiles(files, sampleReference);
//            ValidateSpectra(spectra);

//            var reader = new FossSpectraReader();
//            foreach (var file in files)
//            {
//                using (var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Delete))
//                {
//                    SpectrumData spectrum = (SpectrumData)reader.ReadStream(stream).First();
//                    spectrum.SpectrumInformation.Add(file.Name, file.Coefficient);
//                    spectrum.SpectrumInformation[InformationConstants.SampleReference] = sampleReference;
//                    var data = new List<float>();
//                    for (var i = 0; i < spectrum.Data.Count; i++)
//                    {
//                        data.Add(spectrum.Data[i] * float.Parse(file.Coefficient));
//                    }
//                    var weightedSpectrum = new SpectrumData
//                    {
//                        Data = data,
//                        SpectrumInformation = spectrum.SpectrumInformation,
//                        Wavelengths = spectrum.Wavelengths
//                    };
//                    weightedSpectrum.coefficient = float.Parse(file.Coefficient);
//                    weightedSpectrum.Name = file.Name;
//                    weightedSpectrum.FileType = file.FileType;
//                    yield return weightedSpectrum;
//                }
//            }
//        }
//    }
//}

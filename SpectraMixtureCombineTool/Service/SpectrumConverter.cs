using Aunir.SpectrumAnalysis2.FossConnector;
using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.JcampConnector;
using SpectraMixtureCombineTool.Model;
using SpectraMixtureCombineTool.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Service
{
    public class SpectrumConverter
    {
        //public WeightedSpectrum CombineWeightedSpectra(IEnumerable<ISpectrumData> spectra)
        //{
        //    var spectraList = spectra.ToList();
        //    var output = spectraList[1];



        //}

        public IEnumerable<ISpectrumData> GetWeightedSpectra(List<SpectraFileViewModel> files)
        {
            var reader = new FossSpectraReader();
            foreach(var file in files)
            {
                using(var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Delete))
                {
                    var spectrum = reader.ReadStream(stream).First();                    
                    for(var i = 0; i < spectrum.Data.Count; i++)
                    {
                        spectrum.Data[i] *= float.Parse(file.Coefficient);
                    }
                    spectrum.SpectrumInformation.Add(file.Name, file.Coefficient);
                    yield return spectrum;
                }                
            }
        }
    }
}

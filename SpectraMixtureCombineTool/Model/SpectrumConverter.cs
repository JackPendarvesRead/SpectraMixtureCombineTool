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
        public IEnumerable<ISpectrumData> GetWeightedSpectra(IEnumerable<SpectrumData> spectra, int percentageChange = 10)
        {
            for (var i = -percentageChange; i <= percentageChange; i++)
            {
                yield return AggregateSpectra(spectra, percentageChange);
            }            
        }        

        private ISpectrumData AggregateSpectra(IEnumerable<SpectrumData> spectra, int percentageChange)
        {
            var data = GetWeightedData(spectra, percentageChange).ToList();

            var wavelengths = spectra.Select(x => x.Wavelengths).First();

            var dic = spectra.Select(x => x.SpectrumInformation).Merge();
            dic[InformationConstants.SampleReference] = dic[InformationConstants.SampleReference] + percentageChange.ToString();

            return new SpectrumData
            {
                Data = data,
                Wavelengths = wavelengths,
                SpectrumInformation = dic
            };
        }

        private IEnumerable<float> GetWeightedData(IEnumerable<SpectrumData> spectra, int percentageChange)
        {
            var aggregated = spectra.Aggregate(new SpectrumData(), (acc, next) => 
            {
                float coefficient;
                switch (next.FileType)
                {
                    case SpectraFileType.Constant:
                        coefficient = 1;
                        break;

                    case SpectraFileType.Filler:
                        coefficient = 1 - percentageChange / spectra.Where(x => x.FileType == SpectraFileType.Filler).Count();
                        break;

                    case SpectraFileType.Ingredient:   
                        coefficient = 1 + percentageChange / spectra.Where(x => x.FileType == SpectraFileType.Ingredient).Count();
                        break;

                    default:
                        throw new Exception("Could not recognise SpectraFileType in GetWeightedData method");
                }

                acc.Data = acc.Data != null ? acc.Data.Zip(next.Data, (acc, next) => acc + next * coefficient).ToList() : next.Data;
                return acc;
            });
            return aggregated.Data;
        }
    }
}

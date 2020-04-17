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
        public IEnumerable<WeightedSpectrum> GetWeightedSpectra(Mixture mixture, int percentageChange = 10)
        {
            for (var i = -percentageChange; i <= percentageChange; i++)
            {
                float percentageCoefficient = (float)i / 100f;
                yield return AggregateSpectra(mixture, percentageCoefficient);
            }            
        }

        private WeightedSpectrum AggregateSpectra(Mixture mixture, float percentageCoefficient)
        {      
            var dic = mixture.Spectra.Select(x => x.SpectrumInformation).Merge();

            float coefficientSum = 0f;
            float[] weightedSpectra = new float[mixture.Spectra.First().Data.Count];

            for (var j = 0; j < weightedSpectra.Length; j++)
            {
                for (var i = 0; i < mixture.Spectra.Count; i++)
                {
                    float abs = mixture.Spectra[i].Data[j];
                    float weightedCoefficient = GetWeightedCoefficient(mixture.Spectra[i], mixture.FillerCount, mixture.IngredientCount, percentageCoefficient);
                    coefficientSum += weightedCoefficient;
                    float value = abs * weightedCoefficient;
                    weightedSpectra[j] += value;

                    var ingredients = mixture.Spectra[i].SpectrumInformation.Where(x => x.Key.StartsWith(JcampInformationConstants.Ingredient));
                    foreach(var ingredient in ingredients)
                    {
                        dic[ingredient.Key] = weightedCoefficient.ToString();
                    }
                }
                weightedSpectra[j] /= coefficientSum;
                coefficientSum = 0f;
            }

            var wavelengths = mixture.Spectra.Select(x => x.Wavelengths).First();
            dic[InformationConstants.SampleReference] = percentageCoefficient.ToString() + dic[InformationConstants.SampleReference];

            return new WeightedSpectrum
            {
                Data = weightedSpectra,
                Wavelengths = wavelengths,
                SpectrumInformation = dic,
                PercentChange = percentageCoefficient
            };
        }

        //private IEnumerable<float> GetWeightedData(Mixture mixture, float percentageCoefficient)
        //{
        //    float coefficientSum = 0f;
        //    float[] weightedSpectra = new float[mixture.Spectra.First().Data.Count];

        //    for(var j = 0; j < weightedSpectra.Length; j++)
        //    {
        //        for (var i = 0; i < mixture.Spectra.Count; i++)
        //        {
        //            float abs = mixture.Spectra[i].Data[j];
        //            float weightedCoefficient = GetWeightedCoefficient(mixture.Spectra[i], mixture.FillerCount, mixture.IngredientCount, percentageCoefficient);
        //            coefficientSum += weightedCoefficient;
        //            float value = abs * weightedCoefficient;
        //            weightedSpectra[j] += value;
        //        }
        //        weightedSpectra[j] /= coefficientSum;
        //        coefficientSum = 0f;
        //    }
        //    return weightedSpectra;
        //}

        private float GetWeightedCoefficient(SpectrumData spectrumData, int fillerCount, int ingredientCount, float percentageCoefficient)
        {
            float adjustedPercentageCoefficient = spectrumData.FileType switch
            {
                SpectraFileType.Constant => 1,
                SpectraFileType.Filler => 1 - percentageCoefficient / fillerCount,
                SpectraFileType.Ingredient => 1 + percentageCoefficient / ingredientCount,
                _ => throw new Exception("Could not recognise SpectraFileType in GetWeightedData method")
            };
            return spectrumData.RatioValue * adjustedPercentageCoefficient;
        }
    }
}

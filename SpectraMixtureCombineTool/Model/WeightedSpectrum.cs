using Aunir.SpectrumAnalysis2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class WeightedSpectrum : ISpectrumData
    {
        public IList<float> Data { get; set; }
        public IList<float> Wavelengths { get; set; }
        public IDictionary<string, string> SpectrumInformation { get; set; }
        public float PercentChange { get; set; }
    }
}

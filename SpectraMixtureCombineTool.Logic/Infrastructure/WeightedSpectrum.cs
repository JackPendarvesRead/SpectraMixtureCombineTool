using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.Interfaces.Constants;
using Aunir.SpectrumAnalysis2.Interfaces.Pretreatments;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Infrastructure
{
    public class WeightedSpectrum : ISpectrumData
    {
        public IList<float> Data { get; set; }
        public IList<float> Wavelengths { get; set; }
        public IDictionary<string, string> SpectrumInformation { get; set; }
        public float PercentChange { get; set; }

        public string SpectrumReference => throw new NotImplementedException();

        public int NumberOfDatapoints => throw new NotImplementedException();

        public string InstrumentType => throw new NotImplementedException();

        public string InstrumentId => throw new NotImplementedException();

        public XUnits XUnits => throw new NotImplementedException();

        public YUnits YUnits => throw new NotImplementedException();

        public void ApplyPretreatment(IPretreatment pretreatment)
        {
            throw new NotImplementedException();
        }
    }
}

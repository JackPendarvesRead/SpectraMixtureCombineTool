using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.Interfaces.Constants;
using Aunir.SpectrumAnalysis2.Interfaces.Pretreatments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Infrastructure
{
    public class SpectrumData : Aunir.SpectrumAnalysis2.Core.Default.SpectrumData
    {

        public SpectrumData(IList<float> wavelengths, IList<float> data, IDictionary<string, string> spectrumInformation)
            : base(wavelengths,
                  data, 
                  spectrumInformation, 
                  spectrumInformation[InformationConstants.SpectrumReference], 
                  spectrumInformation[InformationConstants.InstrumentType], 
                  spectrumInformation[InformationConstants.InstrumentId],                       
                  Enum.Parse<XUnits>(spectrumInformation[InformationConstants.XUnits]),
                  Enum.Parse<YUnits>(spectrumInformation[InformationConstants.YUnits]))
        {
        }

        public SpectrumData(IList<float> wavelengths, IList<float> data, IDictionary<string,string> spectrumInformation, string spectrumReference, string instrumentType, string instrumentId, XUnits xUnits, YUnits yUnits) 
            : base(wavelengths, data, spectrumInformation, spectrumReference, instrumentType, instrumentId, xUnits, yUnits)
        {
        }

        public string Name { get; set; }
        public float Inclusion { get; set; }
        public SpectraFileType FileType { get; set; }
    }    
}

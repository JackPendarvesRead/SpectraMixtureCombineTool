using Aunir.SpectrumAnalysis2.Core.Connectors;
using Aunir.SpectrumAnalysis2.Core.Default;
using Aunir.SpectrumAnalysis2.FossConnector;
using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.Pretreatments;
using Aunir.SpectrumAnalysis2.Pretreatments.PretreatmentGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectraMixtureCombineTool.Service
{
    public class SpectrumFileReader
    {
        public void Read(Stream stream)
        {
            var x = new FossSpectraReader();
            var spectra = x.ReadStream(stream);
            foreach(var spectrum in spectra)
            {
                foreach(var dataPoint in spectrum.Data)
                {
                }
            }
        }        
    }
}
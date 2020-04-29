using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.JcampConnector;
using SpectraMixtureCombineTool.Logic.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Writer
{
    public class JcampFileWriter
    {
        public void WriteJcampFile(string filePath, IEnumerable<WeightedSpectrum> data)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Delete);
            var writer = new JcampSpectraWriter();
            writer.WriteSpectraToStream(data, stream);
        }
    }
}

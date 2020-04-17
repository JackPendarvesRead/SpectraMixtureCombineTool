using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.JcampConnector;
using SpectraMixtureCombineTool.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class FileWriter
    {
        public void WriteJcampFile(string filePath, IEnumerable<WeightedSpectrum> data)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Delete);
            var writer = new JcampSpectraWriter();
            writer.WriteSpectraToStream(data, stream);
        }
    }
}

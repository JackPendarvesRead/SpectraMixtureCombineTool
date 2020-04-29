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
    internal sealed class JcampFileWriter : IWriter<SpectrumData>
    {
        public void Write(string filePath, IEnumerable<SpectrumData> data)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Delete);
            var writer = new JcampSpectraWriter();
            writer.WriteSpectraToStream(data, stream);
        }
    }
}

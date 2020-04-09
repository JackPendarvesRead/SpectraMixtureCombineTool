using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.JcampConnector;
using SpectraMixtureCombineTool.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class FileWriter
    {
        public void WriteFile(string filePath, IEnumerable<ISpectrumData> data)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Delete);
            var writer = new JcampSpectraWriter();
            writer.WriteSpectraToStream(data, stream);
        }

        public void WriteTxtFile(string filePath, SpectraFileViewModel[] data)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Delete);
            var coefficientString = new StringBuilder();
            var constituentString = new StringBuilder();
            for(var i=0; i < data.Length; i++)
            {
                if (i == 0)
                {
                    coefficientString.Append(data[i].Coefficient);
                    constituentString.Append(data[i].Constituent);
                }
                else
                {
                    coefficientString.Append("," + data[i].Coefficient);
                    constituentString.Append("," + data[i].Constituent);
                }
            }
            var exportArray = new string[2] { constituentString.ToString(), coefficientString.ToString() };            
            File.WriteAllLines(Path.ChangeExtension(filePath, "txt"), exportArray);
        }
    }
}

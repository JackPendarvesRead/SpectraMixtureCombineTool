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

        public void WriteTextFile(string filePath, List<WeightedSpectrum> data, SpectraFileViewModel[] files)
        {
            var list = new List<string>();
            list.Add(GetConstituentString(files));
            foreach(var d in data)
            {
                list.Add(GetCoefficientString(files, d));
            }         
            File.WriteAllLines(Path.ChangeExtension(filePath, "txt"), list);
        }

        private string GetConstituentString(SpectraFileViewModel[] files)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < files.Length; i++)
            {
                if(i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(files[i].Ingredient);
               
            }
            return sb.ToString();
        }

        private string GetCoefficientString(SpectraFileViewModel[] files, WeightedSpectrum spectrum)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < files.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }                
                sb.Append(files[i].Coefficient + "*" + spectrum.PercentChange);
            }
            return sb.ToString();
        }
    }
}

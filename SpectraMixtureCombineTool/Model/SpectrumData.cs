using Aunir.SpectrumAnalysis2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class SpectrumData : ISpectrumData
    {
        public IList<float> Data { get; set; }
        public IList<float> Wavelengths { get; set; }
        public IDictionary<string, string> SpectrumInformation { get; set; }
        public string Name { get; set; }
        public float RatioValue { get; set; }
        public SpectraFileType FileType { get; set; }
    }

    public static class SpectrumDataExtension
    {
        public static IDictionary<string, string> Merge(this IEnumerable<IDictionary<string, string>> dics)
        {
            var map = new Dictionary<string, string>();
            foreach(var dic in dics)
            {
                foreach(var pair in dic)
                {
                    map[pair.Key] = pair.Value;
                }
            }
            return map;
        }        
    }
}

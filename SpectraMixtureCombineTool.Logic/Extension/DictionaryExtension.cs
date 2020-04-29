using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Extension
{
    public static class DictionaryExtension
    {
        public static IDictionary<string, string> Merge(this IEnumerable<IDictionary<string, string>> dics)
        {
            var map = new Dictionary<string, string>();
            foreach (var dic in dics)
            {
                foreach (var pair in dic)
                {
                    map[pair.Key] = pair.Value;
                }
            }
            return map;
        }
    }
}

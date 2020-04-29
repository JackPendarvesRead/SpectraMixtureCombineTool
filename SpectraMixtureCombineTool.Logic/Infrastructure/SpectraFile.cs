using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Infrastructure
{
    public class SpectraFile
    {
        public string FilePath { get; set; }
        public string Ingredient { get; set; }
        public float Coefficient { get; set; }
        public SpectraFileType FileType { get; set; }
    }
}

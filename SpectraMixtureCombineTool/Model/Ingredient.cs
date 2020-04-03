using Aunir.SpectrumAnalysis2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class Ingredient
    {
        public ISpectrumData Spectra { get; set; }
        public string Name { get; set; }
        public float Coefficient { get; set; }
    }
}

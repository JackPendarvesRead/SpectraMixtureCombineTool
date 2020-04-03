using Aunir.SpectrumAnalysis2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class Formulation
    {
        public List<ISpectrumData> ConstantIngredients { get; set; }
        public List<ISpectrumData> FillerIngredients { get; set; }
        public List<ISpectrumData> Ingredients { get; set; }
    }
}

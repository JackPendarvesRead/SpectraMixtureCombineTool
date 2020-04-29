using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Infrastructure
{
    public class Mixture
    {

        public Mixture(List<SpectrumData> spectra)
        {
            Spectra = spectra;
        }

        public List<SpectrumData> Spectra { get; }


        //public IEnumerable<SpectrumData> IngredientData => Spectra.Where(x => x.FileType == SpectraFileType.Ingredient);
        //public IEnumerable<SpectrumData> FillerData => Spectra.Where(x => x.FileType == SpectraFileType.Filler);
        //public IEnumerable<SpectrumData> ConstantData => Spectra.Where(x => x.FileType == SpectraFileType.Constant);

        //public float CoefficientSum => Spectra.Select(x => x.RatioValue).Sum();

        public int IngredientCount => Spectra.Where(x => x.FileType == SpectraFileType.Ingredient).Count();
        public int ConstantCount => Spectra.Where(x => x.FileType == SpectraFileType.Constant).Count();
        public int FillerCount => Spectra.Where(x => x.FileType == SpectraFileType.Filler).Count();
    }
}

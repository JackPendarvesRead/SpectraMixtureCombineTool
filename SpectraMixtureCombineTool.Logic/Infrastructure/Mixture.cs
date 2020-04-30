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

        public int IngredientCount => Spectra.Where(x => x.FileType == SpectraFileType.Ingredient).Count();
        public int ConstantCount => Spectra.Where(x => x.FileType == SpectraFileType.Constant).Count();
        public int FillerCount => Spectra.Where(x => x.FileType == SpectraFileType.Filler).Count();
    }
}

using Aunir.SpectrumAnalysis2.Interfaces;
using SpectraMixtureCombineTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class SpectrumCalculator
    {
        private void Test(IEnumerable<SpectraFileViewModel> files, float coefficient)
        {
            var constantCount = files.Where(x => x.FileType == SpectraFileType.Constant).Count();
            var fillerCount = files.Where(x => x.FileType == SpectraFileType.Filler).Count();
            var ingredientCount = files.Where(x => x.FileType == SpectraFileType.Ingredient).Count();

            foreach(var file in files)
            {

            }
        }
    }
}

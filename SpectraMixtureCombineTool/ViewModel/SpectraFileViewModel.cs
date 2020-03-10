using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraMixtureCombineTool.ViewModel
{
    public class SpectraFileViewModel : ReactiveObject
    {
        public string FilePath { get; set; }
        public string Name { get; set; }
        public string Coefficient { get; set; }

        public SpectraFileViewModel(string filePath)
        {
            FilePath = filePath;
        }
    }
}

using ReactiveUI;
using SpectraMixtureCombineTool.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectraMixtureCombineTool.ViewModel
{
    public class SpectraFileViewModel : ReactiveObject
    {
        public string FilePath { get; set; }
        public string Constituent { get; set; }
        public string Coefficient { get; set; }
        public SpectraFileType FileType { get; set; }

        public SpectraFileViewModel(string filePath)
        {
            FilePath = filePath;
            Constituent = Path.GetFileNameWithoutExtension(filePath);
        }
    }
}

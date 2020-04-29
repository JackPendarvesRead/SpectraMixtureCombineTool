using ReactiveUI;
using SpectraMixtureCombineTool.Logic.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.ViewModel
{
    public class SpectraFileViewModel : ReactiveObject
    {
        public string FilePath { get; set; }
        public string Ingredient { get; set; }
        public string Coefficient { get; set; }
        public SpectraFileType FileType { get; set; }
        public Array SpectraFileTypes => Enum.GetValues(typeof(SpectraFileType));
        
        public SpectraFileViewModel(string filePath)
        {
            FilePath = filePath;
            Ingredient = Path.GetFileNameWithoutExtension(filePath);
        }
    }
}

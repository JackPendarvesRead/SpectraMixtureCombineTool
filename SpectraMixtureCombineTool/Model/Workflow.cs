using SpectraMixtureCombineTool.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class Workflow
    {
        public void Execute(string filePath, SpectraFileViewModel[] files)
        {
            var reader = new SpectraReader();
            var sampleRef = Path.GetFileNameWithoutExtension(filePath);
            var spectra = reader.Read(files, sampleRef);

            var mixture = new Mixture(spectra);

            var converter = new SpectrumConverter();
            var weighted = converter.GetWeightedSpectra(mixture);

            var writer = new FileWriter();
            writer.WriteTextFile(filePath, weighted.ToList(), files);
            writer.WriteJcampFile(filePath, weighted);
        }
    }
}

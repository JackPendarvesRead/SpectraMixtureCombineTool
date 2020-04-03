using SpectraMixtureCombineTool.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectraMixtureCombineTool.Model
{
    public class Workflow
    {
        public void Execute(string filePath, SpectraFileViewModel[] files)
        {
            var converter = new SpectrumConverter();
            var sampleRef = Path.GetFileNameWithoutExtension(filePath);
            var weighted = converter.GetWeightedSpectra(files, sampleRef);

            //var sim = new SpectrumVariationSimulation();
            //var varied = sim.Split(weighted);

            var writer = new FileWriter();
            writer.WriteTxtFile(filePath, files);
            writer.WriteFile(filePath, weighted);
        }
    }
}

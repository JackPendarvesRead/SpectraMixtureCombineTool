﻿using SpectraMixtureCombineTool.Logic.Converter;
using SpectraMixtureCombineTool.Logic.Infrastructure;
using SpectraMixtureCombineTool.Logic.Reader;
using SpectraMixtureCombineTool.Logic.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Workflow
{
    public sealed class Workflow
    {
        public void Execute(string filePath, IEnumerable<SpectraFile> files, float percentageChange, int numberOfIterations)
        {
            var reader = new SpectraReader();
            var sampleRef = Path.GetFileNameWithoutExtension(filePath);
            var spectra = reader.Read(files, sampleRef);

            var mixture = new Mixture(spectra);

            var converter = new SpectrumConverter();
            var weighted = converter.GetWeightedSpectra(mixture, percentageChange, numberOfIterations);

            var writer = new JcampFileWriter();
            writer.Write(filePath, weighted);
        }
    }
}

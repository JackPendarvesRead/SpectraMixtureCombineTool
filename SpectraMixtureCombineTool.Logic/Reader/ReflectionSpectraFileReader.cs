using Aunir.SpectrumAnalysis2.Core.Connectors;
using Aunir.SpectrumAnalysis2.Core.Default;
using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.Pretreatments;
using Aunir.SpectrumAnalysis2.Pretreatments.PretreatmentGeneration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Reader
{
    public static class RefelectionSpectrumFileReader
    {
        public static IEnumerable<ISpectrumData> Import(Stream file)
        {
            var controller = new ReflectionConnectorController();
            controller.Initialise();
            var fileType = controller.IdentifyFile(file);
            if (fileType == null)
            {
                throw new Exception("File type not recognised by Spectrum Reader");
            }
            Log.Debug("Read filetype: {SpectraFileType}", fileType.TypeName);
            var reader = controller.GetReaderForFileType(fileType).GenerateReader(new DelegateInformationProvider(_ => new Dictionary<string, string>()));
            var spectra = reader.ReadStream(file);
            var spectraPretreamentFactory = controller.GetPretreatmentsForFileType(fileType);
            //var factory = PretreatmentFactory.CreateFromGeneratorsInAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            var pretreater = new SpectrumPretreater(new PretreatmentFactory());
            foreach (var spectrum in spectra)
            {
                var pretreatments = spectraPretreamentFactory.GenerateStandardPretreatments(spectrum);
                var harmonisedSpectrum = pretreater.ApplyPretreatments(spectrum, pretreatments);
                yield return harmonisedSpectrum;
            }
        }
    }
}

using Aunir.SpectrumAnalysis2.Core.Connectors;
using Aunir.SpectrumAnalysis2.Core.Default;
using Aunir.SpectrumAnalysis2.FossConnector;
using Aunir.SpectrumAnalysis2.Interfaces;
using Aunir.SpectrumAnalysis2.Interfaces.Constants;
using Aunir.SpectrumAnalysis2.JcampConnector;
using Serilog;
using SpectraMixtureCombineTool.Logic.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Reader
{
    internal sealed class SpectraReader
    {
        public List<AlchemySpectrumData> Read(IEnumerable<SpectraFile> files, string sampleReference)
        {
            var spectraFiles = ReadFiles(files, sampleReference);
            ValidateSpectra(spectraFiles);
            return spectraFiles.ToList();
        }

        private IEnumerable<AlchemySpectrumData> ReadFiles(IEnumerable<SpectraFile> files, string sampleReference)
        {
            var controller = new ReflectionConnectorController();
            controller.Initialise();
            foreach (var file in files)
            {
                using (var stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Delete))
                {
                    
                    var fileType = controller.IdentifyFile(stream);
                    if (fileType == null)
                    {
                        throw new Exception("File type not recognised by Spectrum Reader");
                    }
                    Log.Debug("Read filetype: {SpectraFileType}", fileType.TypeName);
                    var reader = controller.GetReaderForFileType(fileType).GenerateReader(new DelegateInformationProvider(_ => new Dictionary<string, string>()));
                    ISpectrumData spectrum = reader.ReadStream(stream).First();
                    spectrum.SpectrumInformation.Add(JcampInformationConstants.Ingredient + file.Ingredient, file.Coefficient.ToString());
                    spectrum.SpectrumInformation[InformationConstants.SampleReference] = sampleReference;

                    var spectrumData = new AlchemySpectrumData(spectrum.Wavelengths, spectrum.Data, spectrum.SpectrumInformation, spectrum.SpectrumReference, spectrum.InstrumentType, spectrum.InstrumentId, spectrum.XUnits, spectrum.YUnits)
                    {
                        Inclusion = file.Coefficient,
                        Name = file.Ingredient,
                        FileType = file.FileType
                    };
                    yield return spectrumData;
                }
            }
        }

        private void ValidateSpectra(IEnumerable<ISpectrumData> data)
        {
            var wavelengths = data.Select(x => x.Wavelengths);

            wavelengths.Aggregate((acc, next) =>
            {
                if (acc.Count != next.Count)
                    throw new Exception("Wavelength counts do not match in files");

                for (var i = 0; i < acc.Count; i++)
                {
                    if (acc[i] != next[i])
                        throw new Exception("Wavelengths do not match. Check resolution or start/end wavelengths in files.");
                }
                return acc;
            });
        }
    }
}

using Aunir.SpectrumAnalysis2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Reader
{
    interface IReader<T>
        where T : ISpectrumData
    {
        T Read(string path);
    }
}

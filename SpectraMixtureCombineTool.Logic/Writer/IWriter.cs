using Aunir.SpectrumAnalysis2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraMixtureCombineTool.Logic.Writer
{
    interface IWriter<T> 
        where T : ISpectrumData
    {
        void Write(string path, IEnumerable<T> data);
    }
}

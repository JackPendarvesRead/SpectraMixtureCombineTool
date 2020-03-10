using DynamicData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraMixtureCombineTool.Service
{
    public class SpectraFileCache
    {
        private readonly SourceCache<string, string> files = new SourceCache<string, string>(x => x);
        public IObservable<IChangeSet<string, string>> Connect() => files.Connect();

        public void AddFile(string filePath)
        {
            files.AddOrUpdate(filePath);
        }
    }
}

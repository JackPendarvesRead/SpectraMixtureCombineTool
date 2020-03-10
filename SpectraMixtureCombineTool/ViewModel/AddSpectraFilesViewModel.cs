using DynamicData;
using ReactiveUI;
using SpectraMixtureCombineTool.Service;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SpectraMixtureCombineTool.ViewModel
{
    public class AddSpectraFilesViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "AddSpectraFiles";
        public IScreen HostScreen { get; }       

        private readonly ReadOnlyObservableCollection<SpectraFileViewModel> _files;
        public ReadOnlyObservableCollection<SpectraFileViewModel> Files => _files;

        public AddSpectraFilesViewModel(SpectraFileCache cache, IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            cache.Connect()
                .Transform(x => new SpectraFileViewModel(x))
                .Bind(out _files)
                .Subscribe();
        }
    }
}

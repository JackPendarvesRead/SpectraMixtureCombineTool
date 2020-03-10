using ReactiveUI;
using SpectraMixtureCombineTool.Service;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpectraMixtureCombineTool.ViewModel
{
    public class MainWindowViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateAddSpectraFileViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddSpectraFileCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }

        private readonly SpectraFileCache cache = new SpectraFileCache();

        public MainWindowViewModel()
        {
            Router = new RoutingState();

            NavigateAddSpectraFileViewCommand = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new AddSpectraFilesViewModel(cache, this)));
            AddSpectraFileCommand = ReactiveCommand.Create(AddSpectraFileImpl);
            AddSpectraFileCommand = ReactiveCommand.Create(SaveImpl);

            NavigateAddSpectraFileViewCommand.Execute();
        }

        private void SaveImpl()
        {
            using(var sfd = new SaveFileDialog())
            {
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Saved the file!");
                }
            }
        }

        private void AddSpectraFileImpl()
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    cache.AddFile(ofd.FileName);
                }
            }
        }
    }
}

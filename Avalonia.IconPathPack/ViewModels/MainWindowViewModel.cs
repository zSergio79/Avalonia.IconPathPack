using Avalonia;
using Avalonia.Input.Platform;
using Avalonia.Media;
using Avalonia.ReactiveUI;

using DynamicData;
using DynamicData.Binding;

using ReactiveUI;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Avalonia.IconPathPack.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IActivatableViewModel
    {
        #region Activatable
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
        #endregion

        #region private 
        private IClipboard? _clipboard;
        private XmlNamespaceManager nsManager;
        #endregion

        private List<PathIconViewModel> sourceIcons = new();
       private ObservableCollection<PathIconViewModel> _icons;
        public ObservableCollection<PathIconViewModel> Icons
        {
            get => _icons;
            set => this.RaiseAndSetIfChanged(ref _icons, value);
        }

        private string _filter = string.Empty;
        public string Filter
        {
            get => _filter;
            set => this.RaiseAndSetIfChanged(ref _filter, value);
        }

        #region Commands
        public ReactiveCommand<PathIconViewModel, Unit> CopyToClipboard { get; }
        #endregion

        #region .ctor
        public MainWindowViewModel()
        {
            
        }
        public MainWindowViewModel(IClipboard? clipboard = null)
        {
            _clipboard = clipboard;
            nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("common", "https://github.com/avaloniaui");
            nsManager.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            this.WhenActivated((CompositeDisposable d) => { LoadIcons(); });

            CopyToClipboard = ReactiveCommand.Create<PathIconViewModel, Unit>((pivm) =>
            {
                var streamGeometry = $"<StreamGeometry x:Key=\"{pivm.Name}\">{pivm.Geometry}</StreamGeometry>";
                _clipboard?.SetTextAsync(streamGeometry); 
                return Unit.Default;
            });

            this.WhenAnyValue(x => x.Filter)
                .Throttle(TimeSpan.FromMilliseconds(600))
                .Subscribe(x => Icons = new ObservableCollection<PathIconViewModel>(sourceIcons.Where(i => i.Name.Contains(x, StringComparison.OrdinalIgnoreCase))));

        }
        #endregion

        #region Methods
        private void LoadIcons()
        {
            sourceIcons = new List<PathIconViewModel>();
            foreach (var file in Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Icons"), "*.xaml"))
            {
                try
                {
                    sourceIcons.AddRange(ParseFile(file));
                }
                catch 
                { }
            }
            Icons = new ObservableCollection<PathIconViewModel>(sourceIcons);
        }

        private IEnumerable<PathIconViewModel> ParseFile(string filename)
        {
            XDocument xDocument = XDocument.Load(filename);
            var all = xDocument.XPathSelectElements("//common:GeometryDrawing[@x:Key]",nsManager);
            return all.Select(x => PathIconViewModel.FromXml(x));
        }
        #endregion
    }
}

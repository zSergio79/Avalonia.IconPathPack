using Avalonia.Media;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Avalonia.IconPathPack.ViewModels
{
    public partial class PathIconViewModel : ViewModelBase
    {
        public string Name {  get; set; } = string.Empty;
        public string Geometry { get; set; } = string.Empty;

        public static PathIconViewModel FromXml(XElement xml)
        {
            var name = xml.Attribute("{http://schemas.microsoft.com/winfx/2006/xaml}Key")?.Value ?? string.Empty;
            var geometry = xml.Attribute("Geometry")?.Value ?? string.Empty;
            return new PathIconViewModel()
            {
                Name = name,
                Geometry = geometry,
            };
        }
    }
}

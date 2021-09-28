using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM3D2_MenuCategoryChanger
{
    internal class MenuObject
    {
        internal string Header { get; set; }
        internal int Version { get; set; }
        internal string TxtPath { get; set; }
        internal string Name { get; set; }
        internal string Category { get; set; }
        internal string Description { get; set; }
        internal List<Parameter> Parameters { get; set; } = new List<Parameter>();
    }

    internal class Parameter
    {
        internal string Name { get; set; }
        internal List<string> Settings { get; set; } = new List<string>();

        internal Parameter(string name, string[] settings)
        {
            Name = name;
            Settings.AddRange(settings);
        }
    }
}

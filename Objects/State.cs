using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeasureTape.Objects
{
    public class State
    {
        public int Width { get; set; } = 600;
        public int Height { get; set; } = 150;

        public int Step { get; set; } = 5;
        public int Corner { get; set; } = 0;
        public bool Inverted { get; set; } = false;
        public RenderSettings RenderSettings { get; set; } = new RenderSettings();
    }
}

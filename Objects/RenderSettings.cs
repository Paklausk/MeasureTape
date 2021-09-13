using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasureTape.Objects
{
    public class RenderSettings
    {
        public Color BackgroundColor { get; set; } = Color.FromArgb(0xff, 0xff, 0xe9, 0x1c);
        public Color InvertedBackgroundColor { get; set; } = Color.FromArgb(0xff, 0x0d, 0x39, 0x77);
        public Color PenColor { get; set; } = Color.Black;
        public Color InvertedPenColor { get; set; } = Color.White;
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasureTape.Objects
{
    public class RenderContext
    {
        public Color BackgroundColor { get; private set; }
        public Color Color { get; private set; }
        public Brush ColorBrush { get; private set; }
        public Pen ColorPen { get; private set; }
        public Font SizeFont { get; private set; }
        public Font MetricsFont { get; private set; }
        public int TextSkipDistance { get; } = 40;
        public int TextDistance { get; } = 30;
        public int LongMark { get; } = 20;
        public int MediumMark { get; } = 10;
        public int ShortMark { get; } = 5;
        public StringFormat CenterStringFormat { get; private set; } = new StringFormat(StringFormatFlags.NoWrap) { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        public RenderContext(State state)
        {
            Calculate(state.RenderSettings, state);
        }
        protected void Calculate(RenderSettings settings, State state)
        {
            BackgroundColor = state.Inverted ? settings.InvertedBackgroundColor : settings.BackgroundColor;
            Color = state.Inverted ? settings.InvertedPenColor : settings.PenColor;

            ColorBrush?.Dispose();
            ColorBrush = new SolidBrush(Color);
            ColorPen?.Dispose();
            ColorPen = new Pen(Color);

            SizeFont = FontHelper.CreateFont("Arial", 18, 600);
            MetricsFont = FontHelper.CreateFont("Arial", 14, 400);
        }
    }
}

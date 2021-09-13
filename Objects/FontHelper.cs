using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MeasureTape.Objects
{
    public static class FontHelper
    {
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateFont(int nHeight, int nWidth, int nEscapement, int nOrientation,
            int fnWeight, uint fdwItalic, uint fdwUnderline, uint fdwStrikeOut, uint fdwCharSet, 
            uint fdwOutputPrecision, uint fdwClipPrecision, uint fdwQuality, uint fdwPitchAndFamily, string lpszFace);
        
        public static Font CreateFont(string fontName, int fontSize, int fontWeight, bool italic = false)
        {
            IntPtr hFont = CreateFont(fontSize, 0, 0, 0, fontWeight, Convert.ToUInt32(italic), 0, 0, 1, 0, 0, 0, 0, fontName + "\0");
            return Font.FromHfont(hFont);
        }
    }
}

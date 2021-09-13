using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MeasureTape.Objects;

namespace MeasureTape
{
    public class MeasureTapeWindow : Form
    {
        const int MIN_STEP = 2, MAX_STEP = 100;
        StateSaver _stateSaver = new StateSaver();
        State _state;
        RenderContext _renderContext;
        public MeasureTapeWindow()
        {
            InitializeComponent();
            _state = _stateSaver.Load();
            _renderContext = new RenderContext(_state);
            Width = _state.Width;
            Height = _state.Height;
            DoubleBuffered = true;
        }
        private void MeasureTapeWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _stateSaver.Save(_state);
        }
        private void MeasureTapeWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
                case Keys.R:
                    _state.Corner = (_state.Corner + 1) % 4;
                    RecalculateAndInvalidate();
                    break;
                case Keys.I:
                    _state.Inverted = !_state.Inverted;
                    RecalculateAndInvalidate();
                    break;
                case Keys.Add:
                case Keys.Oemplus:
                    _state.Step = Math.Min(_state.Step + 1, MAX_STEP);
                    RecalculateAndInvalidate();
                    break;
                case Keys.Subtract:
                case Keys.OemMinus:
                    _state.Step = Math.Max(_state.Step - 1, MIN_STEP);
                    RecalculateAndInvalidate();
                    break;
                default:
                    string helpText = @$"Usable keys:

R - Rotate corner;
I - Invert color;
+ - Increase step;
- - Decrease step;

Current step = {_state.Step}";
                    MessageBox.Show(helpText, "Help", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    break;

            }
        }
        private void MeasureTapeWindow_Paint(object sender, PaintEventArgs e)
        {
            Render(e.Graphics);
        }
        private void MeasureTapeWindow_Resize(object sender, EventArgs e)
        {
            RecalculateAndInvalidate();
        }
        protected void Render(Graphics g)
        {
            #region DrawBackground
            g.Clear(_renderContext.BackgroundColor);
            #endregion
            #region Calculations
            int stepXSign = 0, stepYSign = 0;
            int offSetX = 0, offsetY = 0;
            switch (_state.Corner)
            {
                case 0:
                    stepXSign = 1;
                    stepYSign = 1;
                    offSetX = 0;
                    offsetY = 0;
                    break;
                case 1:
                    stepXSign = -1;
                    stepYSign = 1;
                    offSetX = Width;
                    offsetY = 0;
                    break;
                case 2:
                    stepXSign = -1;
                    stepYSign = -1;
                    offSetX = Width;
                    offsetY = Height;
                    break;
                case 3:
                    stepXSign = 1;
                    stepYSign = -1;
                    offSetX = 0;
                    offsetY = Height;
                    break;
                default:
                    break;
            }
            int tapeLength = Math.Max(Width, Height);
            #endregion
            #region DrawMarksAndLengths
            int markIndex = 1;
            while (tapeLength >= _state.Step * markIndex)
            {
                bool isLongMark = markIndex % 10 == 0, isMediumMark = markIndex % 5 == 0;
                int markLength = isLongMark ? _renderContext.LongMark : (isMediumMark ? _renderContext.MediumMark : _renderContext.ShortMark);
                {
                    int x = offSetX + _state.Step * stepXSign * markIndex, y = offsetY + _state.Step * stepYSign * markIndex;
                    int currentLength = markIndex * _state.Step;
                    string currentLengthString = currentLength.ToString();
                    bool drawCurrentXLength = isLongMark & (currentLength >= _renderContext.TextSkipDistance) & (currentLength <= Width - _renderContext.TextSkipDistance), drawCurrentYLength = isLongMark & (currentLength >= _renderContext.TextSkipDistance) & (currentLength <= Height - _renderContext.TextSkipDistance);
                    if (x < Width)
                    {
                        g.DrawLine(_renderContext.ColorPen, new Point(x, offsetY), new Point(x, offsetY + markLength * stepYSign));
                        g.DrawLine(_renderContext.ColorPen, new Point(x, offsetY ^ Height), new Point(x, (offsetY ^ Height) + markLength * stepYSign * -1));
                        if (drawCurrentXLength)
                        {
                            g.DrawString(currentLengthString, _renderContext.MetricsFont, _renderContext.ColorBrush, new Point(x, offsetY + _renderContext.TextDistance * stepYSign), _renderContext.CenterStringFormat);
                            g.DrawString(currentLengthString, _renderContext.MetricsFont, _renderContext.ColorBrush, new Point(x, (offsetY ^ Height) + _renderContext.TextDistance * stepYSign * -1), _renderContext.CenterStringFormat);
                        }
                    }
                    if (y < Height)
                    {
                        g.DrawLine(_renderContext.ColorPen, new Point(offSetX, y), new Point(offSetX + markLength * stepXSign, y));
                        g.DrawLine(_renderContext.ColorPen, new Point(offSetX ^ Width, y), new Point((offSetX ^ Width) + markLength * stepXSign * -1, y));
                        if (drawCurrentYLength)
                        {
                            g.DrawString(currentLengthString, _renderContext.MetricsFont, _renderContext.ColorBrush, new Point(offSetX + _renderContext.TextDistance * stepXSign, y), _renderContext.CenterStringFormat);
                            g.DrawString(currentLengthString, _renderContext.MetricsFont, _renderContext.ColorBrush, new Point((offSetX ^ Width) + _renderContext.TextDistance * stepXSign * -1, y), _renderContext.CenterStringFormat);
                        }
                    }
                }
                markIndex++;
            }
            #endregion
            #region DrawCornerMark
#if DEBUG
            g.FillRectangle(Brushes.Red, new Rectangle(offSetX -3, offsetY - 3, 6, 6));
#endif
            #endregion
            #region DrawSize
            g.DrawString($"{Width}x{Height}", _renderContext.SizeFont, _renderContext.ColorBrush, new Rectangle(Point.Empty, Size), _renderContext.CenterStringFormat);
            #endregion
        }
        protected void RecalculateAndInvalidate()
        {
            _renderContext = new RenderContext(_state);
            Invalidate();
        }
        #region Windows Form Designer generated code
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MeasureTapeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MeasureTapeWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MeasureTape";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MeasureTapeWindow_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MeasureTapeWindow_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MeasureTapeWindow_KeyDown);
            this.Resize += new System.EventHandler(this.MeasureTapeWindow_Resize);
            this.ResumeLayout(false);

        }
        #endregion
        #region WndProc
        private const int BORDER_WIDTH = 10;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84: // WM_NCHITTEST
                    {
                        Point pos = new Point(m.LParam.ToInt32());
                        pos = this.PointToClient(pos);

                        if (pos.X < BORDER_WIDTH && pos.Y < BORDER_WIDTH)
                        {
                            m.Result = (IntPtr)13; // HTTOPLEFT
                            return;
                        }
                        else if (pos.X > Width - BORDER_WIDTH && pos.Y < BORDER_WIDTH)
                        {
                            m.Result = (IntPtr)14; // HTTOPRIGHT
                            return;
                        }
                        else if (pos.X > Width - BORDER_WIDTH && pos.Y > Height - BORDER_WIDTH)
                        {
                            m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                            return;
                        }
                        else if (pos.X < BORDER_WIDTH && pos.Y > Height - BORDER_WIDTH)
                        {
                            m.Result = (IntPtr)16; // HTBOTTOMLEFT
                            return;
                        }
                        else if (pos.X < BORDER_WIDTH)
                        {
                            m.Result = (IntPtr)10; // HTLEFT
                            return;
                        }
                        else if (pos.Y < BORDER_WIDTH)
                        {
                            m.Result = (IntPtr)12; // HTTOP
                            return;
                        }
                        else if (pos.X > Width - BORDER_WIDTH)
                        {
                            m.Result = (IntPtr)11; // HTRIGHT
                            return;
                        }
                        else if (pos.Y > Height - BORDER_WIDTH)
                        {
                            m.Result = (IntPtr)15; // HTBOTTOM
                            return;
                        }
                        else
                        {
                            m.Result = (IntPtr)2;  // HTCAPTION
                            return;
                        }
                    }
                case 0x05: // WM_SIZE
                    if (_state != null)
                    {
                        var lparam = new Point(m.LParam.ToInt32());
                        _state.Width = lparam.X;
                        _state.Height = lparam.Y;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class RoundButton : Button
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            GraphicsPath grPath = new GraphicsPath();
            grPath.AddEllipse(1, 1, ClientSize.Width - 4, ClientSize.Height - 4);
            this.Region = new System.Drawing.Region(grPath);
            base.OnPaint(e);
        }
    }

}
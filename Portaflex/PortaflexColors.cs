using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Portaflex
{
    static class PortaflexColors
    {
        private static Color[] colors = { Color.AliceBlue, Color.BurlyWood, Color.Gold, Color.Khaki, Color.Salmon,
                                          Color.YellowGreen, Color.LightPink, Color.SandyBrown,Color.Plum, Color.MediumTurquoise, Color.Coral, Color.MediumAquamarine };

        public static Color getIndexColor(int index)
        {
            return colors[index % colors.Length];
        }

        public static Color dimColor(Color c, int level)
        {
            var color = Color.FromArgb(c.A, toRGBRange(c.R + level) , toRGBRange(c.G + level), toRGBRange(c.B + level));
            return color;
        }

        public static Color getColumnExpenseColor(int index)
        {
            return dimColor(getIndexColor(index), 50); 
        }

        public static Color getColumnIncomeColor(int index)
        {
            return dimColor(getIndexColor(index), 70);
        }

        private static int toRGBRange(int i)
        {
            if (i < 0)
                return 0;
            if (i > 255)
                return 255;
            return i;
        }

        public static void DrawTab(object sender, DrawItemEventArgs e)
        {
            var tabControl = (TabControl)sender;
            var CurrentTab = tabControl.TabPages[e.Index];
            var ItemRect = tabControl.GetTabRect(e.Index);
            var endColor = getIndexColor(e.Index == tabControl.TabCount - 1 ? 0 : e.Index);
            var startColor = dimColor(endColor, 50);
            var image = tabControl.ImageList.Images[CurrentTab.ImageIndex];
            var FillBrush = new LinearGradientBrush(ItemRect, startColor, endColor, (float) 90);
            var TextBrush = new SolidBrush(Color.Black);
            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            var font = e.Font;

            //If we are currently painting the Selected TabItem we'll
            //change the brush colors and inflate the rectangle.
            if (System.Convert.ToBoolean(e.State & DrawItemState.Selected))
            {
                font = new Font(font, FontStyle.Bold);            
                ItemRect.Inflate(2, 2);
            }

            //Set up rotation for left and right aligned tabs
            if (tabControl.Alignment == TabAlignment.Left || tabControl.Alignment == TabAlignment.Right)
            {
                float RotateAngle = 90;
                if (tabControl.Alignment == TabAlignment.Left)
                    RotateAngle = 270;
                var cp = new PointF(ItemRect.Left + (ItemRect.Width / 2), ItemRect.Top + (ItemRect.Height / 2));
                e.Graphics.TranslateTransform(cp.X, cp.Y);
                e.Graphics.RotateTransform(RotateAngle);
                ItemRect = new Rectangle(-(ItemRect.Height / 2), -(ItemRect.Width / 2), ItemRect.Height, ItemRect.Width);
            }



            //Next we'll paint the TabItem with our Fill Brush
            e.Graphics.FillRectangle(FillBrush, ItemRect);

            var imageRect = new Rectangle(ItemRect.X, ItemRect.Y, 0, 0);

            if (image != null)
            {
                imageRect = new Rectangle(ItemRect.X + 7, ItemRect.Y + (ItemRect.Height - image.Height)/2 , image.Width, image.Height);
                e.Graphics.DrawImage(image, imageRect);
            }

            //Now draw the text.

            var textRectangle = new Rectangle(ItemRect.X + 4 + imageRect.Width, ItemRect.Y, ItemRect.Width - 4 - imageRect.Width, ItemRect.Height);
            e.Graphics.DrawString(CurrentTab.Text, font, TextBrush, textRectangle, sf);

            //Reset any Graphics rotation
            e.Graphics.ResetTransform();

            //Finally, we should Dispose of our brushes.
            FillBrush.Dispose();
            TextBrush.Dispose();
        }
    }
}

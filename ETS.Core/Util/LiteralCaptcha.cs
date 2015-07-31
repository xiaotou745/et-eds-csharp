using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Util
{
    public interface ICaptcha
    {
        string Captcha { get; }
        string Result { get; }
    }


    public class LiteralCaptcha : ICaptcha
    {
        private int _width;
        private int _height;
        private int _charCount;

        public LiteralCaptcha(int width, int height, int charCount)
        {
            _width = width;
            _height = height;
            _charCount = charCount;
        }

        private string _captcha;

        public string Captcha
        {
            get
            {
                if (string.IsNullOrEmpty(_captcha))
                {
                    var rnd = new Random();
                    var number = Helper.GenCode(_charCount); //rnd.Next((int)Math.Pow(10, _charCount));
                    _captcha = number.ToString().PadLeft(_charCount, '0');
                }
                return _captcha;
            }
        }

        public string Result
        {
            get
            {
                return Captcha;
            }
        }

        public byte[] Generate()
        {
            using (var ms = new MemoryStream())
            using (var bmp = new Bitmap(_width, _height))
            using (var g = System.Drawing.Graphics.FromImage((Image)bmp))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                g.FillRectangle(Brushes.WhiteSmoke, new Rectangle(0, 0, bmp.Width, bmp.Height));

                g.DrawString(Captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);


                int c = 40;
                var rand = new Random();
                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(_width);
                    int y = rand.Next(_height);
                    g.DrawRectangle(Pens.Gray, x, y, 1, 1);
                }

                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                return ms.ToArray();
            }
        }
    }

}

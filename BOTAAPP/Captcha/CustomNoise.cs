using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace XCaptchaSample.Captcha
{
    public class CustomNoise : XCaptcha.Noise
    {

        public CustomNoise()
            : base(new HatchBrush(HatchStyle.SmallConfetti, Color.Gray, Color.Gray))
        {
            
        }


        public override void Create(Graphics graphics, XCaptcha.ICanvas canvas)
        {
            var random = new Random();


            for (var i = 0; i < (canvas.Width * canvas.Height / 10); i++)
            {
                var x = random.Next(canvas.Width);
                var y = random.Next(canvas.Height);
                var w = random.Next(3);
                var h = random.Next(3);

                graphics.FillEllipse(Brush, x, y, w, h);
            }

        }

    }
}
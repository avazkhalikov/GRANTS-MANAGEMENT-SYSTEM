
using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace XCaptchaSample.Captcha
{
    public class CustomDistort:XCaptcha.Distort
    {

        public override GraphicsPath Create(GraphicsPath path, XCaptcha.ICanvas canvas)
        {


            var random = new Random();
            var rect = new Rectangle(0, 0, canvas.Width, canvas.Height);

            const float wrapFactor = 8F;

            PointF[] points =
                {
                    new PointF(random.Next(rect.Width) / wrapFactor,random.Next(rect.Height) / wrapFactor),
                    new PointF(rect.Width - random.Next(rect.Width) / wrapFactor, random.Next(rect.Height) / wrapFactor),
                    new PointF(random.Next(rect.Width) / wrapFactor, rect.Height - random.Next(rect.Height) / wrapFactor),
                    new PointF(rect.Width - random.Next(rect.Width) / wrapFactor, rect.Height - random.Next(rect.Height) / wrapFactor)
                };

            var matrix = new Matrix();
            matrix.Translate(0F, 0F);

            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            return path;
        }
    }
}
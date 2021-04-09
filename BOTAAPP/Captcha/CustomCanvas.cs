using System.Drawing;
using System.Drawing.Drawing2D;

namespace XCaptchaSample.Captcha
{

    public class CustomCanvas : XCaptcha.Canvas
    {
        public CustomCanvas()
            : base(150 /* width*/, 50 /*height*/, new HatchBrush(HatchStyle.Percent50, Color.LightGray, Color.LightGray) /* brush */)
        {
            
        }
    }
}

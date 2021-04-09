using System.Drawing.Drawing2D;
using System.Drawing;

namespace XCaptchaSample.Captcha
{
    public class CustomTextStyle : XCaptcha.TextStyle
    {
        public CustomTextStyle()
            : base(new Font("Consolas", 36, FontStyle.Regular) /*font*/,
            new HatchBrush(HatchStyle.Percent50, Color.LightGray, Color.Gray) /*brush*/)
        {
            
        }
    }
}
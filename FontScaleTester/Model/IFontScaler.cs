using FontScaleTester.Drawing;

namespace FontScaleTester.Model
{
	public interface IFontScaler
	{
		FontBank.FontDescription GetScaledFont(string text, RectangleF targetBounds, FontBank.FontDescription startingFont);
	}
}

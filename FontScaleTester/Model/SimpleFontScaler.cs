using FontScaleTester.Drawing;
using System;

namespace FontScaleTester.Model
{
	/// <summary>
	/// A simple font scaler that uses brute force comparisons to find a font that fits into the target bounds.
	/// </summary>
	public class SimpleFontScaler : IFontScaler
	{
		private FontBank _fontBank;

		public SimpleFontScaler(FontBank fontBank)
		{
			_fontBank = fontBank ?? throw new ArgumentNullException();
		}

		public FontBank.FontDescription GetScaledFont(string text, RectangleF targetBounds, FontBank.FontDescription startingFont)
		{
			FontBank.FontDescription font = startingFont;
			SizeF targetSize = targetBounds.Size;
			SizeF measuredSize = SizeF.Empty;
			int compareResult = -1;
			int lastCompareResult = 0;
			FontBank.FontDescription prevFont = font;

			do
			{
				measuredSize = (SizeF)font.Font.MeasureString(text);
				compareResult = CompareFit(targetSize, measuredSize);

				if (compareResult < 0)
					font = _fontBank.GetNextSizeSmaller(font);
				else if (compareResult > 0)
					font = _fontBank.GetNextSizeLarger(font);

				if (lastCompareResult != 0 && lastCompareResult != compareResult)
					font = prevFont;

				if (prevFont.FontSize == font.FontSize)
					break;

				lastCompareResult = compareResult;
				prevFont = font;
			} while (compareResult != 0);

			return font;
		}

		private int CompareFit(SizeF target, SizeF other, float tol = 0.1f )
		{
			int result = 0;

			// Check widths
			float widthDiff = target.Width - other.Width;
			float widthTol = target.Width * tol;
			if (Math.Abs(widthDiff) > widthTol)
				result = (other.Width > target.Width) ? -1 : 1;

			// Can also do height... I just don't feel like it :D

			return result;
		}
	}
}

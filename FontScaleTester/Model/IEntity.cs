using FontScaleTester.Drawing;
using Microsoft.Xna.Framework;

namespace FontScaleTester.Model
{
	public interface IEntity
	{
		Color Colour { get; set; }
		RectangleF Bounds { get; set; }
	}
}

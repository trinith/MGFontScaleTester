using FontScaleTester.Drawing;
using Microsoft.Xna.Framework;

namespace FontScaleTester.Model
{
	public class EntityBase : IEntity
	{
		public Color Colour { get; set; } = Color.White;
		public RectangleF Bounds { get; set; } = new RectangleF(0, 0, 1, 1);
	}
}

using FontScaleTester.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontScaleTester.View
{
	public class EditorView : ViewBase
	{
		private SpriteBatch _spriteBatch;

		public IEntityView EditingView { get; private set; }
		public bool SizeHandleIsHot { get; set; } = false;
		public bool ControlIsHot { get; set; } = false;
		public float SelectionDistance { get; private set; } = 3;

		public override void SetNeedsRefresh()
		{
			this.EditingView.SetNeedsRefresh();
		}

		public EditorView(IEntityView view, SpriteBatch spriteBatch) 
			: base(view.Entity)
		{
			this.EditingView = view ?? throw new ArgumentNullException();
			_spriteBatch = spriteBatch ?? throw new ArgumentNullException();
		}

		public override void Draw(GameTime gameTime)
		{
			this.EditingView.Draw(gameTime);

			Color controlColour = (this.ControlIsHot) ? Color.White : Color.Gray;
			_spriteBatch.DrawRectangle((Rectangle)this.EditingView.Entity.Bounds, controlColour);

			Color handleColour = (this.SizeHandleIsHot) ? Color.LimeGreen : Color.DarkGreen;
			_spriteBatch.DrawCircle(this.EditingView.Entity.Bounds.OppositeCorner, this.SelectionDistance, 8, handleColour);
		}
	}
}

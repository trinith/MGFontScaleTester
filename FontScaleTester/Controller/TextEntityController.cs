using FontScaleTester.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontScaleTester.Controller
{
	public class TextEntityViewController : IController
	{
		private TextEntityView _view;
		private GraphicsDevice _device;
		private SpriteBatch _spriteBatch;

		public TextEntityViewController(TextEntityView view, GraphicsDevice device, SpriteBatch spriteBatch)
		{
			_view = view ?? throw new ArgumentNullException();
			_device = device ?? throw new ArgumentNullException();
			_spriteBatch = spriteBatch ?? throw new ArgumentNullException();
		}

		public void Update(GameTime gameTime)
		{
			if (_view.NeedsRefresh)
			{
				_view.RefreshCachedTexture(_device, _spriteBatch);
			}
		}
	}
}

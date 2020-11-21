using FontScaleTester.Drawing;
using FontScaleTester.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FontScaleTester.View
{
	public class TextEntityView : ViewBase
	{
		private IFontScaler _fontScaler;
		private SpriteBatch _spriteBatch;
		private FontBank _fontBank;

		private ITextEntity _textEntity;
		private FontBank.FontDescription _font;

		private FontBank.FontDescription _cachedFont;
		private RenderTarget2D _cachedTexture = null;
		private Point _cachedTextureScaledSize;

		public override bool NeedsRefresh
		{
			get { return _cachedTexture == null; }
		}

		public override void SetNeedsRefresh()
		{
			_cachedTexture = null;
		}

		public TextEntityView(IEntity entity, IFontScaler fontScaler, SpriteBatch spriteBatch, FontBank fontBank) 
			: base(entity)
		{
			_fontScaler = fontScaler ?? throw new ArgumentNullException();
			_spriteBatch = spriteBatch ?? throw new ArgumentNullException();
			_fontBank = fontBank ?? throw new ArgumentNullException();

			_textEntity = this.GetEntityAs<ITextEntity>();
			_font = _fontBank.GetFont(_textEntity.FontName, _textEntity.FontSize);

			_cachedFont = _font;
		}

		private SizeF GetScaledSize(SizeF size, SizeF targetSize)
		{
			SizeF scaledSize = size;
			scaledSize.Height = (int)(scaledSize.Width / (targetSize.Width / (float)targetSize.Height));

			// Not bothering with the case where width needs to scale to height.

			return scaledSize;
		}

		public void RefreshCachedTexture(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			FontBank.FontDescription renderFont = _font;

			if (_textEntity.Behaviour == TextEntityBehaviour.AutoScale)
				renderFont = _fontScaler.GetScaledFont(_textEntity.Text, _textEntity.Bounds, renderFont);

			switch (_textEntity.Behaviour)
			{
				case TextEntityBehaviour.ClipToBounds:
					_cachedTexture = new RenderTarget2D(device, (int)_textEntity.Bounds.Width, (int)_textEntity.Bounds.Height);
					break;
				case TextEntityBehaviour.AutoScale:
				case TextEntityBehaviour.Default:
				default:
					Vector2 textSize = renderFont.Font.MeasureString(_textEntity.Text);
					_cachedTexture = new RenderTarget2D(device, (int)textSize.X, (int)textSize.Y);
					break;
			}

			var oldTargets = device.GetRenderTargets();
			device.SetRenderTarget(_cachedTexture);

			spriteBatch.Begin();
			spriteBatch.DrawString(renderFont.Font, _textEntity.Text, Vector2.Zero, Color.White);
			spriteBatch.End();

			device.SetRenderTargets(oldTargets);

			_cachedFont = renderFont;
			_cachedTextureScaledSize = (Point)GetScaledSize(_textEntity.Bounds.Size, new SizeF(_cachedTexture.Width, _cachedTexture.Height));
		}

		public override void Draw(GameTime gameTime)
		{
			// Draw the text.
			if (_cachedTexture != null)
			{
				switch (_textEntity.Behaviour)
				{
					case TextEntityBehaviour.AutoScale:
						_spriteBatch.Draw(_cachedTexture, new Rectangle(_textEntity.Bounds.Location.ToPoint(), _cachedTextureScaledSize), _textEntity.Colour);
						break;
					default:
						_spriteBatch.Draw(_cachedTexture, _textEntity.Bounds.Location, _textEntity.Colour);
						break;
				}
			}

			// Debug draw which font got used.
			_spriteBatch.DrawString(_fontBank.GetFont("Calibri", 12).Font, _cachedFont.ToString(), Vector2.Zero, Color.White);
		}
	}
}

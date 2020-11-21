using FontScaleTester.Controller;
using FontScaleTester.Drawing;
using FontScaleTester.Model;
using FontScaleTester.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FontScaleTester
{
	public class MainGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private List<IEntityView> _views = new List<IEntityView>();
		private List<IController> _controllers = new List<IController>();

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			this.IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = 1820;
			_graphics.PreferredBackBufferHeight = 1024;
			_graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			FontBank fontBank = new FontBank();
			fontBank.Load(this.Content, "Fonts");

			IFontScaler fontScaler = new SimpleFontScaler(fontBank);

			var textEntity = new TextEntity()
			{
				Colour = Color.Cyan,
				Text = "This is a test.",
				Bounds = new RectangleF(100, 100, 100, 25),
				FontName = "Calibri",
				FontSize = 20,
				Behaviour = TextEntityBehaviour.AutoScale,
			};

			var textEntityView = new TextEntityView(
				textEntity,
				fontScaler,
				_spriteBatch,
				fontBank
			);

			var textEntityEditorView = new EditorView(
				textEntityView,
				_spriteBatch
			);

			_views.Add(textEntityEditorView);
			_controllers.Add(new EditorViewController(textEntityEditorView));
			_controllers.Add(new TextEntityViewController(textEntityView, _graphics.GraphicsDevice, _spriteBatch));
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			foreach (var controller in _controllers)
				controller.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			_graphics.GraphicsDevice.Clear(Color.Black);

			_spriteBatch.Begin();

			foreach (var view in _views)
				view.Draw(gameTime);

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

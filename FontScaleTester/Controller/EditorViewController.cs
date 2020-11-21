using FontScaleTester.Drawing;
using FontScaleTester.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FontScaleTester.Controller
{
	public class EditorViewController : IController
	{
		private EditorView _view;
		private MouseState _prevMS;

		private float _selectionTolerance = 1.25f;
		private Vector2? _sizeHandleDragOffset = null;
		private Vector2? _controlDragOffset = null;

		public EditorViewController(EditorView view)
		{
			_view = view ?? throw new ArgumentNullException();
		}

		public void Update(GameTime gameTime)
		{
			MouseState ms = Mouse.GetState();

			Vector2 mousePos = ms.Position.ToVector2();
			_view.SizeHandleIsHot = Vector2.Distance(mousePos, _view.Entity.Bounds.OppositeCorner) <= _view.SelectionDistance * _selectionTolerance;
			_view.ControlIsHot = !_view.SizeHandleIsHot && _view.Entity.Bounds.Contains(ms.Position.ToVector2());

			if (ms.LeftButton == ButtonState.Pressed && _prevMS.LeftButton == ButtonState.Released)
			{
				if (_view.SizeHandleIsHot)
					_sizeHandleDragOffset = ms.Position.ToVector2() - _view.Entity.Bounds.OppositeCorner;
				else if (_view.ControlIsHot && _controlDragOffset == null)
					_controlDragOffset = ms.Position.ToVector2() - _view.Entity.Bounds.Location;
			}
			else if (ms.LeftButton == ButtonState.Released && _prevMS.LeftButton == ButtonState.Pressed)
			{
				_sizeHandleDragOffset = null;
				_controlDragOffset = null;
			}

			if (_sizeHandleDragOffset != null)
			{
				SizeF oldSize = _view.Entity.Bounds.Size;

				RectangleF newBounds = new RectangleF(_view.Entity.Bounds.Location, ms.Position.ToVector2() - _sizeHandleDragOffset.Value);
				newBounds.Width = Math.Max(newBounds.Width, 1);
				newBounds.Height = Math.Max(newBounds.Height, 1);
				_view.Entity.Bounds = newBounds;

				if (oldSize != _view.Entity.Bounds.Size)
					_view.SetNeedsRefresh();
			}
			else if (_controlDragOffset != null)
			{
				Vector2 newPosition = ms.Position.ToVector2() - _controlDragOffset.Value;
				RectangleF newBounds = new RectangleF(newPosition, _view.Entity.Bounds.Size);
				_view.Entity.Bounds = newBounds;
			}

			_prevMS = ms;
		}
	}
}

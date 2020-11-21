using FontScaleTester.Model;
using Microsoft.Xna.Framework;
using System;

namespace FontScaleTester.View
{
	public abstract class ViewBase : IEntityView
	{
		public IEntity Entity { get; private set; }
		public virtual bool NeedsRefresh { get; } = false;

		public virtual void SetNeedsRefresh() { }

		public T GetEntityAs<T>() where T : IEntity
		{
			return (T)this.Entity;
		}

		public ViewBase(IEntity entity)
		{
			this.Entity = entity ?? throw new ArgumentNullException();
		}

		public abstract void Draw(GameTime gameTime);
	}
}

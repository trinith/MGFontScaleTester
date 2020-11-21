using FontScaleTester.Model;
using Microsoft.Xna.Framework;

namespace FontScaleTester.View
{
	public interface IEntityView
	{
		IEntity Entity { get; }
		bool NeedsRefresh { get; }

		void SetNeedsRefresh();
		T GetEntityAs<T>() where T : IEntity;

		void Draw(GameTime gameTime);
	}
}

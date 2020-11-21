namespace FontScaleTester.Model
{
	public enum TextEntityBehaviour
	{
		Default,
		ClipToBounds,
		AutoScale,
	}

	public interface ITextEntity : IEntity
	{
		string FontName { get; set; }
		int FontSize { get; set; }
		string Text { get; set; }
		TextEntityBehaviour Behaviour { get; set; }
	}
}

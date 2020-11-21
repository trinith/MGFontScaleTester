namespace FontScaleTester.Model
{
	public class TextEntity : EntityBase, ITextEntity
	{
		public string FontName { get; set; } = "";
		public int FontSize { get; set; } = 12;
		public string Text { get; set; } = "TextEntity";
		public TextEntityBehaviour Behaviour { get; set; } = TextEntityBehaviour.Default;
	}
}

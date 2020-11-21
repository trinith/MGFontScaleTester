using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace FontScaleTester.Model
{
	public class FontBank
	{
		public struct FontDescription
		{
			public SpriteFont Font { get; set; }
			public string FontName { get; set; }
			public int FontSize { get; set; }

			public override string ToString()
			{
				return $"{{FontName={this.FontName}, FontSize={this.FontSize}}}";
			}
		}

		private Dictionary<string, Dictionary<int, FontDescription>> _fonts = new Dictionary<string, Dictionary<int, FontDescription>>();
		private Dictionary<string, List<int>> _fontSizes = new Dictionary<string, List<int>>();

		public void Load(ContentManager content, string fontFolder)
		{
			string folder = Path.Combine(content.RootDirectory, fontFolder);

			foreach (var fontFile in Directory.GetFiles(folder))
			{
				int startIndex = fontFile.IndexOf(content.RootDirectory) + content.RootDirectory.Length;
				string assetPath = fontFile.Substring(startIndex + 1);

				int extIndex = assetPath.LastIndexOf(".xnb");
				assetPath = assetPath.Substring(0, extIndex);

				SpriteFont font = content.Load<SpriteFont>(assetPath);
				string assetName = Path.GetFileNameWithoutExtension(assetPath);
				int lastUnderscore = assetName.LastIndexOf('_');

				string fontName = assetName.Substring(0, lastUnderscore);
				int fontSize = int.Parse(assetName.Substring(lastUnderscore + 1));

				if (!_fonts.ContainsKey(fontName))
				{
					_fonts.Add(fontName, new Dictionary<int, FontDescription>());
					_fontSizes.Add(fontName, new List<int>());
				}

				_fonts[fontName].Add(fontSize, new FontDescription() { Font = font, FontName = fontName, FontSize = fontSize });
				_fontSizes[fontName].Add(fontSize);
				_fontSizes[fontName].Sort((lhs, rhs) => lhs.CompareTo(rhs));
			}
		}

		public FontDescription GetFont(string name, int size)
		{
			return _fonts[name][size];
		}

		// NOTE: These two methods can defintely have better algorithms... but for now, I don't care.

		public FontDescription GetNextSizeSmaller(FontDescription desc)
		{
			List<int> sizes = _fontSizes[desc.FontName];

			int index = 0;
			for (index = 0; index < sizes.Count; index++)
			{
				if (sizes[index] == desc.FontSize)
					break;
			}

			if (index > 0)
				index--;

			return _fonts[desc.FontName][sizes[index]];
		}

		public FontDescription GetNextSizeLarger(FontDescription desc)
		{
			List<int> sizes = _fontSizes[desc.FontName];

			int index = 0;
			for (index = 0; index < sizes.Count; index++)
			{
				if (sizes[index] == desc.FontSize)
					break;
			}

			if (index < sizes.Count - 1)
				index++;

			return _fonts[desc.FontName][sizes[index]];
		}
	}
}

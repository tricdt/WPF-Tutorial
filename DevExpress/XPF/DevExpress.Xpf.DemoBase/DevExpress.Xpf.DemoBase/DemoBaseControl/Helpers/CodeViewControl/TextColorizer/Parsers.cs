#region Copyright (c) 2000-2024 Developer Express Inc.
/*
{*******************************************************************}
{                                                                   }
{       Developer Express .NET Component Library                    }
{                                                                   }
{                                                                   }
{       Copyright (c) 2000-2024 Developer Express Inc.              }
{       ALL RIGHTS RESERVED                                         }
{                                                                   }
{   The entire contents of this file is protected by U.S. and       }
{   International Copyright Laws. Unauthorized reproduction,        }
{   reverse-engineering, and distribution of all or any portion of  }
{   the code contained in this file is strictly prohibited and may  }
{   result in severe civil and criminal penalties and will be       }
{   prosecuted to the maximum extent possible under the law.        }
{                                                                   }
{   RESTRICTIONS                                                    }
{                                                                   }
{   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           }
{   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          }
{   SECRETS OF DEVELOPER EXPRESS INC. THE REGISTERED DEVELOPER IS   }
{   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    }
{   CONTROLS AS PART OF AN EXECUTABLE PROGRAM ONLY.                 }
{                                                                   }
{   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      }
{   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        }
{   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       }
{   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  }
{   AND PERMISSION FROM DEVELOPER EXPRESS INC.                      }
{                                                                   }
{   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       }
{   ADDITIONAL RESTRICTIONS.                                        }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2000-2024 Developer Express Inc.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
namespace DevExpress.Xpf.DemoBase.Helpers.TextColorizer.Internal {
	enum TokenType { Error = 0, Block, Symbol, Object, Property, Value, Space, LineBreak, Complex, Comment, PlainText, String, Keyword, CollapsedRegion, Attribute }
	class CodeLexem {
		const double Opacity = 0.65;
		const double DarkOpacity = 0.5;
		public static readonly Color CollapsedRegionColor = Color.FromArgb(255, 128, 128, 128);
		public static readonly Color AttributeColor = Color.FromArgb(255, 43, 145, 175);
		public bool Highlighted { get; set; }
		public string Text { get; set; }
		public TokenType Type { get; set; }
		public CodeLexem(bool highlighted) : this("", highlighted) { }
		public CodeLexem(string text, bool highlighted) : this(TokenType.Complex, text, highlighted) { }
		public CodeLexem(TokenType type, string text, bool highlighted) {
			Text = text;
			Type = type;
			Highlighted = highlighted;
		}
		public List<CodeLexem> Parse(CodeLanguage lang, bool highlighted) {
			switch(lang) {
				case CodeLanguage.Plain: return (new BaseParser()).Parse(Text, highlighted);
				case CodeLanguage.XAML: return (new XamlParser()).Parse(Text, highlighted);
				case CodeLanguage.CS: return (new CSParser()).Parse(Text, highlighted);
				case CodeLanguage.VB: return (new VBParser()).Parse(Text, highlighted);
			}
			return null;
		}
		Color GetCompatableColor(Color color, bool isDark) {
			return Highlighted ? color : Color.FromArgb((byte)(color.A * (isDark ? DarkOpacity : Opacity)), color.R, color.G, color.B);
		}
		protected Run CreateRun(string text, Color color) { return CreateRun(text, color, false); }
		protected Run CreateDarkRun(string text, Color color) { return CreateRun(text, color, true); }
		protected Run CreateRun(string text, Color color, bool isDark) { return new Run() { Text = text, Foreground = new SolidColorBrush(GetCompatableColor(color, isDark)) }; }
		public Inline ToInline(bool isDark = false) {
			if(isDark) {
				var darkWhite = Color.FromRgb(230, 230, 230);
				var red = Colors.Red;
				switch(Type) {
					case TokenType.Complex: return CreateDarkRun(Text, darkWhite);
					case TokenType.LineBreak: return CreateDarkRun("\n", darkWhite);
					case TokenType.Object: return CreateDarkRun(Text, darkWhite);
					case TokenType.Property: return CreateDarkRun(Text, Color.FromRgb(146, 202, 244));
					case TokenType.Space: return CreateDarkRun(Text, darkWhite);
					case TokenType.Symbol: return CreateDarkRun(Text, Color.FromRgb(110, 127, 127));
					case TokenType.Value: return CreateDarkRun(Text, Color.FromRgb(86, 156, 214));
					case TokenType.PlainText: return CreateDarkRun(Text, darkWhite);
					case TokenType.Comment: return CreateDarkRun(Text, Color.FromRgb(86,164 , 57));
					case TokenType.Error: return CreateDarkRun(Text, darkWhite);
					case TokenType.String: return CreateDarkRun(Text, Color.FromRgb(214, 157, 133));
					case TokenType.Keyword: return CreateDarkRun(Text, Color.FromRgb(64, 144, 212));
					case TokenType.Attribute: return CreateDarkRun(Text, Color.FromRgb(78, 201, 176));
					case TokenType.CollapsedRegion: return CreateDarkRun(Text, Color.FromRgb(128, 128, 128));
				}
			} else
				switch(Type) {
					case TokenType.Complex: return CreateRun(Text, Colors.LightGray);
					case TokenType.LineBreak: return CreateRun("\n", Colors.Black);
					case TokenType.Object: return CreateRun(Text, Colors.Brown);
					case TokenType.Property: return CreateRun(Text, Colors.Red);
					case TokenType.Space: return CreateRun(Text, Colors.Black);
					case TokenType.Symbol: return CreateRun(Text, Colors.Blue);
					case TokenType.Value: return CreateRun(Text, Colors.Blue);
					case TokenType.PlainText: return CreateRun(Text, Colors.Black);
					case TokenType.Comment: return CreateRun(Text, Colors.Green);
					case TokenType.Error: return CreateRun(Text, Colors.LightGray);
					case TokenType.String: return CreateRun(Text, Colors.Brown);
					case TokenType.Keyword: return CreateRun(Text, Colors.Blue);
					case TokenType.Attribute: return CreateRun(Text, AttributeColor);
					case TokenType.CollapsedRegion: return CreateRun(Text, CollapsedRegionColor);
				}
			return null;
		}
	}
	class SourceString {
		string Source { get; set; }
		int StartIndex { get; set; }
		public SourceString(string source, int startIndex) {
			if(startIndex > source.Length)
				throw new ArgumentException();
			this.Source = source;
			this.StartIndex = startIndex;
		}
		public int Length { get { return Source.Length - StartIndex; } }
		public char this[int index] { get { return Source[StartIndex + index]; } }
		internal int IndexOf(string text) {
			int index = Source.IndexOf(text, StartIndex, StringComparison.Ordinal);
			if(index < 0)
				return index;
			return index - StartIndex;
		}
		internal int IndexOf(string value, int startIndex) {
			int index = Source.IndexOf(value, StartIndex + startIndex, StringComparison.Ordinal);
			if(index < 0)
				return index;
			return index - StartIndex;
		}
		internal int IndexOfAny(char[] anyOf) {
			int index = Source.IndexOfAny(anyOf, StartIndex);
			if(index < 0)
				return index;
			return index - StartIndex;
		}
		internal int IndexOf(char value) {
			int index = Source.IndexOf(value, StartIndex);
			if(index < 0)
				return index;
			return index - StartIndex;
		}
		internal string Substring(int startIndex, int length) {
			return Source.Substring(StartIndex + startIndex, length);
		}
		internal SourceString Substring(int startIndex) {
			return new SourceString(Source, StartIndex + startIndex);
		}
		internal bool StartsWith(string value, StringComparison comparisonType) {
			return string.Compare(Source, StartIndex, value, 0, value.Length, comparisonType) == 0;
		}
		internal char? PreviousSymbol(int dIndex) {
			int index = StartIndex - dIndex;
			if(index < 0)
				return null;
			return Source[index];
		}
		public int Position { get { return StartIndex; } }
		public string Text { get { return Source; } }
	}
	class BaseParser {
		bool caseSensitive;
		char[] SpaceChars = { ' ', '	' };
		public BaseParser() : this(true) { }
		public BaseParser(bool caseSensitive) {
			this.caseSensitive = caseSensitive;
		}
		protected char previousSymbol;
		protected string StringCut(ref SourceString text, int count) {
			if(count == 0)
				return string.Empty;
			previousSymbol = text[count - 1];
			string result = text.Substring(0, count);
			text = text.Substring(count);
			return result;
		}
		protected void TrySpace(List<CodeLexem> res, ref SourceString text, bool highlighted) {
			StringBuilder spaces = new StringBuilder();
			while(SpaceChars.Contains(text[0]))
				spaces.Append(StringCut(ref text, 1));
			if(spaces.Length > 0)
				res.Add(new CodeLexem(TokenType.Space, spaces.ToString(), highlighted));
		}
		protected bool TryExtract(List<CodeLexem> res, ref SourceString text, string lex, TokenType type, bool highlighted) {
			if(text.StartsWith(lex, StringComparison.Ordinal)) {
				res.Add(new CodeLexem(type, StringCut(ref text, lex.Length), highlighted));
				return true;
			}
			return false;
		}
		protected void TryExtractTo(List<CodeLexem> res, ref SourceString text, string lex, TokenType type, string except, bool highlighted) {
			int index = text.IndexOf(lex);
			if(except != null)
				while(index >= 0 && text.Substring(0, index + 1).EndsWith(except, StringComparison.Ordinal))
					index = text.IndexOf(lex, index + 1);
			if(index < 0) return;
			BreakLines(res, ref text, index + lex.Length, type, highlighted);
		}
		protected void TryMatchRegex(List<CodeLexem> res, ref SourceString text, string regex, TokenType type, bool highlighted) {
			var rx = new Regex(regex);
			var m = rx.Match(text.Text, text.Position);
			if (!m.Success)
				return;
			BreakLines(res, ref text, m.Index - text.Position + m.Length, type, highlighted);
		}
		protected void BreakLines(List<CodeLexem> res, ref SourceString text, int to, TokenType type, bool highlighted) {
			while(text.Length > 0 && to > 0) {
				int index = text.IndexOf("\n");
				if(index >= to) {
					res.Add(new CodeLexem(type, StringCut(ref text, to), highlighted));
					break;
				}
				if(index != 0) res.Add(new CodeLexem(type, StringCut(ref text, index), highlighted));
				res.Add(new CodeLexem(TokenType.LineBreak, StringCut(ref text, 1), highlighted));
				to -= index + 1;
			}
		}
		public List<CodeLexem> Parse(string text, bool highlighted) {
			var res = Parse(new SourceString(text + "\n", 0), highlighted);
			if(res.Count != 0 && res[res.Count - 1].Type == TokenType.LineBreak)
				res.RemoveAt(res.Count - 1);
			return res;
		}
		protected virtual List<CodeLexem> Parse(SourceString text, bool highlighted) {
			List<CodeLexem> res = new List<CodeLexem>();
			SourceString extendedText = text;
			BreakLines(res, ref extendedText, extendedText.Length, TokenType.PlainText, highlighted);
			return res;
		}
	}
	internal class CSParser : BaseParser {
		char[] CSEndOfTerm = { ' ', '\t', '\n', '=', '/', '>', '<', '"', '{', '}', ',', '(', ')', ';', '\0', '?' };
		string[] CSKeyWords = { "abstract","event","new","struct","as","explicit","null",
								"switch","base","extern","object","this","bool","false",
								"operator","throw","break","finally","out","true","byte",
								"fixed","override","try","case","float","params","typeof",
								"catch","for","private","uint","char","foreach","protected",
								"ulong","checked","goto","public","unchecked","class",
								"if","readonly","unsafe","const","implicit","ref","ushort",
								"continue","in","return","using","decimal","int","sbyte",
								"virtual","default","interface","sealed","volatile","delegate",
								"internal","short","void","do","is","sizeof","while",
								"double","lock","stackalloc","else","long","static","enum",
								"namespace","string","from","get","group","into","join","let",
								"orderby","partial","select","set","var","where","yield", "async", "await",
								"#region","#endregion","#if","#endif"};
		public CSParser() { }
		protected override List<CodeLexem> Parse(SourceString text, bool highlighted) {
			SourceString extendedText = text;
			List<CodeLexem> res = new List<CodeLexem>();
			while(extendedText.Length > 0) {
				TrySpace(res, ref extendedText, highlighted);
				if (TryExtract(res, ref extendedText, "/*", TokenType.Comment, highlighted)) {
					TryExtractTo(res, ref extendedText, "*/", TokenType.Comment, null, highlighted);
				}
				if(TryExtract(res, ref extendedText, "//", TokenType.Comment, highlighted)) {
					TryExtractTo(res, ref extendedText, "\n", TokenType.Comment, null, highlighted);
				}
				if (TryExtract(res, ref extendedText, "\"", TokenType.String, highlighted)) {
					TryMatchRegex(res, ref extendedText, "((\\.)|[^\\\\\"])*\"", TokenType.String, highlighted);
				}
				if (TryExtract(res, ref extendedText, "'", TokenType.String, highlighted)) {
					TryExtractTo(res, ref extendedText, "'", TokenType.String, null, highlighted);
				}
				if(TryExtract(res, ref extendedText, "[", TokenType.PlainText, highlighted)) {
					ParseCSAttribute(res, ref extendedText, highlighted);
				}
				ParseCSKeyWord(res, ref extendedText, TokenType.Keyword, highlighted);
				ParseCSSymbol(res, ref extendedText, TokenType.PlainText, highlighted);
				if(extendedText.Length == 0)
					continue;
				TryExtract(res, ref extendedText, "\n", TokenType.LineBreak, highlighted);
			}
			return res;
		}
		void ParseCSAttribute(List<CodeLexem> res, ref SourceString text, bool highlighted) {
			char? prevChar = text.PreviousSymbol(2);
			if(!prevChar.HasValue || prevChar.Value != ' ')
				return;
			List<int> indexes = new List<int>();
			indexes.Add(text.Length - 1);
			indexes.Add(text.IndexOf(']'));
			indexes.Add(text.IndexOf(','));
			indexes.Add(text.IndexOf('('));
			indexes.Add(text.IndexOf('\n'));
			indexes.RemoveAll(i => i <= 0);
			res.Add(new CodeLexem(TokenType.Attribute, StringCut(ref text, indexes.Min()), highlighted));
		}
		int lastLength = -1;
		void ParseCSSymbol(List<CodeLexem> res, ref SourceString text, TokenType lt, bool highlighted) {
			if(lastLength == -1 || lastLength != text.Length) {
				lastLength = text.Length;
				return;
			}
			CodeLexem cl = res.Count > 0 ? res.Last() : null;
			if(cl != null && cl.Type == TokenType.PlainText)
				cl.Text += StringCut(ref text, 1);
			else
				res.Add(new CodeLexem(TokenType.PlainText, StringCut(ref text, 1), highlighted));
		}
		void ParseCSKeyWord(List<CodeLexem> res, ref SourceString text, TokenType type, bool highlighted) {
			int index = -1;
			if(!CSEndOfTerm.Contains(previousSymbol)) return;
			foreach(string str in CSKeyWords) {
				if(text.StartsWith(str, StringComparison.Ordinal)) {
					if(!CSEndOfTerm.Contains(text[str.Length])) continue;
					index = str.Length;
					break;
				}
			}
			if(index < 0) return;
			res.Add(new CodeLexem(type, StringCut(ref text, index), highlighted));
		}
	}
	class XamlParser : BaseParser {
		char[] XamlEndOfTerm = { ' ', '	', '\n', '=', '/', '>', '<', '"', '{', '}', ',' };
		char[] XamlSymbol = { '=', '/', '>', '"', '{', '}', ',' };
		char[] XamlNamespaceDelimeter = { ':' };
		public XamlParser() { }
		protected bool IsInsideBlock = false;
		protected override List<CodeLexem> Parse(SourceString text, bool highlighted) {
			SourceString extendedText = text;
			List<CodeLexem> res = new List<CodeLexem>();
			while(extendedText.Length > 0) {
				if(TryExtract(res, ref extendedText, "<!--", TokenType.Comment, highlighted)) {
					TryExtractTo(res, ref extendedText, "-->", TokenType.Comment, null, highlighted);
				}
				if(extendedText.StartsWith("<", StringComparison.Ordinal)) IsInsideBlock = false;
				if(TryExtract(res, ref extendedText, "\"{}", TokenType.Value, highlighted))
					TryExtractTo(res, ref extendedText, "\"", TokenType.Value, null, highlighted);
				if(TryExtract(res, ref extendedText, "</", TokenType.Symbol, highlighted) ||
				   TryExtract(res, ref extendedText, "<", TokenType.Symbol, highlighted) ||
				   TryExtract(res, ref extendedText, "{", TokenType.Symbol, highlighted) ||
				   TryExtract(res, ref extendedText, "\"{", TokenType.Symbol, highlighted)) {
					ParseXamlKeyWord(res, ref extendedText, TokenType.Object, highlighted);
				}
				if(TryExtract(res, ref extendedText, "\"", TokenType.Value, highlighted)) {
					TryExtractTo(res, ref extendedText, "\"", TokenType.Value, null, highlighted);
				}
				ParseXamlKeyWord(res, ref extendedText, IsInsideBlock ? TokenType.Object : TokenType.Property, highlighted);
				TryExtract(res, ref extendedText, "}\"", TokenType.Symbol, highlighted);
				if(extendedText.StartsWith(">", StringComparison.Ordinal)) IsInsideBlock = true;
				ParseSymbol(res, ref extendedText, TokenType.Symbol, highlighted);
				TrySpace(res, ref extendedText, highlighted);
				TryExtract(res, ref extendedText, "\n", TokenType.LineBreak, highlighted);
			}
			return res;
		}
		void ParseSymbol(List<CodeLexem> res, ref SourceString text, TokenType lt, bool highlighted) {
			int index = text.IndexOfAny(XamlSymbol);
			if(index != 0) return;
			res.Add(new CodeLexem(TokenType.Symbol, text.Substring(0, 1), highlighted));
			text = text.Substring(1);
		}
		void ParseXamlKeyWord(List<CodeLexem> res, ref SourceString text, TokenType type, bool highlighted) {
			int index = text.IndexOfAny(XamlEndOfTerm);
			if(index <= 0) return;
			int nsIndex = text.IndexOf(':');
			if(nsIndex > 0 && nsIndex < index) {
				res.Add(new CodeLexem(type, StringCut(ref text, nsIndex), highlighted));
				res.Add(new CodeLexem(TokenType.Symbol, StringCut(ref text, 1), highlighted));
				res.Add(new CodeLexem(type, StringCut(ref text, index - nsIndex - 1), highlighted));
			} else {
				res.Add(new CodeLexem(type, StringCut(ref text, index), highlighted));
			}
		}
	}
	class VBParser : BaseParser {
		char[] VBEndOfTerm = { ' ', '\t', '\n', '=', '/', '>', '<', '"', '{', '}', ',', '(', ')', ';', ':', '\0', '?' };
		string[] VBKeyWords = { "attribute","addhandler","andalso","byte","catch","cdate","cint","const",
								"csgn","culgn","declare","directcast","else","enum","exit",
								"friend","getxmlnamespace","handles","in","is","like","mod",
								"mybase","new","noinheritable","on","or","overrides","property",
								"readonly","resume","set","single","string","then","try",
								"ulong","wend","with","addressof","as","byval",
								"cbool","cdbl","class","continue","cstr","cushort","default",
								"do","elseif","erase","false","function","global","if",
								"inherits","isnot","long","module","myclass","next",
								"notoverridable","operator","orelse","paramarray","protected","redim","return",
								"shadows","static","structure","throw","trycast","ushort","when",
								"withevents","alias","boolean","call","cbyte","cdec","clng","csbyte",
								"ctype","date","delegate","double","end","error","finally","get",
								"gosub","implements","integer","let","loop","mustinherit","namespace",
								"not","object","option","overloads","partial","public","rem",
								"sbyte","shared","step","sub","to","typeof","using","while", "async", "await",
								"writeonly","and","byref","case","cchar","char","cobj","cshort",
								"cuint","decimal","dim","each","endif","event","for","gettype","goto","imports",
								"interface","lib","me","mustoverride","narrowing","nothing","of","optional",
								"overridable","private","raiseevent","removehandler","select","short","stop",
								"synclock","true","uinteger","variant","widening","xor" };
		public VBParser() : base(false) { }
		protected override List<CodeLexem> Parse(SourceString text, bool highlighted) {
			SourceString extendedText = text;
			List<CodeLexem> res = new List<CodeLexem>();
			while(extendedText.Length > 0) {
				if(TryExtract(res, ref extendedText, "'", TokenType.Comment, highlighted)) {
					TryExtractTo(res, ref extendedText, "\n", TokenType.Comment, null, highlighted);
				}
				if(TryExtract(res, ref extendedText, "`", TokenType.Comment, highlighted)) {
					TryExtractTo(res, ref extendedText, "\n", TokenType.Comment, null, highlighted);
				}
				TryExtract(res, ref extendedText, "rem\n", TokenType.Comment, highlighted);
				if(TryExtract(res, ref extendedText, "rem ", TokenType.Comment, highlighted)) {
					TryExtractTo(res, ref extendedText, "\n", TokenType.Comment, null, highlighted);
				}
				if(TryExtract(res, ref extendedText, "rem\t", TokenType.Comment, highlighted)) {
					TryExtractTo(res, ref extendedText, "\n", TokenType.Comment, null, highlighted);
				}
				if(TryExtract(res, ref extendedText, "\"", TokenType.String, highlighted)) {
					TryExtractTo(res, ref extendedText, "\"", TokenType.String, "\"\"", highlighted);
					TryExtract(res, ref extendedText, "c", TokenType.String, highlighted);
				}
				ParseVBKeyWord(res, ref extendedText, TokenType.Keyword, highlighted);
				ParseVBSymbol(res, ref extendedText, TokenType.PlainText, highlighted);
				if(extendedText.Length == 0)
					continue;
				TrySpace(res, ref extendedText, highlighted);
				TryExtract(res, ref extendedText, "\n", TokenType.LineBreak, highlighted);
			}
			return res;
		}
		int lastLength = -1;
		void ParseVBSymbol(List<CodeLexem> res, ref SourceString text, TokenType lt, bool highlighted) {
			if(lastLength == -1 || lastLength != text.Length) {
				lastLength = text.Length;
				return;
			}
			CodeLexem cl = res.Count > 0 ? res.Last() : null;
			if(cl != null && cl.Type == TokenType.PlainText)
				cl.Text += StringCut(ref text, 1);
			else
				res.Add(new CodeLexem(TokenType.PlainText, StringCut(ref text, 1), highlighted));
		}
		void ParseVBKeyWord(List<CodeLexem> res, ref SourceString text, TokenType type, bool highlighted) {
			int index = -1;
			if(!VBEndOfTerm.Contains(previousSymbol)) return;
			foreach(string str in VBKeyWords) {
				if(text.StartsWith(str, StringComparison.OrdinalIgnoreCase)) {
					if(!VBEndOfTerm.Contains(text[str.Length])) continue;
					index = str.Length;
					break;
				}
			}
			if(index < 0) return;
			res.Add(new CodeLexem(type, StringCut(ref text, index), highlighted));
		}
	}
}

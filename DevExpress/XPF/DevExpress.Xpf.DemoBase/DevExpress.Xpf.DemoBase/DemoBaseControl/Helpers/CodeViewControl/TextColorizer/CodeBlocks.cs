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
using System.Windows.Controls;
using System.Windows.Documents;
using DevExpress.Mvvm.Native;
using DevExpress.Utils;
namespace DevExpress.Xpf.DemoBase.Helpers.TextColorizer.Internal {
	abstract class CodeBlockBase {
		protected CodeBlockSettings Settings;
		protected readonly bool Highlighted;
		public readonly Span Inline = new Span();
		protected readonly bool IsDark;
		public CodeBlockBase(CodeBlockSettings settings, bool highlighted, bool isDark) {
			Settings = settings;
			Highlighted = highlighted;
			IsDark = isDark;
		}
		public abstract string GetExpandedText();
		public abstract void UpdateInline();
	}
	class SimpleCodeBlock : CodeBlockBase {
		public readonly string Text;
		protected readonly List<CodeLexem> CodeLines;
		public SimpleCodeBlock(CodeBlockSettings settings, string text, bool highlighted, bool isDark) : base(settings, highlighted, isDark) {
			Text = text;
			var lex = new CodeLexem(text.Replace("\r", ""), Highlighted);
			CodeLines = lex.Parse(Settings.CodeLanguage, Highlighted);
		}
		public override string GetExpandedText() => Text;
		public override void UpdateInline() {
			Inline.Inlines.Clear();
			Inline.Inlines.AddRange(CodeLines.Select(x => x.ToInline(IsDark)));
		}
	}
	class HintCodeBlock : SimpleCodeBlock {
		public HintCodeBlock(CodeBlockSettings settings, string text)
			: base(settings, DecreaseTextIndents(text), true, false) {
			TextBlock tb = new TextBlock();
			tb.Inlines.AddRange(CodeLines.Select(x => x.ToInline(false)));
			Run lastInline = tb.Inlines.LastInline as Run;
			if(lastInline != null && String.IsNullOrWhiteSpace(lastInline.Text))
				tb.Inlines.Remove(lastInline);
			HintContent = tb;
		}
		static string DecreaseTextIndents(string text) {
			if(String.IsNullOrEmpty(text))
				return text;
			string[] lines = text.Split('\n');
			if(lines.Length == 1)
				return text;
			string firstLine = lines[0];
			int nonSpaceCharPos = 0;
			for(int ch = 0; ch < firstLine.Length; ch++) {
				if(firstLine[ch] != ' ')
					break;
				nonSpaceCharPos = ch;
			}
			string result = String.Empty;
			foreach(string str in lines) {
				result += (result == String.Empty ? String.Empty : "\n") + str.Remove(0, str.Length > nonSpaceCharPos ? nonSpaceCharPos : str.Length);
			}
			return result;
		}
		public TextBlock HintContent { get; private set; }
	}
	abstract class HierarchyCodeBlock : CodeBlockBase {
		protected List<CodeBlockBase> CodeBlocks;
		const int UsingMinLinesCount = 2;
		public HierarchyCodeBlock(CodeBlockSettings settings, bool hightighted, bool isDark) : base(settings, hightighted, isDark) {
			Text = String.Empty;
			HintText = String.Empty;
		}
		public string Text { get; protected set; }
		string HintText { get; set; }
		HintCodeBlock HintCodeBlock;
		public object GetHintContent() {
			if(HintCodeBlock == null)
				HintCodeBlock = new HintCodeBlock(Settings, HintText);
			return HintCodeBlock.HintContent;
		}
		protected void PopulateInnerCodeBlocks() {
			CodeBlocks = new List<CodeBlockBase>();
			string text = Text.Replace("\r", "");
			string[] lines = text.Split('\n');
			int lineIndex = CreateXmlns(lines);
			PopulateRegions(lines, lineIndex);
		}
		int CreateXmlns(string[] lines) {
			if(!Settings.DetextUsings)
				return 0;
			SimpleCodeBlock beforeUsingCodeBlock = null;
			List<string> usingCodeBlockLines = null;
			string usingFirstLineText = null;
			string processedText = string.Empty;
			int usingLinesCount = 0;
			for(int lineIndex = 0; lineIndex < lines.Length; ++lineIndex) {
				string line = lines[lineIndex];
				if(Settings.IsUsing(line)) {
					if(usingCodeBlockLines == null) {
						if(!string.IsNullOrEmpty(processedText))
							beforeUsingCodeBlock = new SimpleCodeBlock(Settings, processedText + Environment.NewLine, Highlighted, IsDark);
						processedText = string.Empty;
						usingFirstLineText = line;
						usingCodeBlockLines = new List<string>();
					}
					usingCodeBlockLines.Add(line);
					++usingLinesCount;
					continue;
				}
				if(usingCodeBlockLines == null) {
					processedText = string.IsNullOrEmpty(processedText) ? line : processedText + Environment.NewLine + line;
					continue;
				}
				if(usingLinesCount > 0 && usingLinesCount <= UsingMinLinesCount) {
					processedText = string.Empty;
					if(beforeUsingCodeBlock != null)
						processedText = beforeUsingCodeBlock.Text;
					processedText += usingFirstLineText;
					if(usingLinesCount > 1)
						processedText += Environment.NewLine;
					CodeBlocks.Add(new SimpleCodeBlock(Settings, processedText, Highlighted, IsDark));
				} else {
					if(beforeUsingCodeBlock != null)
						CodeBlocks.Add(beforeUsingCodeBlock);
					var usingCollapsed = Settings.UsingCollapsed(usingCodeBlockLines);
					var usingCodeBlock = new UsingsCodeBlock(Settings, usingCollapsed.Item1, usingCollapsed.Item2, usingFirstLineText, Highlighted, IsDark);
					var firstHintLine = usingCodeBlockLines.FirstOrDefault();
					if(firstHintLine != null)
						usingCodeBlock.AppendHintLine(firstHintLine, TextLineType.Default);
					foreach(var usingCodeBlockLine in usingCodeBlockLines.Skip(1))
						usingCodeBlock.AppendTextLine(usingCodeBlockLine, TextLineType.Default);
					CodeBlocks.Add(usingCodeBlock);
					usingCodeBlock.PopulateInnerCodeBlocks();
				}
				return lineIndex;
			}
			return 0;
		}
		HierarchyCodeBlock CreateRegionCodeBlock(string line, int regionStartLeft) {
			if(Settings.IsHighlightedRegionStartLeft(line))
				return new HighlightedCodeBlock(Settings, IsDark);
			var collapsedText = Settings.GetCollapsedText(line).TrimEnd();
			return new RegionCodeBlock(Settings, collapsedText.Remove(regionStartLeft), collapsedText.Substring(regionStartLeft), line, Highlighted, IsDark);
		}
		void PopulateRegions(string[] lines, int startIndex) {
			HierarchyCodeBlock regionCodeLine = null;
			string processedText = null;
			int regionStartCount = 0;
			for(int lineIndex = startIndex; lineIndex < lines.Length; lineIndex++) {
				string line = lines[lineIndex];
				if(regionCodeLine == null) {
					var regionStartLeft = Settings.RegionStartLeft(line);
					if(regionStartLeft >= 0) {
						regionStartCount = 1;
						if(processedText != null) {
							CodeBlocks.Add(new SimpleCodeBlock(Settings, processedText + "\r\n", Highlighted, IsDark));
							processedText = null;
						}
						regionCodeLine = CreateRegionCodeBlock(line, regionStartLeft);
						CodeBlocks.Add(regionCodeLine);
						continue;
					}
					processedText = processedText == null ? line : processedText + "\r\n" + line;
					continue;
				}
				if(Settings.RegionStartLeft(line) >= 0) {
					regionCodeLine.AppendTextLine(line, TextLineType.RegionStart);
					regionStartCount++;
					continue;
				}
				if(Settings.IsRegionEnd(line)) {
					regionCodeLine.AppendTextLine(line, TextLineType.RegionEnd);
					if(--regionStartCount != 0)
						continue;
					regionCodeLine.PopulateInnerCodeBlocks();
					regionCodeLine = null;
					continue;
				}
				regionCodeLine.AppendTextLine(line, TextLineType.Default);
			}
			if(processedText != null)
				CodeBlocks.Add(new SimpleCodeBlock(Settings, processedText, Highlighted, IsDark));
		}
		public List<ExpandableHierarchyCodeBlock> GetRegions() {
			var result = new List<ExpandableHierarchyCodeBlock>();
			if(CodeBlocks == null)
				return result;
			foreach(var codeBlock in CodeBlocks) {
				HierarchyCodeBlock hierarchyCodeLine = codeBlock as HierarchyCodeBlock;
				if(hierarchyCodeLine == null)
					continue;
				ExpandableHierarchyCodeBlock expandableHierarchyCodeLine = hierarchyCodeLine as ExpandableHierarchyCodeBlock;
				if(expandableHierarchyCodeLine != null) {
					result.Add(expandableHierarchyCodeLine);
					if(!expandableHierarchyCodeLine.Expanded)
						continue;
				}
				result.AddRange(hierarchyCodeLine.GetRegions());
			}
			return result;
		}
		protected virtual void AppendTextLine(string text, TextLineType textLineType) {
			Text += text + "\n";
			AppendHintLine(text, textLineType);
		}
		void AppendHintLine(string text, TextLineType textLineType) {
			if(textLineType == TextLineType.Default)
				HintText += (HintText == String.Empty ? String.Empty : "\n") + text;
		}
		protected string GetInnerExpandedText() {
			return string.Join("\r\n", CodeBlocks.Select(x => x.GetExpandedText()).Where(x => !string.IsNullOrEmpty(x)));
		}
		protected List<Inline> UpdateAndGetInnerInlines() {
			var res = new List<Inline>();
			foreach(var codeLine in CodeBlocks) {
				codeLine.UpdateInline();
				res.Add(codeLine.Inline);
			}
			return res;
		}
	}
	abstract class ExpandableHierarchyCodeBlock : HierarchyCodeBlock {
		readonly string CollapsedTextPrefix;
		readonly string CollapsedText;
		readonly string ExpandedFirstText;
		public ExpandableHierarchyCodeBlock(CodeBlockSettings settings, string collapsedTextPrefix, string collapsedText, string expandedFirstText, bool highlighted, bool isDark)
			: base(settings, highlighted, isDark) {
			CollapsedTextPrefix = collapsedTextPrefix;
			CollapsedText = collapsedText;
			ExpandedFirstText = expandedFirstText;
		}
		public string FirstLineText { get; private set; }
		bool expanded;
		public bool Expanded {
			get { return expanded; }
			set {
				if(expanded == value) return;
				expanded = value;
				UpdateInline();
			}
		}
		Inline collapsedTextStart, collapsedTextEnd;
		public SLTextRange GetCollapsedTextRange() => collapsedTextStart == null ? null : new SLTextRange(collapsedTextStart.ContentStart, collapsedTextEnd.ContentEnd); public override string GetExpandedText() => ExpandedFirstText + "\n" + GetInnerExpandedText();
		public override void UpdateInline() {
			FirstLineText = (Expanded ? ExpandedFirstText : CollapsedTextPrefix + CollapsedText) + "\n";
			Inline.Inlines.Clear();
			if(Expanded) {
				collapsedTextStart = collapsedTextEnd = null;
				Inline.Inlines.AddRange(new CodeLexem(ExpandedFirstText.Replace("\r", "") + "\n", Highlighted).Parse(Settings.CodeLanguage, Highlighted).Select(x => x.ToInline(IsDark)));
				Inline.Inlines.AddRange(UpdateAndGetInnerInlines());
			} else {
				var collapsedPrefixCodeLexem = new CodeLexem(CollapsedTextPrefix.Replace("\r", ""), Highlighted).Parse(Settings.CodeLanguage, Highlighted);
				Inline.Inlines.AddRange(collapsedPrefixCodeLexem.Select(x => x.ToInline(IsDark)));
				var collapsedRegionCodeLexem = new CodeLexem(CollapsedText.Replace("\r", ""), Highlighted).Parse(Settings.CodeLanguage, Highlighted);
				collapsedRegionCodeLexem.ForEach(lexem => lexem.Type = TokenType.CollapsedRegion);
				var collapsedTextInlines = collapsedRegionCodeLexem.Select(x => x.ToInline(IsDark)).ToList();
				collapsedTextStart = collapsedTextInlines.Count == 0 ? null : collapsedTextInlines[0];
				collapsedTextEnd = collapsedTextInlines.Count == 0 ? null : collapsedTextInlines[collapsedTextInlines.Count - 1];
				Inline.Inlines.AddRange(collapsedTextInlines);
				Inline.Inlines.Add(new CodeLexem(TokenType.LineBreak, "\n", Highlighted).ToInline(IsDark));
			}
		}
	}
	class HighlightedCodeBlock : HierarchyCodeBlock {
		public HighlightedCodeBlock(CodeBlockSettings settings, bool isDark) : base(settings, true, isDark) {
		}
		public override void UpdateInline() {
			Inline.Inlines.Clear();
			Inline.Inlines.AddRange(UpdateAndGetInnerInlines());
		}
		public override string GetExpandedText() => GetInnerExpandedText();
		int regionStartLineCount = 0;
		protected override void AppendTextLine(string text, TextLineType textLineType) {
			if(textLineType == TextLineType.RegionStart)
				regionStartLineCount++;
			if(textLineType == TextLineType.RegionEnd)
				regionStartLineCount--;
			if(regionStartLineCount == -1)
				return;
			base.AppendTextLine(text, textLineType);
		}
	}
	class RegionCodeBlock : ExpandableHierarchyCodeBlock {
		public RegionCodeBlock(CodeBlockSettings settings, string collapsedTextPrefix, string collapsedText, string firstLineText, bool highlighted, bool isDark)
			: base(CodeBlockSettings.Create(settings.CodeLanguage, true), collapsedTextPrefix, collapsedText, firstLineText, highlighted, isDark) {
		}
	}
	class UsingsCodeBlock : ExpandableHierarchyCodeBlock {
		public UsingsCodeBlock(CodeBlockSettings settings, string collapsedTextPrefix, string collapsedText, string expandedFirstText, bool highlighted, bool isDark)
			: base(CodeBlockSettings.Create(settings.CodeLanguage, true), collapsedTextPrefix, collapsedText, expandedFirstText, highlighted, isDark) {
		}
	}
	sealed class RootCodeBlock : HierarchyCodeBlock {
		public RootCodeBlock(CodeLanguage language, string text, bool isDark) : this(CodeBlockSettings.Create(language), text, isDark) { }
		RootCodeBlock(CodeBlockSettings settings, string text, bool isDark) : base(settings, !settings.IsHighlightedRegionStartLeft(text), isDark) {
			Text = text;
			if(!string.IsNullOrWhiteSpace(Text))
				PopulateInnerCodeBlocks();
		}
		public override void UpdateInline() {
			Inline.Inlines.Clear();
			Inline.Inlines.AddRange(UpdateAndGetInnerInlines());
		}
		public override string GetExpandedText() => GetInnerExpandedText();
	}
	sealed class CodeBlockSettings {
		public readonly CodeLanguage CodeLanguage;
		CodeBlockSettings(CodeLanguage codeLanguage, bool detextUsings, string regionStartLeftString, string highlightedRegionStartLeftString,
			string regionStartRightString, string regionEndString, Func<string, int> findUsingInLine, Func<IList<string>, string> usingCollapsedString) {
			Guard.ArgumentNotNull(findUsingInLine, "findUsingInLine");
			DetextUsings = detextUsings;
			RegionStartLeftString = regionStartLeftString;
			HighlightedRegionStartLeftString = highlightedRegionStartLeftString;
			RegionStartRightString = regionStartRightString;
			RegionEndString = regionEndString;
			FindUsingInLine = findUsingInLine;
			UsingCollapsedString = usingCollapsedString;
			CodeLanguage = codeLanguage;
		}
		public readonly bool DetextUsings;
		readonly string RegionStartLeftString;
		readonly string HighlightedRegionStartLeftString;
		readonly string RegionStartRightString;
		readonly string RegionEndString;
		readonly Func<string, int> FindUsingInLine;
		readonly Func<IList<string>, string> UsingCollapsedString;
		public bool IsHighlightedRegionStartLeft(string line) {
			return !string.IsNullOrEmpty(HighlightedRegionStartLeftString) && line.Contains(HighlightedRegionStartLeftString);
		}
		public int RegionStartLeft(string line) {
			return string.IsNullOrEmpty(RegionStartLeftString) ? -1 : line.IndexOf(RegionStartLeftString);
		}
		public bool IsRegionEnd(string line) {
			return !string.IsNullOrEmpty(RegionEndString) && line.Contains(RegionEndString);
		}
		public bool IsUsing(string line) => FindUsingInLine(line) >= 0;
		public string GetCollapsedText(string firstLineText) {
			if(string.IsNullOrEmpty(RegionStartLeftString)) return firstLineText;
			firstLineText = firstLineText.Replace(RegionStartLeftString, string.Empty);
			if(string.IsNullOrEmpty(RegionStartRightString)) return firstLineText;
			firstLineText = firstLineText.Replace(RegionStartRightString, string.Empty);
			return firstLineText;
		}
		public Tuple<string, string> UsingCollapsed(IList<string> lines) {
			if(UsingCollapsedString == null)
				throw new InvalidOperationException();
			var i = FindUsingInLine(lines[0]);
			if(i < 0)
				throw new InvalidOperationException();
			return Tuple.Create(lines[0].Remove(i), UsingCollapsedString(lines));
		}
		public static CodeBlockSettings Create(CodeLanguage codeLanguage, bool forceSkipDetextUsings = false) {
			switch(codeLanguage) {
			case CodeLanguage.XAML:
				return new CodeBlockSettings(
					detextUsings: !forceSkipDetextUsings,
					regionStartLeftString: "<!--#region ",
					highlightedRegionStartLeftString: "<!--#region !-->",
					regionStartRightString: "-->",
					regionEndString: "<!--#endregion",
					findUsingInLine: line => {
						var xmlns = line.IndexOf("xmlns");
						return xmlns >= 0 ? xmlns : line.IndexOf("mc:Ignorable=");
					},
					usingCollapsedString: lines => "XAML namespaces" + (lines.Last().Contains('>') ? ">" : string.Empty),
					codeLanguage: codeLanguage
				);
			case CodeLanguage.CS:
				return new CodeBlockSettings(
					detextUsings: !forceSkipDetextUsings,
					regionStartLeftString: "#region ",
					highlightedRegionStartLeftString: "#region !",
					regionStartRightString: string.Empty,
					regionEndString: "#endregion",
					findUsingInLine: line => line.IndexOf("using "),
					usingCollapsedString: _ => "usings",
					codeLanguage: codeLanguage
				);
			case CodeLanguage.VB:
				return new CodeBlockSettings(
					detextUsings: !forceSkipDetextUsings,
					regionStartLeftString: "#Region ",
					highlightedRegionStartLeftString: "#Region \"!",
					regionStartRightString: string.Empty,
					regionEndString: "#End Region",
					findUsingInLine: line => line.IndexOf("Imports "),
					usingCollapsedString: _ => "Imports",
					codeLanguage: codeLanguage
				);
			case CodeLanguage.Plain:
				return new CodeBlockSettings(
					detextUsings: false,
					regionStartLeftString: string.Empty,
					highlightedRegionStartLeftString: string.Empty,
					regionStartRightString: string.Empty,
					regionEndString: string.Empty,
					findUsingInLine: _ => -1,
					usingCollapsedString: null,
					codeLanguage: codeLanguage
				);
			default: throw new InvalidOperationException();
			}
		}
	}
	public enum TextLineType { Default, RegionStart, RegionEnd }
}

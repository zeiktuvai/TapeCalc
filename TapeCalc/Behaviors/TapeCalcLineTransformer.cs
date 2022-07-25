using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TapeCalc.Behaviors
{
    public class TapeCalcLineTransformer : DocumentColorizingTransformer
    {
        public const string _FindLetterRegex = @"[A-Za-z]";
        protected override void ColorizeLine(DocumentLine line)
        {
            string lineText = CurrentContext.Document.GetText(line);

            if (Regex.IsMatch(lineText, _FindLetterRegex))
            {
                int indexOfFirstLetter = Regex.Match(lineText, _FindLetterRegex).Index;
                int indexOfLastLetter = lineText.Length - indexOfFirstLetter;

                ChangeLinePart(line.Offset + indexOfFirstLetter, line.Offset + indexOfFirstLetter + indexOfLastLetter, vl =>
                    vl.TextRunProperties.ForegroundBrush = Brush.Parse("Green"));
            }       
            
            if (Regex.IsMatch(lineText, @"[-]"))
            {
                int indexOfFirstNumber = 0;
                int indexOfLastNumbner = Regex.IsMatch(lineText, _FindLetterRegex) ?
                    Regex.Match(lineText.Substring(0, Regex.Match(lineText, _FindLetterRegex).Index), @"[0-9]", RegexOptions.RightToLeft).Index + 1:
                    line.Length;

                ChangeLinePart(line.Offset + indexOfFirstNumber, line.Offset + indexOfFirstNumber + indexOfLastNumbner, vl =>
                    vl.TextRunProperties.ForegroundBrush = Brush.Parse("Red"));
            }
        }
    }
}


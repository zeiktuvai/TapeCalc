using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TapeCalc.Models;
using AvaloniaEdit.Document;

namespace TapeCalc.Helpers
{
    public class TextProcessor
    {
        //TODO: Replace this later with settings screen
        public static int decPlace { get; set; } = 2;
        public static List<CalculatorLineItemModel> ConvertTextEditorText(TextDocument doc)
        {            
            var _returnList = new List<CalculatorLineItemModel>();
            bool _sameBlock = true;

            foreach (var line in doc.Lines)
            {
                var LineText = doc.GetText(line);
                var previousLine = _returnList.LastOrDefault();

                if (Regex.IsMatch(LineText, "[\r\n]") || string.IsNullOrEmpty(LineText))
                {
                    if (_sameBlock == true)
                    {
                        _sameBlock = false;
                        _returnList.Add(new CalculatorLineItemModel { LineItemType = Enums.LineItemTypeEnum.EndOfBlock });
                    }
                    
                    _returnList.Add(new CalculatorLineItemModel { LineItemType = Enums.LineItemTypeEnum.BlankLine});
                    continue;
                }

                if (Regex.IsMatch(LineText.Substring(0, 1), @"[A-Za-z]"))
                {
                    _returnList.Add(new CalculatorLineItemModel
                    {
                        LineItemType = Enums.LineItemTypeEnum.TextLine,
                        Comment = LineText
                    });
                    continue;
                }

                if (Regex.IsMatch(LineText, "[―]"))
                {
                    _returnList.Add(new CalculatorLineItemModel
                    {
                        LineItemType = Enums.LineItemTypeEnum.Separator,
                        Comment = "―"
                    });
                    continue;
                }

                if (Regex.IsMatch(LineText, "[=]"))
                {
                    _returnList.Add(new CalculatorLineItemModel
                    {
                        LineItemType = Enums.LineItemTypeEnum.TotalLine,
                        Operation = "=",
                        Operand = GetOperand(LineText.Trim(), decPlace),
                        Comment = GetComment(LineText.Trim())
                    });

                    _sameBlock = false;                    
                    continue;
                }

                if ((!Regex.IsMatch(LineText.Substring(0, 1), "[-+/*]")) || line.Length > 1)
                {                    
                    if (Regex.IsMatch(LineText, "[0-9]") && LineText.Length > 1)
                    {
                        if (Regex.IsMatch(LineText.Substring(0, 1), "[-+/*a-zA-Z0-9]"))
                        {
                            var newLineItem = new CalculatorLineItemModel()
                            {
                                LineItemType = Enums.LineItemTypeEnum.LineItem,
                                Operation = GetOperator(LineText.Trim()),
                                Operand = GetOperand(LineText.Trim(), decPlace),
                                Comment = GetComment(LineText.Trim())
                            };

                            if (previousLine != null && previousLine.LineItemType == Enums.LineItemTypeEnum.TotalLine)
                            {
                                _sameBlock = true;
                            }
                                                        
                                _returnList.Add(newLineItem);
                            
                        }
                    }
                }
            }

            _returnList.Add(new CalculatorLineItemModel { LineItemType = Enums.LineItemTypeEnum.EndOfLine });
            return _returnList;
        }

        public static TextDocument ConvertTextEditorText(List<CalculatorLineItemModel> ProcessList)
        {
            var _processList = ProcessList;
            var _returnString = new StringBuilder();
            string _decimalFormat = string.Format("n{0}", decPlace);

            foreach (var item in _processList)
            {
                switch (item.LineItemType)
                {
                    case Enums.LineItemTypeEnum.LineItem:
                        _returnString.AppendFormat(string.Format("{0}{1,20} {2}", item.Operation, ((decimal)item.Operand).ToString(_decimalFormat), item.Comment));
                        _returnString.AppendLine();
                        break;

                    case Enums.LineItemTypeEnum.TotalLine:
                        _returnString.AppendFormat(string.Format("{0}{1,20} {2}", item.Operation, ((decimal)item.Operand).ToString(_decimalFormat), item.Comment));
                        _returnString.AppendLine();
                        break;

                    case Enums.LineItemTypeEnum.Separator:
                        _returnString.Append("―――――――――――――――");
                        _returnString.AppendLine();
                        break;

                    case Enums.LineItemTypeEnum.BlankLine:
                        _returnString.AppendLine();
                        break;

                    case Enums.LineItemTypeEnum.TextLine:
                        _returnString.Append(item.Comment);
                        _returnString.AppendLine();
                        break;

                    case Enums.LineItemTypeEnum.EndOfLine:
                        break;
                }
            }
            
            TextDocument text = new TextDocument(_returnString.ToString());
            

            return new TextDocument(_returnString.ToString().ToCharArray());
        }

        private static string GetOperator(string line)
        {
            if (Regex.IsMatch(line, "[-+/*]"))
            {
                return line.Substring(line.IndexOfAny(new char[] { '+', '-', '*', '/' }), 1);
            }
            else
            {
                return "+";
            }
        }

        private static decimal GetOperand(string line, int decimalPlace)
        {
            decimal _operand;

            if (Regex.IsMatch(line, "[A-Za-z]"))
            {
                var substring = line.Substring(1).Trim();
                var regex = new Regex(@"[0-9]+(\.[0-9]+)?");
                _operand = decimal.Parse(regex.Match(substring).Value);
            }
            else
            {
                _operand = decimal.Parse(line.Substring(Regex.Match(line, "[0-9]").Index));
            }

            if (!Regex.IsMatch(_operand.ToString(), @"\."))
            {
                _operand = decimal.Parse(_operand.ToString(String.Format("F{0}", decimalPlace)));
            }

            return _operand;
        }

        private static string GetComment(string line)
        {
            if (Regex.IsMatch(line, "[A-Za-z]"))
            {
                var substring = line.Substring(1).Trim();
                var regex = new Regex(@"[0-9]+(\.[0-9]+)?");
                return regex.Replace(substring, "").Trim();                
            }
            else
            {
                return "";
            }
        }
    }
}

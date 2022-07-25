﻿using System;
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
        public static List<CalculatorLineItemModel> ConvertTextEditorText(TextDocument doc)
        {            
            var _returnList = new List<CalculatorLineItemModel>();

            foreach (var line in doc.Lines)
            {
                var LineText = doc.GetText(line);

                if (Regex.IsMatch(LineText, "[\r\n]") || string.IsNullOrEmpty(LineText))
                {
                    _returnList.Add(new CalculatorLineItemModel
                    {
                        LineItemType = Enums.LineItemTypeEnum.BlankLine
                    });
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
                        Operand = GetOperand(LineText.Trim()),
                        Comment = GetComment(LineText.Trim())
                    });
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
                                Operand = GetOperand(LineText.Trim()),
                                Comment = GetComment(LineText.Trim())
                            };
                            _returnList.Add(newLineItem);
                        }
                    }
                }
            }

            _returnList.Add(new CalculatorLineItemModel { LineItemType = Enums.LineItemTypeEnum.EndOfLine });
            return _returnList;
        }

        public static string ConvertTextEditorText(List<CalculatorLineItemModel> ProcessList)
        {
            var _processList = ProcessList;
            var _returnString = string.Empty;

            foreach (var item in _processList)
            {
                switch (item.LineItemType)
                {
                    case Enums.LineItemTypeEnum.LineItem:
                        _returnString += string.Format("{0} {1,20} {2}", item.Operation, item.Operand.ToString(), item.Comment);
                        _returnString += "\r\n";
                        break;

                    case Enums.LineItemTypeEnum.TotalLine:
                        _returnString += string.Format("{0} {1,20} {2}", item.Operation, item.Operand.ToString(), item.Comment);
                        _returnString += "\r\n";
                        break;

                    case Enums.LineItemTypeEnum.Separator:
                        _returnString += "――――――――――\r\n";
                        break;

                    case Enums.LineItemTypeEnum.BlankLine:
                        _returnString += "\r\n";
                        break;

                    case Enums.LineItemTypeEnum.TextLine:
                        _returnString += item.Comment;
                        _returnString += "\r\n";
                        break;

                    case Enums.LineItemTypeEnum.EndOfLine:
                        break;
                }
            }
            return _returnString;
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

        private static decimal GetOperand(string line)
        {
            if (Regex.IsMatch(line, "[A-Za-z]"))
            {
                var substring = line.Substring(1).Trim();
                var regex = new Regex(@"[0-9]+(\.[0-9]+)?");
                return decimal.Parse(regex.Match(substring).Value);

            }
            else
            {
                return decimal.Parse(line.Substring(Regex.Match(line, "[0-9]").Index));
            }
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

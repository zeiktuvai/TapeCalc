using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using TapeCalc.Models;


namespace TapeCalc.Views
{
    public partial class MainWindow : Window
    {
        public int position { get; set; } = 0;
        public MainWindow()
        {
            InitializeComponent();     
           
        }

        //private void KeyPress(object sender, KeyEventArgs e)
        //{
        //    var text = (sender as TextBox).Text;
        //    if (!String.IsNullOrEmpty(text))
        //    {
        //        if ((!Regex.IsMatch(text.Substring(0, 1), "[-+/*]")) || text.Length > 1)
        //        {
        //            if (Regex.IsMatch(text.Substring(0, 1), "[-+/*a-zA-Z0-9]"))
        //            {
        //                switch (e.Key)
        //                {
        //                    case Key.Add or Key.OemPlus:
        //                        text = text.Substring(0, text.Length - 1);
        //                        ProcessKeyStroke(text);
        //                        tbxCalcEntry.Text = "+";
        //                        tbxCalcEntry.CaretIndex = text.Length;
        //                        break;
        //                    case Key.Subtract or Key.OemMinus:
        //                        text = text.Substring(0, text.Length - 1);
        //                        ProcessKeyStroke(text);
        //                        tbxCalcEntry.Text = "-";
        //                        tbxCalcEntry.CaretIndex = text.Length;
        //                        break;
        //                    case Key.Enter:
        //                        position = 0;
        //                        ProcessKeyStroke(text, true);
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    if (e.Key == Key.Up && lbxCalcView.ItemCount != 0)
        //    {
        //        if (position == 0)
        //        {
        //            position = lbxCalcView.ItemCount;
        //        }
        //        position--;
        //        var items = (List<CalcultorLineItemModel>)lbxCalcView.Items;
        //        tbxCalcEntry.Text = items[position].Operand.ToString();
        //        //TODO: all this code
        //    }
        //}

        //private List<CalcultorLineItemModel> ProcessKeyStroke(string text, bool isEnter = false)
        //{
        //    List<CalcultorLineItemModel> items = new List<CalcultorLineItemModel>();
        //    if (lbxCalcView.Items != null)
        //    {
        //        items = (List<CalcultorLineItemModel>)lbxCalcView.Items;
        //    }

        //    var lineItem = new CalcultorLineItemModel();
        //    var content = text.Trim();

        //    lineItem.Id = Guid.NewGuid();
        //    lineItem.Operation = GetOperator(content);
        //    lineItem.Operand = GetOperand(content);
        //    lineItem.Comment = GetComment(content);

        //    items.Add(lineItem);

        //    lbxCalcView.Items = null;
        //    if (isEnter == true)
        //    {
        //        lbxCalcView.Items = CalculateValues(items);
        //    } 
        //    else
        //    {
        //        lbxCalcView.Items = items;
        //    }
        //    tbxCalcEntry.Text = "";

        //    return items;
        //}

        //private string GetOperator(string line)
        //{
        //    var content = line.Split("\r\n");
        //    if(Regex.IsMatch(content[0].ToString(), "[-+/*]"))
        //    {
        //        return content[0].Substring(content[0].IndexOfAny(new char[] { '+', '-', '*', '/' }), 1);
        //    }
        //    else
        //    {
        //        return "+";
        //    }
        //}

        //private decimal GetOperand(string line)
        //{
        //    if(Regex.IsMatch(line, "[A-Za-z]"))
        //    {
        //        return decimal.Parse(line.Substring(Regex.Match(line, "[0-9]").Index, 
        //                Regex.Match(line, "[A-Za-z]").Index - 1));
        //    }
        //    else
        //    {
        //        return decimal.Parse(line.Substring(Regex.Match(line, "[0-9]").Index));
        //    }
        //}

        //private string GetComment(string line)
        //{
        //    if (Regex.IsMatch(line,"[A-Za-z]"))
        //    {
        //        return line.Substring(Regex.Match(line, "[A-Za-z]").Index);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        //private List<CalcultorLineItemModel> CalculateValues(List<CalcultorLineItemModel> calcList)
        //{
        //    decimal calculation = 0;
        //    var count = 0;
        //    List<CalcultorLineItemModel> processedList = new List<CalcultorLineItemModel>();

        //    if (calcList.Count > 1)
        //    {
        //        foreach (var item in calcList)
        //        {
        //            //var op = item.Operation;
        //            //var num = item.Operand;
        //            //var comment = item.Comment;
        //            //var combined = op.ToString() + num.ToString() + comment.ToString();
        //            CalcultorLineItemModel newLine = new CalcultorLineItemModel();
        //            count++;

        //            //if (!Regex.IsMatch(combined, "[=]"))
        //            if (!(item.Id == Guid.Parse("7217c4bd-0b69-4bfb-82b7-d5d125c1093c") || item.Id == Guid.Parse("b80986f3-a524-406f-a39e-79f09c5d090c")))
        //            {
        //                newLine.Id = item.Id;
        //                newLine.Operation = item.Operation;
        //                newLine.Operand = item.Operand;
        //                newLine.Comment = item.Comment;

        //                switch (item.Operation)
        //                {
        //                    case "+":
        //                        calculation += item.Operand ?? 0;
        //                        break;
        //                    case "-":
        //                        calculation -= item.Operand ?? 0;
        //                        break;
        //                }
        //                processedList.Add(newLine);
        //            }
        //            if (item.Id == Guid.Parse("b80986f3-a524-406f-a39e-79f09c5d090c"))
        //            {
        //                processedList.Add(new CalcultorLineItemModel() { Id = Guid.Parse("7217c4bd-0b69-4bfb-82b7-d5d125c1093c"), Operation = "===", Comment = "=========" });
        //                processedList.Add(new CalcultorLineItemModel() { Id = Guid.Parse("b80986f3-a524-406f-a39e-79f09c5d090c"), Operation = "+", Operand = calculation, Comment = "" });
        //            }
        //            if (count == calcList.Count)
        //            {
        //                processedList.Add(new CalcultorLineItemModel() {Id = Guid.Parse("7217c4bd-0b69-4bfb-82b7-d5d125c1093c"), Operation = "===", Comment = "=========" });
        //                processedList.Add(new CalcultorLineItemModel() {Id = Guid.Parse("b80986f3-a524-406f-a39e-79f09c5d090c"), Operation = "+", Operand = calculation, Comment = ""});
        //                //calculation = 0;
        //                //count = 0;
        //            }
        //        }
        //        return processedList;
        //    } else
        //    {
        //        return calcList;
        //    }
        //}
    }
}
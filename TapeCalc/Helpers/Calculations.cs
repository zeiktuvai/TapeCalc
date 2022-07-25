using System.Collections.Generic;
using TapeCalc.Models;
using System.Text.RegularExpressions;
using TapeCalc.Enums;

namespace TapeCalc.Helpers
{
    public static class Calculations
    {
        public static List<CalculatorLineItemModel> CalculateValues(List<CalculatorLineItemModel> calcList)
        {
            decimal calculation = 0;
            int count = 0;
            List<CalculatorLineItemModel> processedList = new List<CalculatorLineItemModel>();

            if (calcList.Count > 1)
            {
                foreach (var item in calcList)
                {
                    CalculatorLineItemModel newLine = new CalculatorLineItemModel();

                    switch (item.LineItemType)
                    {
                        case LineItemTypeEnum.LineItem:
                            var test = calcList.IndexOf(item);
                            if (calcList.IndexOf(item) > 1)
                            {
                                var PreviousLine = calcList[calcList.IndexOf(item) - 1];
                                if (PreviousLine.LineItemType == LineItemTypeEnum.TotalLine)
                                {
                                    calculation = PreviousLine.Operand ?? 0;
                                    count++;
                                }
                            }


                            newLine.LineItemType = LineItemTypeEnum.LineItem;
                            newLine.Operation = item.Operation;
                            newLine.Operand = item.Operand;
                            newLine.Comment = item.Comment;
                            calculation = PerformCalculation(item.Operation, item.Operand ?? 0, calculation);
                            count++;
                            break;

                        case LineItemTypeEnum.EndOfLine:
                            if (calcList[calcList.FindIndex(li => li.LineItemType == LineItemTypeEnum.EndOfLine) - 1]
                               .LineItemType != LineItemTypeEnum.BlankLine && count > 1)
                            {                                                                
                                processedList.Add(new CalculatorLineItemModel { LineItemType = LineItemTypeEnum.Separator });
                                newLine.Operation = "=";
                                newLine.Operand = calculation;
                                newLine.Comment = "";
                                newLine.LineItemType = LineItemTypeEnum.TotalLine;
                                calculation = 0;                                
                            } 
                            else
                            {
                                return processedList;
                            }
                            break;

                        case LineItemTypeEnum.Separator:
                            newLine.LineItemType = LineItemTypeEnum.Separator;
                            break;

                        case LineItemTypeEnum.TotalLine:
                            newLine.Operation = "=";
                            newLine.Operand = calculation;
                            newLine.Comment = item.Comment;
                            newLine.LineItemType = LineItemTypeEnum.TotalLine;
                            calculation = 0;
                            count = 0;
                            break;

                        case LineItemTypeEnum.BlankLine:
                            newLine.LineItemType = LineItemTypeEnum.BlankLine;
                            break;
                        
                        case LineItemTypeEnum.TextLine:
                            newLine.LineItemType = LineItemTypeEnum.TextLine;
                            newLine.Comment = item.Comment;
                            break;
                    }

                    processedList.Add(newLine);        
                }
                return processedList;
            }
            else
            {
                return calcList;
            }
        }

        private static decimal PerformCalculation(string operation, decimal operand, decimal runningCalc)
        {
            decimal calc = runningCalc;

            switch (operation)
            {
                case "+":
                    calc += operand;
                    break;
                case "-":
                    calc -= operand;
                    break;
                case "/":
                    calc /= operand;
                    break;
                case "*":
                    calc *= operand;
                    break;
            }
            return calc;
        }
    }
}

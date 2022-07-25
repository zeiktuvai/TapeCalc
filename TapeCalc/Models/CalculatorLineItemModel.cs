using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TapeCalc.Enums;

namespace TapeCalc.Models
{
    public class CalculatorLineItemModel
    {
        public LineItemTypeEnum LineItemType { get; set; } = 0;
        public string Operation { get; set; }
        public decimal? Operand { get; set; }
        public string Comment { get; set; }
    }
}

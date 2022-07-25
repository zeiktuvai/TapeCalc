using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapeCalc.Models
{
    public class CalculatorPageModel
    {
        public string Name { get; set; } = "Untitled";

        public List<CalculatorLineItemModel> LineItems { get; set; }
    }
}

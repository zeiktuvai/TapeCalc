using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapeCalc.Enums
{
    public enum LineItemTypeEnum
    {
        LineItem = 0,
        TotalLine = 1,
        Separator = 2,
        BlankLine = 3,
        EndOfLine = 4,
        TextLine = 5,
        EndOfBlock = 6
    }
}

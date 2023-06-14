using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NPexwLib
{
    public class Cell
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Value = 0;
        public Cell(int x, int y, int value) 
        {
            X = x;
            Y = y;
            Value = value;
        }
    }
}

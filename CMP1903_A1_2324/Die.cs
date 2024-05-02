using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1903_A1_2324
{
    internal class Die
    {
        private static readonly Random random = new Random();

        private int _dieValue { get; set; } //Property to hold the current die value

        public int RollDie()
        {
            _dieValue = random.Next(1, 7);
            return _dieValue;
        }

        public int GetDieValue()
        {
            return _dieValue;
        }
    }
}

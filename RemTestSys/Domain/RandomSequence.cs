using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Domain
{
    public class RandomSequence
    {
        int[] _seq;
        int _cursor;
        static Random rnd = new Random();
        public RandomSequence(int start, int count)
        {
            if (count < 1) throw new ArgumentException("count");
            _seq = new int[count];
            for (int i = 0; i < _seq.Length; i++)
            {
                _seq[i] = start++;
            }
            Mix();
            _cursor = -1;
        }

        public int GetNext()
        {
            _cursor++;
            if (_cursor >= _seq.Length) _cursor = 0;
            return _seq[_cursor];
        }

        private void Mix()
        {
            for (int i = 0; i < _seq.Length; i++)
            {
                int boof = _seq[i];
                int r = rnd.Next(i, _seq.Length);
                _seq[i] = _seq[r];
                _seq[r] = boof;
            }
        }
    }
}

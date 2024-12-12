using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Scripts
{
    public class MultiHeadTape
    {
        #region Internal Fields
        private readonly List<string> _tape;
        #endregion

        #region Private Variables
        private int _size;
        private readonly int _headCount;
        private readonly List<int> _headPositions;
        #endregion

        #region Public Variables
        public List<string> GetTapeSymbols() => _tape;

        public List<int> GetHeadPositions() => _headPositions;

        public int TapeLength() => _tape.Count;
        #endregion

        #region Ctor
        public MultiHeadTape(string tapeInput, HashSet<string> inputAlphabet, int headCount)
        {
            _tape = new List<string>();
            _headPositions = new List<int>();
            for (var i = 0; i < headCount; i++)
            {
                _headPositions.Add(0);
            }
            _headCount = headCount;
            InitializeTape(tapeInput, inputAlphabet);
        }
        #endregion

        #region Public Methods
        public string ShowTape() => string.Concat(_tape);

        public List<string> Read() =>
            _headPositions.Select(head => _tape[head]).ToList();

        public void Write(List<string> symbols, HashSet<string> tapeAlphabet, List<Motion> motions)
        {
            for (var i = 0; i < symbols.Count; i++)
            {
                var symbol = symbols[i];
                var motion = motions[i];
                var head = _headPositions[i];

                if (!tapeAlphabet.Contains(symbol))
                    throw new ArgumentException($"The symbol '{symbol}' is not in the tape alphabet.");

                _tape[head] = symbol;

                var newHead = MoveHead(head, motion);
                _headPositions[i] = newHead;
            }

        }

        #endregion

        #region Private Methods
        private int MoveHead(int head, Motion motion)
        {
            var newHead = head + (int)motion;

            if(newHead >= _size)
            {
                _tape.Add("_");
                _size++;
            }

            else if (newHead < 0)
            {
                _tape.Insert(0, "_");
                _size++;
                newHead = 0;
                for (var i = 0; i < _headCount; i++)
                {
                    if (i == newHead) continue;
                    _headPositions[i]++;
                }
            }
            return newHead;
        }


        private void InitializeTape(string tapeInput, HashSet<string> inputAlphabet)
        {
            if (tapeInput.Length == 0)
            {
                _tape.Add("_");
                _size++;
                return;
            }

            foreach (var ch in tapeInput.Select(character => character.ToString()))
            {
                if (!inputAlphabet.Contains(ch))
                    throw new ArgumentException($"The symbol '{ch}' is not in the input alphabet."); ;
                _tape.Add(ch);
                _size++;
            }
        }
        #endregion
    }
}
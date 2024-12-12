using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Scripts
{
    public class StandardTape
    {
        #region Internal Fields
        private readonly List<string> _tape;
        #endregion

        #region Private Variables
        private int _size;
        private int _head;
        #endregion

        #region Public Variables
        public int HeadPosition => _head;
        public List<string> GetTapeSymbols() => _tape;

        #endregion

        #region Ctor
        public StandardTape(string tapeInput, HashSet<string> inputAlphabet)
        {
            _tape = new List<string>();
            _head = 0;
            InitializeTape(tapeInput, inputAlphabet);
        }
        #endregion

        #region Public Methods

        public string ShowTape() => string.Concat(_tape);

        public string Read() => _tape[_head];

        public void Write(string symbol, HashSet<string> tapeAlphabet, Motion motion)
        {
            if(!tapeAlphabet.Contains(symbol))
                throw new ArgumentException($"The symbol '{symbol}' is not in the tape alphabet.");
            _tape[_head] = symbol;
            MoveHead(motion);
        }

        #endregion

        #region Private Methods
        private void MoveHead(Motion motion)
        {
            var newHead = _head + (int)motion;

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
            }
            _head = newHead;
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
                    throw new ArgumentException($"The symbol '{ch}' is not in the tape alphabet.");
                _tape.Add(ch);
                _size++;
            }
        }
        #endregion
    }
}

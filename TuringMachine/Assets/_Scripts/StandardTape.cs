using System;
using System.Collections.Generic;
using UnityEngine;

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

        public string ShowTape()
        {
            var str = "";
            foreach (var a in _tape)
            {
                str += a;
            }
            return str;
        }

        public string Read()
        {
            if (_head >= _tape.Count)
                return "_";
            return _tape[_head];
        }


        public void Write(string symbol, HashSet<string> tapeAlphabet, Motion motion)
        {
            if(!tapeAlphabet.Contains(symbol))
                throw new ArgumentException($"The symbol '{symbol}' is not in the tape alphabet.");
            if (_head < _size)
            {
                _tape[_head] = symbol;
            }
            else
            {
                _tape.Add(symbol);
                _size++;
            }
            MoveHead(motion);
        }

        public List<string> GetTapeSymbols() => _tape;

        #endregion

        #region Private Methods
        private void MoveHead(Motion motion)
        {
            var newHead = _head + (int)motion;

            if (newHead < 0)
            {
                _tape.Insert(0, "_");
                _size++;
                newHead = 0;
            }

            _head = newHead;
        }

        private void InitializeTape(string tapeInput, HashSet<string> inputAlphabet)
        {
            foreach (var character in tapeInput)
            {
                var ch = character.ToString();
                if (!inputAlphabet.Contains(ch)) throw new ArgumentException($"The symbol '{ch}' is not in the tape alphabet.");
                _tape.Add(ch);
                _size++;
            }
        }
        #endregion
    }
}

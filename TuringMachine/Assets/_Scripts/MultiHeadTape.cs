using System;
using System.Collections.Generic;
using UnityEngine;

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

        public string ShowTape()
        {
            var str = "";
            foreach (var a in _tape)
            {
                str += a;
            }
            return str;
        }
        public List<string> Read()
        {
            var headReads = new List<string>();
            for (var i = 0; i < _headCount; i++)
            {
                var head = _headPositions[i];
                if (head >= _tape.Count)
                    headReads.Add("_");
                else
                {
                    headReads.Add(_tape[head]);
                }
            }

            return headReads;
        }
        public List<string> GetTapeSymbols() => _tape;
        public List<int> GetHeadPositions() => _headPositions;


        public void Write(List<string> symbols, HashSet<string> tapeAlphabet, List<Motion> motions)
        {
            for (var i = 0; i < symbols.Count; i++)
            {
                var symbol = symbols[i];
                var motion = motions[i];
                var head = _headPositions[i];

                if (!tapeAlphabet.Contains(symbol))
                    throw new ArgumentException($"The symbol '{symbol}' is not in the tape alphabet.");
                if (head < _size)
                {
                    _tape[head] = symbol;
                }
                else
                {
                    _tape.Add(symbol);
                    _size++;
                }
                var newHead = MoveHead(head, motion);
                _headPositions[i] = newHead;
            }

        }

        public int TapeLength()
        {
            return _tape.Count;
        }
        #endregion

        #region Private Methods
        private int MoveHead(int head, Motion motion)
        {
            var newHead = head + (int)motion;

            if (newHead < 0)
            {
                _tape.Insert(0, "_");
                _size++;

                // Adjust all head positions to account for the shift
                for (var i = 0; i < _headPositions.Count; i++)
                {
                    _headPositions[i]++;
                }

                newHead = 0;
            }

            return newHead;
        }


        private void InitializeTape(string tapeInput, HashSet<string> inputAlphabet)
        {
            foreach (var character in tapeInput)
            {
                var ch = character.ToString();
                if (!inputAlphabet.Contains(ch)) throw new ArgumentException($"The symbol '{ch}' is not in the input alphabet."); ;
                _tape.Add(ch);
                _size++;
            }
        }
        #endregion
    }
}
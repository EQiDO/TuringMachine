using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Scripts
{
    public class StandardTuringMachine
    {
        #region Private Variables

        private HashSet<string> _inputAlphabet;
        private HashSet<string> _tapeAlphabet;
        private readonly Dictionary<(string currentState, string readSymbol), (string nextState, string writeSymbol, Motion motion)> _transitionFunction;
        //private string _initialState;
        private string _acceptState;

        private string _currentState;
        public StandardTape tape;
        #endregion

        #region Ctor

        public StandardTuringMachine(
            string tapeInput,
            HashSet<string> inputAlphabet,
            HashSet<string> tapeAlphabet,
            Dictionary<(string state, string symbol), (string nextState, string nextSymbol, Motion motion)> transitionFunction,
            string initialState,
            string acceptState)

        {
            _inputAlphabet = inputAlphabet;
            _tapeAlphabet = tapeAlphabet;
            _transitionFunction = transitionFunction;
            _acceptState = acceptState;
            InitializeMachine(initialState, tapeInput, inputAlphabet);
        }
        #endregion

        #region Private Methods

        private void InitializeMachine(string initialState, string tapeInput, HashSet<string> inputAlphabet)
        {
            _currentState = initialState;
            tape = new StandardTape(tapeInput, inputAlphabet);
        }

        private void ApplyTransition((string nextState, string writeSymbol, Motion motion) transitionValue, HashSet<string> tapeAlphabet)
        {
            var (nextState, writeSymbol, motion) = transitionValue;

            _currentState = nextState;
            tape.Write(writeSymbol, tapeAlphabet, motion);
        }
        #endregion

        #region Public Methods

        public bool StartMachine()
        {

            while (true)
            {
                var transitionKey = (_currentState, tape.Read());
                //var transitionValue = _transitionFunction.FirstOrDefault(x => x.Key == transitionKey).Value;

                if (_transitionFunction.TryGetValue(transitionKey, out var transitionValue))
                {
                    ApplyTransition(transitionValue, _tapeAlphabet);
                    tape.ShowTape();
                }
                else
                {
                    return _currentState == _acceptState;
                }
                
            }
        }
        #endregion
    }
}

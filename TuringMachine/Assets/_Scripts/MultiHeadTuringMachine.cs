using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Scripts
{
    public class MultiHeadTuringMachine
    {
        #region Private Variables

        private HashSet<string> _inputAlphabet;
        private HashSet<string> _tapeAlphabet;
        private readonly Dictionary<(string currentState, List<string> readSymbols), (string nextState, List<string> writeSymbols, List<Motion> motions)> _transitionFunction;
        //private string _initialState;
        private string _acceptState;

        private string _currentState;
        public MultiHeadTape tape;
        #endregion

        #region Ctor

        public MultiHeadTuringMachine(
            string tapeInput,
            HashSet<string> inputAlphabet,
            HashSet<string> tapeAlphabet,
            Dictionary<(string state, List<string> readSymbols), (string nextState, List<string> writeSymbols, List<Motion> motions)> transitionFunction,
            string initialState,
            string acceptState,
            int headCount)
        {
            _inputAlphabet = inputAlphabet;
            _tapeAlphabet = tapeAlphabet;
            _transitionFunction = transitionFunction;
            _acceptState = acceptState;
            InitializeMachine(initialState, tapeInput, inputAlphabet, headCount);
        }
        #endregion

        #region Private Methods

        private void InitializeMachine(string initialState, string tapeInput, HashSet<string> inputAlphabet, int headCount)
        {
            _currentState = initialState;
            tape = new MultiHeadTape(tapeInput, inputAlphabet, headCount);
        }

        private void ApplyTransition((string nextState, List<string> writeSymbols, List<Motion> motions) transitionValue, HashSet<string> tapeAlphabet)
        {
            var (nextState, writeSymbols, motions) = transitionValue;

            _currentState = nextState;
            tape.Write(writeSymbols, tapeAlphabet, motions);
        }

        private bool IsEqual((string currentState, List<string> readSymbols) current, (string transitionState, List<string> transitionSymbols) transition)
        {
            return current.currentState == transition.transitionState &&
                   current.readSymbols.SequenceEqual(transition.transitionSymbols);
        }
        #endregion

        #region Public Methods

        public bool StartMachine()
        {

            while (true)
            {
                var transitionKey = (_currentState, tape.Read());
                var transitionValue = _transitionFunction.FirstOrDefault(x => IsEqual(transitionKey, x.Key)).Value;

                if (transitionValue.Equals(default((string nextState, List<string> writeSymbols, List<Motion> motions))))
                {
                    Debug.LogError("No transition available. Machine halted.");
                    return _currentState == _acceptState;
                }

                try
                {
                    ApplyTransition(transitionValue, _tapeAlphabet);

                }
                catch (Exception e)
                {
                    Debug.LogError($"Error while applying transition: {e}");
                    return _currentState == _acceptState;
                }

                if (_currentState == _acceptState)
                {
                    //Debug.Log("Machine accepted the input.");
                    return true;
                }

            }
        }
        #endregion

    }
}
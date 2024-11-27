using System;
using System.Collections;
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

        private readonly GridManager _gridManager;

        private string _currentState;
        public StandardTape tape;
        #endregion

        #region Ctor

        public StandardTuringMachine(
            GridManager gridManager,
            string tapeInput,
            HashSet<string> inputAlphabet,
            HashSet<string> tapeAlphabet,
            Dictionary<(string state, string symbol), (string nextState, string nextSymbol, Motion motion)> transitionFunction,
            string initialState,
            string acceptState,
            bool hasWait)

        {
            _inputAlphabet = inputAlphabet;
            _tapeAlphabet = tapeAlphabet;
            _transitionFunction = transitionFunction;
            _acceptState = acceptState;
            _gridManager = gridManager;
            InitializeMachine(initialState, tapeInput, inputAlphabet, hasWait);
        }
        #endregion

        #region Private Methods

        private void InitializeMachine(string initialState, string tapeInput, HashSet<string> inputAlphabet, bool hasWait)
        {
            _currentState = initialState;
            tape = new StandardTape(tapeInput, inputAlphabet);
            if(hasWait)
                _gridManager.AddCells(tape.GetTapeSymbols());

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
                var transitionValue = _transitionFunction.FirstOrDefault(x => x.Key == transitionKey).Value;

                try
                {
                    ApplyTransition(transitionValue, _tapeAlphabet);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error while applying transition: {e}");
                    return _currentState == _acceptState; ;
                }
                if (_currentState == _acceptState)
                {
                    //Debug.Log("Machine accepted the input.");
                    return true;
                }
            }
        }
        public void StartMachineWithDelay(MonoBehaviour monoBehaviour, float delay)
        {
            monoBehaviour.StartCoroutine(RunMachineWithDelay(delay));
        }

        private IEnumerator RunMachineWithDelay(float delay)
        {
            while (true)
            {
                var transitionKey = (_currentState, tape.Read());
                var transitionValue = _transitionFunction.FirstOrDefault(x => x.Key == transitionKey).Value;

                if (transitionValue.Equals(default((string nextState, string writeSymbol, Motion motion))))
                {
                    Debug.LogError("No transition available. Machine halted.");
                    yield break;
                }
                
                ApplyTransition(transitionValue, _tapeAlphabet);
                var headPosition = new List<int> { tape.HeadPosition };
                _gridManager.UpdateGrid(tape.GetTapeSymbols(), headPosition);

                yield return new WaitForSeconds(delay);

                if (_currentState == _acceptState)
                {
                    Debug.Log("Machine accepted the input.");
                    yield break;
                }
            }
        }
        #endregion
    }
}

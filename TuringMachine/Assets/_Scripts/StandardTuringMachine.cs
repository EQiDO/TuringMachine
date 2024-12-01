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

        private HashSet<string> _tapeAlphabet;
        private readonly Dictionary<(string currentState, string readSymbol), (string nextState, string writeSymbol, Motion motion)> _transitionFunction;
        private string _acceptState;

        private readonly GridManager _gridManager;

        private string _currentState;
        private StandardTape _tape;
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
            _tape = new StandardTape(tapeInput, inputAlphabet);
            if(hasWait)
                _gridManager.AddCells(_tape.GetTapeSymbols());
        }

        private void ApplyTransition((string nextState, string writeSymbol, Motion motion) transitionValue)
        {
            var (nextState, writeSymbol, motion) = transitionValue;

            _currentState = nextState;
            _tape.Write(writeSymbol, _tapeAlphabet, motion);
        }
        #endregion

        #region Public Methods

        public bool StartMachine()
        {

            while (true)
            {

                if (!_transitionFunction.TryGetValue((_currentState, _tape.Read()), out var transitionValue))
                {
                    Debug.LogError("No transition available. Machine halted.");
                    return _currentState == _acceptState;
                }

                try
                {
                    ApplyTransition(transitionValue);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error while applying transition: {e}");
                    return _currentState == _acceptState;
                }

                if (_currentState == _acceptState)
                {
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
            var headPosition = new List<int> { _tape.HeadPosition };
            _gridManager.UpdateGrid(_tape.GetTapeSymbols(), headPosition);

            yield return new WaitForSeconds(delay/2);
            while (true)
            {
                if (!_transitionFunction.TryGetValue((_currentState, _tape.Read()), out var transitionValue))
                {
                    Debug.LogError("No transition available. Machine halted.");
                    yield break;
                }

                yield return new WaitForSeconds(delay);

                try
                {
                    ApplyTransition(transitionValue);
                    headPosition = new List<int> { _tape.HeadPosition };
                    _gridManager.UpdateGrid(_tape.GetTapeSymbols(), headPosition);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error while applying transition: {e}");
                    yield break;
                }

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

using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;
using System.Text;

namespace Assets._Scripts
{
    public class Test : MonoBehaviour
    {
        #region Private Variables
        private readonly HashSet<string> _inputAlphabet = new HashSet<string> { "a", "b", "c" };
        private GridManager _gridManager;
        [SerializeField] private float _delay = 0.5f;
        #endregion

        #region Private Methods
        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();
        }

        void Start()
        {
            // Compare
            CompareAnBn(1000);
            //CompareWReverse(RandomCharacter(1000, _inputAlphabet.ToList()));

            // Standard
            //StandardAnBnTuringMachine(5, true);
            //StandardWreverseTuringMachine(RandomCharacter(4, _inputAlphabet.ToList()), true);

            // Two-Head
            //TwoHeadAnBnTuringMachine(5, true);
            //TwoHeadWreverseTuringMachine(RandomCharacter(6, _inputAlphabet.ToList()), true);

            //Three-Head
            //ThreeHeadTwoPowerNTuringMachine(3, true);
        }
        #endregion

        #region Standard
        private void StandardAnBnTuringMachine(int n, bool hasWait)
        {
            var transitionFunction = new Dictionary<(string state, string symbol), (string nextState, string writeSymbol, Motion motion)>
            {
                //q0
                { ("q0", "a"), ("q1", "x", Motion.R) },
                { ("q0", "y"), ("q3", "y", Motion.R) },
                //q1
                { ("q1", "a"), ("q1", "a", Motion.R) },
                { ("q1", "y"), ("q1", "y", Motion.R) },
                { ("q1", "b"), ("q2", "y", Motion.L) },
                //q2
                { ("q2", "a"), ("q2", "a", Motion.L) },
                { ("q2", "y"), ("q2", "y", Motion.L) },
                { ("q2", "x"), ("q0", "x", Motion.R) },
                //q3
                { ("q3", "y"), ("q3", "y", Motion.R) },
                { ("q3", "_"), ("qf", "_", Motion.S) }
            };

            var inputAlphabet = new HashSet<string> { "a", "b" };
            var tapeAlphabet = new HashSet<string> { "a", "b", "x", "y", "_" };

            var tapeInput = new string('a', n) + new string('b', n);

            var turingMachine = new StandardTuringMachine(
                _gridManager,
                tapeInput,
                inputAlphabet,
                tapeAlphabet,
                transitionFunction,
                "q0",
                "qf",
                hasWait
            );

            if (hasWait)
            {
                turingMachine.StartMachineWithDelay(this, _delay);
            }
            else
            {
                var result = turingMachine.StartMachine();
                Debug.Log($"Machine result: {(result ? $"Accepted" : "Rejected")}");
            }
        }

        private void StandardWreverseTuringMachine(string w, bool hasWait)
        {
            var transitionFunction = new Dictionary<(string state, string symbol), (string nextState, string writeSymbol, Motion motion)>
            {
                //q0
                { ("q0", "a"), ("q0", "a", Motion.R) },
                { ("q0", "b"), ("q0", "b", Motion.R) },
                { ("q0", "c"), ("q0", "c", Motion.R) },
                { ("q0", "_"), ("q1", "_", Motion.L) },

                //q1
                { ("q1", "a"), ("q2", "a", Motion.L) },
                { ("q1", "b"), ("q2", "b", Motion.L) },
                { ("q1", "c"), ("q2", "c", Motion.L) },

                //q2
                { ("q2", "a"), ("q3", "x", Motion.R) },
                { ("q2", "b"), ("q4", "x", Motion.R) },
                { ("q2", "c"), ("q5", "x", Motion.R) },
                { ("q2", "x"), ("q2", "x", Motion.L) },
                { ("q2", "_"), ("q9", "_", Motion.R) },

                //q3
                { ("q3", "a"), ("q3", "a", Motion.R) },
                { ("q3", "b"), ("q3", "b", Motion.R) },
                { ("q3", "c"), ("q3", "c", Motion.R) },
                { ("q3", "x"), ("q3", "x", Motion.R) },
                { ("q3", "_"), ("q6", "a", Motion.L) },

                //q4
                { ("q4", "a"), ("q4", "a", Motion.R) },
                { ("q4", "b"), ("q4", "b", Motion.R) },
                { ("q4", "c"), ("q4", "c", Motion.R) },
                { ("q4", "x"), ("q4", "x", Motion.R) },
                { ("q4", "_"), ("q7", "b", Motion.L) },

                //q5
                { ("q5", "a"), ("q5", "a", Motion.R) },
                { ("q5", "b"), ("q5", "b", Motion.R) },
                { ("q5", "c"), ("q5", "c", Motion.R) },
                { ("q5", "x"), ("q5", "x", Motion.R) },
                { ("q5", "_"), ("q8", "c", Motion.L) },

                //q6
                { ("q6", "a"), ("q6", "a", Motion.L) },
                { ("q6", "b"), ("q6", "b", Motion.L) },
                { ("q6", "c"), ("q6", "c", Motion.L) },
                { ("q6", "x"), ("q2", "x", Motion.L) },

                //q7
                { ("q7", "a"), ("q7", "a", Motion.L) },
                { ("q7", "b"), ("q7", "b", Motion.L) },
                { ("q7", "c"), ("q7", "c", Motion.L) },
                { ("q7", "x"), ("q2", "x", Motion.L) },

                //q8
                { ("q8", "a"), ("q8", "a", Motion.L) },
                { ("q8", "b"), ("q8", "b", Motion.L) },
                { ("q8", "c"), ("q8", "c", Motion.L) },
                { ("q8", "x"), ("q2", "x", Motion.L) },

                //q9
                { ("q9", "x"), ("q9", "_", Motion.R) },
                { ("q9", "a"), ("qf", "a", Motion.S) },
                { ("q9", "b"), ("qf", "b", Motion.S) },
                { ("q9", "c"), ("qf", "c", Motion.S) },
            };

            var tapeAlphabet = new HashSet<string> { "a", "b", "c", "x", "_" };

            var tapeInput = w;

            var turingMachine = new StandardTuringMachine(
                _gridManager,
                tapeInput,
                _inputAlphabet,
                tapeAlphabet,
                transitionFunction,
                "q0",
                "qf",
                hasWait
            );

            if (hasWait)
            {
                turingMachine.StartMachineWithDelay(this, _delay);
            }
            else
            {
                var result = turingMachine.StartMachine();
                Debug.Log($"Machine result: {(result ? $"Accepted" : "Rejected")}");
            }
        }

        #endregion

        #region Two-Head
        private void TwoHeadAnBnTuringMachine(int n, bool hasWait)
        {
            var transitionFunction = new Dictionary<(string currentState, List<string> readSymbols), (string nextState, List<string> writeSymbols, List<Motion> motions)>
            {
                //q0
                {
                    ("q0", new List<string> { "a", "a" }),
                    ("q0", new List<string> { "a", "a" }, new List<Motion> { Motion.S, Motion.R })
                },

                {
                    ("q0", new List<string> { "a", "b" }),
                    ("q1", new List<string> { "a", "b" }, new List<Motion> { Motion.S, Motion.S })
                },
                //q1
                {
                    ("q1", new List<string> { "a", "b" }),
                    ("q1", new List<string> { "_", "_" }, new List<Motion> { Motion.R, Motion.R })
                },

                {
                    ("q1", new List<string> { "_", "_" }),
                    ("qf", new List<string> { "_", "_" }, new List<Motion> { Motion.S, Motion.S })
                },
            };

            var inputAlphabet = new HashSet<string> { "a", "b" };
            var tapeAlphabet = new HashSet<string> { "a", "b", "_" };

            var tapeInput = new string('a', n) + new string('b', n);

            var turingMachine = new MultiHeadTuringMachine(
                _gridManager,
                tapeInput,
                inputAlphabet,
                tapeAlphabet,
                transitionFunction,
                "q0",
                "qf",
                2,
                hasWait
            );

            if (hasWait)
            {
                turingMachine.StartMachineWithDelay(this, _delay);
            }
            else
            {
                var result = turingMachine.StartMachine();
                Debug.Log($"Machine result: {(result ? $"Accepted" : "Rejected")}");
            }
        }
        private void TwoHeadWreverseTuringMachine(string w, bool hasWait)
        {
            var transitionFunction = new Dictionary<(string currentState, List<string> readSymbols), (string nextState, List<string> writeSymbols, List<Motion> motions)>
            {
                //q0
                {
                    ("q0", new List<string> { "a", "a" }),
                    ("q0", new List<string> { "a", "a" }, new List<Motion> { Motion.R, Motion.R })
                },

                {
                    ("q0", new List<string> { "b", "b" }),
                    ("q0", new List<string> { "b", "b" }, new List<Motion> { Motion.R, Motion.R })
                },

                {
                    ("q0", new List<string> { "c", "c" }),
                    ("q0", new List<string> { "c", "c" }, new List<Motion> { Motion.R, Motion.R })
                },

                {
                    ("q0", new List<string> { "_", "_" }),
                    ("q1", new List<string> { "_", "_" }, new List<Motion> { Motion.L, Motion.L })
                },
                //q1
                {
                    ("q1", new List<string> { "a", "a" }),
                    ("q2", new List<string> { "a", "a" }, new List<Motion> { Motion.L, Motion.R })
                },

                {
                    ("q1", new List<string> { "b", "b" }),
                    ("q2", new List<string> { "b", "b" }, new List<Motion> { Motion.L, Motion.R })
                },

                {
                    ("q1", new List<string> { "c", "c" }),
                    ("q2", new List<string> { "c", "c" }, new List<Motion> { Motion.L, Motion.R })
                },

                //q2
                {
                    ("q2", new List<string> { "a", "_" }),
                    ("q2", new List<string> { "_", "a" }, new List<Motion> { Motion.L, Motion.R })
                },

                {
                    ("q2", new List<string> { "b", "_" }),
                    ("q2", new List<string> { "_", "b" }, new List<Motion> { Motion.L, Motion.R })
                },

                {
                    ("q2", new List<string> { "c", "_" }),
                    ("q2", new List<string> { "_", "c" }, new List<Motion> { Motion.L, Motion.R })
                },

                {
                    ("q2", new List<string> { "_", "_" }),
                    ("qf", new List<string> { "_", "_" }, new List<Motion> { Motion.S, Motion.S })
                },
            };

            var tapeAlphabet = new HashSet<string> { "a", "b", "c", "_" };

            var tapeInput = w;

            var turingMachine = new MultiHeadTuringMachine(
                _gridManager,
                tapeInput,
                _inputAlphabet,
                tapeAlphabet,
                transitionFunction,
                "q0",
                "qf",
                2,
                hasWait
            );

            if (hasWait)
            {
                turingMachine.StartMachineWithDelay(this, _delay);
            }
            else
            {
                var result = turingMachine.StartMachine();
                Debug.Log($"Machine result: {(result ? $"Accepted" : "Rejected")}");
            }
        }
        #endregion

        #region Three-Head
        private void ThreeHeadTwoPowerNTuringMachine(int n, bool hasWait)
        {
            var transitionFunction = new Dictionary<(string currentState, List<string> readSymbols), (string nextState, List<string> writeSymbols, List<Motion> motions)>
            {
                //q0
                {
                    ("q0", new List<string> { "1", "1", "1" }),
                    ("q1", new List<string> { "1", "1", "_" }, new List<Motion> { Motion.R, Motion.R, Motion.R  })
                },

                {
                    ("q0", new List<string> { "_", "_", "_" }),
                    ("qf", new List<string> { "_", "_", "1" }, new List<Motion> { Motion.S, Motion.S, Motion.S })
                },

                //q1
                {
                    ("q1", new List<string> { "1", "1", "1" }),
                    ("q1", new List<string> { "1", "1", "1" }, new List<Motion> { Motion.S, Motion.R, Motion.R })
                },

                {
                    ("q1", new List<string> { "1", "_", "_" }),
                    ("q2", new List<string> { "1", "_", "0" }, new List<Motion> { Motion.S, Motion.R, Motion.R  })
                },

                {
                    ("q1", new List<string> { "_", "_", "_" }),
                    ("q2", new List<string> { "_", "_", "0" }, new List<Motion> { Motion.S, Motion.R, Motion.R })
                },

                //q2
                {
                    ("q2", new List<string> { "0", "_", "_" }),
                    ("q3", new List<string> { "0", "_", "1" }, new List<Motion> { Motion.S, Motion.S, Motion.R })
                },

                {
                    ("q2", new List<string> { "1", "_", "_" }),
                    ("q3", new List<string> { "1", "_", "1" }, new List<Motion> { Motion.S, Motion.S, Motion.R })
                },

               
                //q3
                {
                    ("q3", new List<string> { "0", "1", "_" }),
                    ("q4", new List<string> { "0", "1", "1" }, new List<Motion> { Motion.S, Motion.S, Motion.R })
                },

                {
                    ("q3", new List<string> { "1", "1", "_" }),
                    ("q4", new List<string> { "1", "1", "1" }, new List<Motion> { Motion.S, Motion.S, Motion.R })
                },

                //q4
                {
                    ("q4", new List<string> { "1", "1", "_" }),
                    ("q5", new List<string> { "1", "1", "x" }, new List<Motion> { Motion.S, Motion.S, Motion.R })
                },

                {
                    ("q4", new List<string> { "0", "1", "_" }),
                    ("qf", new List<string> { "_", "1", "_" }, new List<Motion> { Motion.S, Motion.S, Motion.S })
                },

                //q5
                {
                    ("q5", new List<string> { "1", "1", "_" }),
                    ("q5", new List<string> { "1", "1", "1" }, new List<Motion> { Motion.S, Motion.R, Motion.R })
                },

                {
                    ("q5", new List<string> { "1", "x", "_" }),
                    ("q6", new List<string> { "1", "1", "_" }, new List<Motion> { Motion.S, Motion.L, Motion.L })
                },

                {
                    ("q5", new List<string> { "0", "1", "_" }),
                    ("q8", new List<string> { "_", "1", "_" }, new List<Motion> { Motion.S, Motion.S, Motion.L })
                },

                //q6
                {
                    ("q6", new List<string> { "1", "1", "1" }),
                    ("q7", new List<string> { "1", "1", "_" }, new List<Motion> { Motion.S, Motion.S, Motion.S })
                },
                //q7
                {
                    ("q7", new List<string> { "1", "1", "_" }),
                    ("q7", new List<string> { "1", "1", "_" }, new List<Motion> { Motion.S, Motion.L, Motion.S })
                },
                {
                    ("q7", new List<string> { "1", "0", "_" }),
                    ("q5", new List<string> { "_", "0", "x" }, new List<Motion> { Motion.R, Motion.R, Motion.R })
                },
                //q8
                {
                    ("q8", new List<string> { "_", "1", "x" }),
                    ("qf", new List<string> { "_", "1", "_" }, new List<Motion> { Motion.S, Motion.S, Motion.S })
                },
            };

            var inputAlphabet = new HashSet<string> { "1"};
            var tapeAlphabet = new HashSet<string> { "1", "0", "x", "_" };

            var inp = new string('1', n);

            var tapeInp = inp;

            var turingMachine = new MultiHeadTuringMachine(
                _gridManager,
                tapeInp,
                inputAlphabet,
                tapeAlphabet,
                transitionFunction,
                "q0",
                "qf",
                3,
                hasWait
            );

            if (hasWait)
            {
                turingMachine.StartMachineWithDelay(this, _delay);
            }
            else
            {
                var result = turingMachine.StartMachine();
                Debug.Log($"Machine result: {(result ? $"Accepted" : "Rejected")}");
            }
        }
        #endregion

        #region Processor
        private void CompareAnBn(int n)
        {
            Debug.Log($"Comparing a^n b^n for n={n}:");

            var standardTime = MeasureExecutionTime(() => StandardAnBnTuringMachine(n, false));
            var multiHeadTime = MeasureExecutionTime(() => TwoHeadAnBnTuringMachine(n, false));

            Debug.Log($"Standard Turing Machine: {standardTime} ms");
            Debug.Log($"Multi-Head Turing Machine: {multiHeadTime} ms");
        }

        private void CompareWReverse(string w)
        {
            Debug.Log($"Comparing w^R for |w| = {w.Length}");

            var standardTime = MeasureExecutionTime(() => StandardWreverseTuringMachine(w, false));
            var multiHeadTime = MeasureExecutionTime(() => TwoHeadWreverseTuringMachine(w, false));

            Debug.Log($"Standard Turing Machine: {standardTime} ms");
            Debug.Log($"Multi-Head Turing Machine: {multiHeadTime} ms");
        }
        private double MeasureExecutionTime(System.Action action)
        {
            var sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
        private string RandomCharacter(int length, List<string> chars)
        {
            var sb = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var randomIndex = Random.Range(0, chars.Count);
                sb.Append(chars[randomIndex]);
            }
            return sb.ToString();
        }
        #endregion

    }
}

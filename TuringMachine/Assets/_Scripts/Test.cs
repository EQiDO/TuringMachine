using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;
using UnityEngine.EventSystems;

namespace Assets._Scripts
{
    public class Test : MonoBehaviour
    {
        void Start()
        {
            //CompareAnBn(10000);

            //CompareWReverse(RandomCharacter(1000, new List<string> {"a", "b", "c"}));
            //for (int i = 0; i < 20; i++)
            //{
            //    MultiHeadWreverseTuringMachine(RandomCharacter(3, new List<string> { "a", "b", "c" }));
            //}
            var sw = new Stopwatch();
            sw.Start();
            StandardWreverseTuringMachine(RandomCharacter(300, new List<string> { "a", "b", "c" }));

            sw.Stop();
            print(sw.ElapsedMilliseconds);

        }

        #region Standard
        private void StandardAnBnTuringMachine(int n)
        {
            var transitionFunction = new Dictionary<(string state, string symbol), (string nextState, string writeSymbol, Motion motion)>
            {
                { ("q0", "a"), ("q1", "x", Motion.R) },
                { ("q0", "y"), ("q3", "y", Motion.R) },
                { ("q1", "a"), ("q1", "a", Motion.R) },
                { ("q1", "y"), ("q1", "y", Motion.R) },
                { ("q1", "b"), ("q2", "y", Motion.L) },
                { ("q2", "a"), ("q2", "a", Motion.L) },
                { ("q2", "y"), ("q2", "y", Motion.L) },
                { ("q2", "x"), ("q0", "x", Motion.R) },
                { ("q3", "y"), ("q3", "y", Motion.R) },
                { ("q3", "_"), ("qf", "_", Motion.L) }
            };

            var inputAlphabet = new HashSet<string> { "a", "b" };
            var tapeAlphabet = new HashSet<string> { "a", "b", "x", "y", "_" };

            var aPart = new string('a', n);
            var bPart = new string('b', n);

            var tapeInp = aPart + bPart;

            var turingMachine = new StandardTuringMachine(
                tapeInp,
                inputAlphabet,
                tapeAlphabet,
                transitionFunction,
                "q0",
                "qf"
            );

            // Run the machine
            var result = turingMachine.StartMachine();
            Debug.Log($"Machine result: {(result ? $"Accepted" : "Rejected")}");
        }

        private void StandardWreverseTuringMachine(string w)
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
                { ("q9", "a"), ("qf", "a", Motion.R) },
                { ("q9", "b"), ("qf", "b", Motion.R) },
                { ("q9", "c"), ("qf", "c", Motion.R) },
            };

            var inputAlphabet = new HashSet<string> { "a", "b", "c" };
            var tapeAlphabet = new HashSet<string> { "a", "b", "c", "x", "_" };

            var tapeInput = w;

            var turingMachine = new StandardTuringMachine(
                tapeInput,
                inputAlphabet,
                tapeAlphabet,
                transitionFunction,
                "q0",
                "qf"
            );

            // Run the machine
            var result = turingMachine.StartMachine();
            Debug.Log($"Machine result: {(result ? $"Accepted" : "Rejected")}");
        }
        #endregion

        #region Multi-Head
        private void MultiHeadAnBnTuringMachine(int n)
        {
            var transitionFunction = new Dictionary<(string currentState, List<string> readSymbols), (string nextState, List<string> writeSymbols, List<Motion> motions)>
            {
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

            var aPart = new string('a', n);
            var bPart = new string('b', n);

            var tapeInp = aPart + bPart;

            var turingMachine = new MultiHeadTuringMachine(
                tapeInp,
                inputAlphabet,
                tapeAlphabet,
                transitionFunction,
                "q0",
                "qf",
                2
            );

            // Run the machine
            var result = turingMachine.StartMachine();
            Debug.Log($"Machine result: {(result ? $"Accepted" : "Rejected")}");
        }
        private void MultiHeadWreverseTuringMachine(string w)
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

            var inputAlphabet = new HashSet<string> { "a", "b", "c" };
            var tapeAlphabet = new HashSet<string> { "a", "b", "c", "_" };

            //var chars = inputAlphabet.ToList();

            var tapeInput = w;

            var turingMachine = new MultiHeadTuringMachine(
                tapeInput,
                inputAlphabet,
                tapeAlphabet,
                transitionFunction,
                "q0",
                "qf",
                2
            );

            var result = turingMachine.StartMachine();
            
            Debug.Log($"Machine result: {(result ? $"Accepted" : "Rejected")}");
            //Debug.Log($"Machine result: {(result ? $"Accepted" : "Rejected")} and The reverse of {w} is {turingMachine.tape.ShowTape()}");
        }
        #endregion

        #region Processor
        private void CompareAnBn(int n)
        {
            Debug.Log($"Comparing a^n b^n for n={n}:");

            var standardTime = MeasureExecutionTime(() => StandardAnBnTuringMachine(n));
            var multiHeadTime = MeasureExecutionTime(() => MultiHeadAnBnTuringMachine(n));

            Debug.Log($"Standard Turing Machine: {standardTime} ms");
            Debug.Log($"Multi-Head Turing Machine: {multiHeadTime} ms");
        }

        private void CompareWReverse(string w)
        {
            //Debug.Log($"Comparing w^R for w=\"{w}\":");

            var standardTime = MeasureExecutionTime(() => StandardWreverseTuringMachine(w));
            var multiHeadTime = MeasureExecutionTime(() => MultiHeadWreverseTuringMachine(w));

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
            var tapeInput = "";
            for (var i = 0; i < length; i++)
            {
                var randomIndex = Random.Range(0, chars.Count);
                var l = chars[randomIndex];
                tapeInput += l;
            }
            //Debug.Log(tapeInput);
            return tapeInput;
        }
        #endregion

    }
}

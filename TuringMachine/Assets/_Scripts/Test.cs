using System.Collections.Generic;
using UnityEngine;
namespace Assets._Scripts
{
    public class Test : MonoBehaviour
    {
        void Start()
        {
            WreverseTuringMachine();
        }

        private void AnBnTuringMachine()
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

            var aPart = new string('a', 10000);
            var bPart = new string('b', 9999);

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


        private void WreverseTuringMachine()
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

            var chars = new List<string> {"a", "b", "c"};

            var tapeInput = "";
            for (var i = 0; i < 10; i++)
            {
                var randomIndex = Random.Range(0, chars.Count);
                var l = chars[randomIndex];
                tapeInput+= l;
            }
            Debug.Log($"tape input is {tapeInput}");

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
            Debug.Log($"Machine result: {(result ? $"Accepted: {turingMachine.tape.ShowTape()}" : "Rejected")}");
        }

    }
}

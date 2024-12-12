# Turing Machine Implementation in Unity

## Introduction

This project implements two Turing Machine variations:

- **Standard Turing Machine**: A classical model with one tape and one head.
- **Multi-Head Turing Machine**: A version with one tape and multiple heads, improving computational efficiency.

## Machine Structures

### Tape (Single Tape Implementation)
- **Structure**: One infinite tape divided into discrete cells, each holding a symbol from the tape alphabet.
- **Operation**: Symbols are read or written by head at specific positions on the tape. Movement and state transitions update head position and tape contents.

### Standard Turing Machine
- **Tape**: One infinite tape shared across all operations.
- **Tape Input**: A finite string of symbols initially placed on the tape.
- **Tape Alphabet (Γ)**: Set of all symbols that can appear on the tape, including the blank symbol.
- **Input Alphabet (Σ)**: Subset of the tape alphabet, excluding the blank symbol, used for input strings.
- **Initial State (q₀)**: The starting state of the machine.
- **Accept State (q_f)**: The state indicating successful computation.
- **Transition Function**: 
  **δ: Q × Γ → Q × Γ × {L, R, S}**
  - **Q**: Set of states.
  - **Γ**: Tape alphabet.
  - **L, R, S**: Movement directions (Left, Right, Stay).

### Tape (Multi-Head Tape Implementation)
- **Structure**: Same as the standard tape.
- **Operation**: same as the standard tape but it can read, write, and move independently for all heads.

### Multi-Head Turing Machine
- **Tape**: Same single tape as the standard version but using multiple heads future.
- **Tape Input**: A finite string of symbols initially placed on the tape.
- **Tape Alphabet (Γ)**: Set of all symbols that can appear on the tape, including the blank symbol.
- **Input Alphabet (Σ)**: Subset of the tape alphabet, excluding the blank symbol, used for input strings.
- **Initial State (q₀)**: The starting state of the machine.
- **Accept State (q_f)**: The state indicating successful computation.
- **Transition Function**:
  **δ: Q × (Γ^k) → Q × (Γ^k) × {L, R, S}^k**
  - **Q**: Set of states.
  - **Γ^k**: Inputs from all k heads.
  - **{L, R, S}^k**: Independent movements for k heads.

## Comparison
### Standard Turing Machine
  - With only one head, the machine can read or write one symbol at a time, requiring more steps to traverse the tape and process multiple sections.

### Multi-Head Turing Machine
  - Each head operates independently, allowing the machine to read or write multiple symbols at different positions in a single step. This reduces the number of transitions needed for tasks that involve processing data spread across the tape.

## Visual Examples
### Speed Comparison
![a^n_b^n](https://github.com/user-attachments/assets/c3bec301-0fb0-4f3c-b77e-00fafa32f447)
![w^r](https://github.com/user-attachments/assets/0a705434-c135-42fc-b378-dabe6d8c6246)

### All Examples Video
https://github.com/user-attachments/assets/81146274-b307-4a7e-94ef-4a6288222854

## User Instructions:
You can create your own machine by going to the [Test.Cs](TuringMachine/Assets/_Scripts/Test.cs) and making a new function, similar to the example functions in the scripts.

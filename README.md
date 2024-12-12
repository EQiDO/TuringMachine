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

## Visual Examples
### Speed Comparison
-![**L = a^n b^n; n = 1000**(a^n b^n.png)]



## Comparison

### Standard Turing Machine
  - With only one head, the machine can read or write one symbol at a time, requiring more steps to traverse the tape and process multiple sections.

### Multi-Head Turing Machine
  - Each head operates independently, allowing the machine to read or write multiple symbols at different positions in a single step. This reduces the number of transitions needed for tasks that involve processing data spread across the tape.

## Benefits of This Implementation

### Code Advantages
1. **OOP**: Modular, reusable classes improve readability and maintainability.
2. **Scalability**: Supports extensions like custom alphabets or variable head counts.

### Why OOP?
- **Modularity**: Independent components for easier development.
- **Reusability**: Shared functionality between machine types.
- **Flexibility**: Easily extendable for new features.


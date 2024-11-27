using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    public class GridManager : MonoBehaviour
    {
        private List<GameObject> _grid = new List<GameObject>();
        private readonly float _spacing = 1;
        private SymbolMaterials _symbolMaterials;
        [SerializeField] private GameObject _cellObj;
        [SerializeField] private GameObject _cellsHolder;

        void Start()
        {
            _symbolMaterials = FindObjectOfType<SymbolMaterials>();
        }
        public void AddCells(List<string> tapeSymbols)
        {
            for (var i = 0; i < tapeSymbols.Count; i++)
            {
                var cell = Instantiate(_cellObj, new Vector3(i * _spacing, 0, 0), Quaternion.identity);
                cell.transform.parent = _cellsHolder.transform;
                _grid.Add(cell);
                UpdateCell(i, tapeSymbols[i]);
            }
        }

        public void UpdateGrid(List<string> tapeSymbols, List<int> headPositions)
        {
            var tapeLength = tapeSymbols.Count;

            while (_grid.Count < tapeLength)
            {
                // Add new cells to the grid if it's too small
                var newCell = Instantiate(_cellObj, new Vector3(_grid.Count * _spacing, 0, 0), Quaternion.identity);
                newCell.transform.parent = _cellsHolder.transform;
                _grid.Add(newCell);
            }

            while (_grid.Count > tapeLength)
            {
                // Remove extra cells if the tape is shorter
                var extraCell = _grid[^1];
                _grid.RemoveAt(_grid.Count - 1);
                Destroy(extraCell);
            }

            // Update materials and head position
            for (var i = 0; i < tapeLength; i++)
            { 
                var symbol = tapeSymbols[i];

                UpdateCell(i, symbol);
            }
            if (headPositions.Count == 1)
            {
                UpdateHeadCellsColor(headPositions[0]);
            }
            else
            {
                UpdateHeadsCellsColor(headPositions);
            }
        }

        public void UpdateCell(int cellIndex, string symbol)
        {

            if (_symbolMaterials.symbolMaterialDict.TryGetValue(symbol, out var material))
            {
                _grid[cellIndex].GetComponent<MeshRenderer>().material = material;
            }
            else
            {
                Debug.LogWarning($"No material found for symbol '{symbol}' at index {cellIndex}");
            }
        }
        public void UpdateHeadCellsColor(int headPosition)
        {
            foreach (var cell in _grid)
            {
                cell.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            if (headPosition >= 0 && headPosition < _grid.Count)
            {
                _grid[headPosition].GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }

        public void UpdateHeadsCellsColor(List<int> headPositions)
        {
            // Reset all cells to white
            foreach (var cell in _grid)
            {
                cell.GetComponent<MeshRenderer>().material.color = Color.white;
            }

            // Track the frequency of head positions
            var positionCounts = new Dictionary<int, int>();
            foreach (var position in headPositions)
            {
                if (positionCounts.ContainsKey(position))
                {
                    positionCounts[position]++;
                }
                else
                {
                    positionCounts[position] = 1;
                }
            }

            // Colors for unique heads
            var headColors = new List<Color> { Color.green, Color.red, Color.blue };

            // Apply colors to cells
            for (var i = 0; i < headPositions.Count; i++)
            {
                var headIndex = headPositions[i];

                if (headIndex < 0 || headIndex >= _grid.Count) continue;

                if (positionCounts[headIndex] > 1)
                {
                    // If multiple heads are here, make it yellow
                    _grid[headIndex].GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                else
                {
                    // Otherwise, use the unique color for the head
                    _grid[headIndex].GetComponent<MeshRenderer>().material.color = headColors[i % headColors.Count];
                }
            }
        }


    }
}

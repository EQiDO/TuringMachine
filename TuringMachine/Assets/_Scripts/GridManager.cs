using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Assets._Scripts
{
    public class GridManager : MonoBehaviour
    {
        #region Private Variables
        private readonly List<GameObject> _grid = new();
        private const float Spacing = 1;
        [SerializeField] private GameObject _cellObj;
        [SerializeField] private GameObject _cellsHolder;

        #endregion

        #region Public Methods
        public void AddCells(List<string> tapeSymbols)
        {
            for (var i = 0; i < tapeSymbols.Count; i++)
            {
                var cell = Instantiate(_cellObj, new Vector3(i * Spacing, 0, 0), Quaternion.identity);
                cell.transform.parent = _cellsHolder.transform;
                _grid.Add(cell);
                UpdateCellSymbol(i, tapeSymbols[i]);
            }

        }

        public void UpdateGrid(List<string> tapeSymbols, List<int> headPositions)
        {
            var tapeLength = tapeSymbols.Count;

            while (_grid.Count < tapeLength)
            {
                var newCell = Instantiate(_cellObj, new Vector3(_grid.Count * Spacing, 0, 0), Quaternion.identity);
                newCell.transform.parent = _cellsHolder.transform;
                _grid.Add(newCell);
            }

            while (_grid.Count > tapeLength)
            {
                var extraCell = _grid[^1];
                _grid.RemoveAt(_grid.Count - 1);
                Destroy(extraCell);
            }

            for (var i = 0; i < tapeLength; i++)
            {
                var symbol = tapeSymbols[i];

                UpdateCellSymbol(i, symbol);
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

        public void UpdateCellSymbol(int cellIndex, string symbol)
        {
            var text = _grid[cellIndex].GetComponentInChildren<TMP_Text>();
            text.text = symbol;
        }

        #endregion

        #region Private Methods
        private void UpdateHeadCellsColor(int headPosition)
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

        private void UpdateHeadsCellsColor(List<int> headPositions)
        {
            foreach (var cell in _grid)
            {
                cell.GetComponent<MeshRenderer>().material.color = Color.white;
            }

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

            var headColors = new List<Color> { Color.green, Color.red, Color.blue };

            for (var i = 0; i < headPositions.Count; i++)
            {
                var headIndex = headPositions[i];

                if (headIndex < 0 || headIndex >= _grid.Count) continue;

                if (positionCounts[headIndex] > 1)
                {
                    _grid[headIndex].GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                else
                {
                    _grid[headIndex].GetComponent<MeshRenderer>().material.color = headColors[i % headColors.Count];
                }
            }
        }
        #endregion
    }
}

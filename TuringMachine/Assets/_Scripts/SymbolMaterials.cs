using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolMaterials : MonoBehaviour
{
    #region Private Variables
    [SerializeField] private List<SymbolMaterial> _symbolMaterials;
    [System.Serializable]
    public class SymbolMaterial
    {
        public string symbol;
        public Material material;
    }
    #endregion

    #region Public Variables
    public Dictionary<string, Material> symbolMaterialDict;
    #endregion

    #region Private Methods
    private void Awake()
    {
        symbolMaterialDict = new Dictionary<string, Material>();
        foreach (var entry in _symbolMaterials)
        {
            symbolMaterialDict[entry.symbol] = entry.material;
        }
    }
    #endregion

}

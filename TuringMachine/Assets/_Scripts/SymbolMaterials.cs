using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolMaterials : MonoBehaviour
{
    [SerializeField] private List<SymbolMaterial> _symbolMaterials;

    public Dictionary<string, Material> symbolMaterialDict;

    [System.Serializable]
    public class SymbolMaterial
    {
        public string symbol;
        public Material material;
    }

    private void Awake()
    {
        symbolMaterialDict = new Dictionary<string, Material>();
        foreach (var entry in _symbolMaterials)
        {
            symbolMaterialDict[entry.symbol] = entry.material;
        }
    }

}

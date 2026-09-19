using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CellularAutomataGenData", menuName = "Scriptable Objects/CellularAutomataGenData")]
public class CellularAutomataGenData : ScriptableObject
{
    [Range(0, 100)]
    public int RandomFillPercent;
    public int SmoothAmount;
    public TileBase Tile;
}

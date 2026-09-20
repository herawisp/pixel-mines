using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject {

    public int[] MaxHealth;
    public int MinYieldAmount;
    public int MaxYieldAmount;
    public TileBase Tile;
}

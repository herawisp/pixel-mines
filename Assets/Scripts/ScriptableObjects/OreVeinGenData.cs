using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "OreVeinGenData", menuName = "Scriptable Objects/OreVeinGenData")]
public class OreVeinGenData : ScriptableObject {
    
    [Range(0, 100)]
    public int InitialChance;
    public int MinVeinAmount;
    public int MaxVeinAmount;
    public int MinYHeight;
    public int MaxYHeight;
    public int ChanceDepletionRate;
    public int OreBlockChance;
    public int ClusterBlockChance;
    public TileBase OreBlockTile;
    public TileBase ClusterBlockTile;
    public bool OverrideAir;
}

using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "PerlinNoiseGenData", menuName = "Scriptable Objects/PerlinNoiseGenData")]
public class PerlinNoiseGenData : ScriptableObject {

    public float Resolution;
    public float Frequency;
    public float MinNoise;
    public float MaxNoise;
    public int MinYHeight;
    public bool OverrideAir;
    public TileBase Tile;
}

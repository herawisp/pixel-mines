using UnityEngine;
using UnityEngine.Tilemaps;

public class RockGenerationData : ScriptableObject {
    
    public float Resolution;
    public float Frequency;
    public float minNoise;
    public float maxNoise;
    public TileBase tile;
}

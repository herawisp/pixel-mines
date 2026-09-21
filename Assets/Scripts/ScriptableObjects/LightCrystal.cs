using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "LightCrystal", menuName = "Scriptable Objects/LightCrystal")]
public class LightCrystal : ScriptableObject {

    public LightCrystalSize Size = LightCrystalSize.Small;
    public TileBase Tile;
    public int Fuel;
    public int Chance;
}

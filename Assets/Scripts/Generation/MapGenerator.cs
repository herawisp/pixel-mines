using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {

    //================================================================================================//
    //================================================================================================//

    [Header("Cave Settings")]
    public int Width;
    public int Height;
    public int Seed;
    public int BedrockThickness;

    [Header("Tilemap Settings")]
    public Tilemap Tilemap;
    public Tilemap BackgroundTilemap;
    public TileBase CaveWallTile;
    public TileBase BedrockTile;

    [Header("Cave Generation Datas")]
    public CellularAutomataGenData BaseCaveData;
    public List<PerlinNoiseGenData> RockGenerationDatas;
    public List<OreVeinGenData> OreVeinGenDatas;
    public DoorGenerator DoorGenerator;
    public LightCrystalGenerator LightCrystalGenerator;

    [Header("Cave Information")]
    public Vector3Int EntrancePosition {get; private set;}
    public Vector3Int ExitPosition {get; private set;}

    readonly CellularAutomata _cellularAutomata = new();
    readonly PerlinNoiseMap _perlinNoiseMap = new();
    readonly OreVeinGenerator _oreVeinGenerator = new();

    //================================================================================================//
    //================================================================================================//

    public void GenerateCave() {
        Seed = UnityEngine.Random.Range(0, 1000);
        System.Random random = new(Seed);

        _cellularAutomata.GenerateMap(Width, Height, random, Tilemap, BaseCaveData);
        _perlinNoiseMap.GenerateMaps(Width, Height, random, Tilemap, RockGenerationDatas);
        _oreVeinGenerator.GenerateOreVeins(Width, Height, random, Tilemap, OreVeinGenDatas);
        (EntrancePosition, ExitPosition) = DoorGenerator.GenerateDoors(Width, Height, Tilemap, random);
        LightCrystalGenerator.GenerateAllLightCrystals(Width, Height, random, Tilemap);
        GenerateCaveWalls();
        GenerateBedrock();
    }

    void GenerateCaveWalls() {
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                Vector3Int cellPosition = new(x, y);
                BackgroundTilemap.SetTile(cellPosition, CaveWallTile);
            }
        }
    }

    void GenerateBedrock() {
        for (int x = - BedrockThickness; x <= Width + BedrockThickness; x++) {
            for (int y = - BedrockThickness; y <= Height + BedrockThickness; y++) {
                if (x >= 0 && x < Width && y >= 0 && y < Height) continue;

                Vector3Int cellPosition = new(x, y);
                Tilemap.SetTile(cellPosition, BedrockTile);
            }
        }
    }
    
    //================================================================================================//
    //================================================================================================//

}

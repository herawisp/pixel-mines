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
    public Tilemap Tilemap;

    [Header("Cave Generation Datas")]
    public CellularAutomataGenData BaseCaveData;
    public List<PerlinNoiseGenData> RockGenerationDatas;
    public List<OreVeinGenData> OreVeinGenDatas;
    public DoorGenerator DoorGenerator;

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
    }
    
    //================================================================================================//
    //================================================================================================//

}

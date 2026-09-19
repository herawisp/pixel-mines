using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoiseMap {

    //================================================================================================//
    //================================================================================================//

    float GetHeight(
        float x, 
        float y, 
        float resolution, 
        float frequency,
        System.Random random
    ) {
        float sampleX = (x + (float) random.NextDouble()) / resolution * frequency;
        float sampleY = (y + (float) random.NextDouble()) / resolution * frequency;
        float noiseHeight = Mathf.PerlinNoise(sampleX, sampleY);
        return noiseHeight;
    }

    void GenerateMap(
        int width,
        int height,
        System.Random random,
        Tilemap tilemap,
        PerlinNoiseGenData data
    ) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height - data.MinYHeight; y++) {
                float noiseHeight = GetHeight(x, y, data.Resolution, data.Frequency, random);
                if (noiseHeight > data.MinNoise && noiseHeight < data.MaxNoise) {
                    Vector3Int position = new(x, y);
                    if (!data.OverrideAir && tilemap.GetTile(position) == null) continue;
                    tilemap.SetTile(position, data.Tile);
                }
            }
        }
    }

    public void GenerateMaps(
        int width, 
        int height, 
        System.Random random,
        Tilemap tilemap,
        List<PerlinNoiseGenData> datas
    ) {
        foreach (PerlinNoiseGenData data in datas) {
            GenerateMap(width, height, random, tilemap, data);
        }
    }
    
    //================================================================================================//
    //================================================================================================//
}

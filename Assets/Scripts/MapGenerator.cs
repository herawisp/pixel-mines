using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0,100)]
    public int randomFillPercent;
    
    public Tilemap tilemap;
    public TileBase tile;

    int[,] map;
    int[,] smoothMap;

    void Start() {
        GenerateMap();
    }

    void GenerateMap() {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 3; i++) {
            SmoothMap();
        }
        GenerateTiles();
    }

    void RandomFillMap() {
        if (useRandomSeed) {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
                    map[x,y] = 1;
                } else {
                    map[x,y] = (pseudoRandom.Next(0,100) < randomFillPercent)? 1 : 0;
                }
            }
        }
    }

    void SmoothMap() {
        smoothMap = (int[,]) map.Clone();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4) {
                    smoothMap[x,y] = 1;
                } else if (neighbourWallTiles < 4) {
                    smoothMap[x,y] = 0;
                }
            }
        }
        map = (int[,]) smoothMap.Clone();
    }

    int GetSurroundingWallCount(int gridX, int gridY) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += Math.Clamp(map[neighbourX,neighbourY], 0, 1);
                    }
                } else {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    void GenerateTiles() {
        if (map != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (map[x,y] == 1) {
                        Vector3Int position = new(x, y);
                        tilemap.SetTile(position, tile);
                    }
                }
            }
        }
    }
}

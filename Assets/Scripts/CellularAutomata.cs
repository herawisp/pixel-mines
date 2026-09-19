using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellularAutomata {
    
    //================================================================================================//
    //================================================================================================//

    int[,] _map;
    int[,] _smoothMap;

    //================================================================================================//
    //================================================================================================//

    public void GenerateMap(
        int width,
        int height,
        System.Random random,
        Tilemap tilemap,
        CellularAutomataGenData data
    ) {
        _map = new int[width, height];
        RandomFillMap(width, height, random, data.RandomFillPercent);

        for (int i = 0; i < data.SmoothAmount; i++) {
            SmoothMap(width, height);
        }
        GenerateTiles(width, height, tilemap, data);
    }

    void RandomFillMap(
        int width,
        int height,
        System.Random random,
        int randomFillPercent
    ) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
                    _map[x,y] = 1;
                } else {
                    _map[x,y] = (random.Next(0,100) < randomFillPercent)? 1 : 0;
                }
            }
        }
    }

    void SmoothMap(
        int width,
        int height
    ) {
        _smoothMap = (int[,]) _map.Clone();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighbourWallTiles = GetSurroundingWallCount(x, y, width, height);

                if (neighbourWallTiles > 4) {
                    _smoothMap[x,y] = 1;
                } else if (neighbourWallTiles < 4) {
                    _smoothMap[x,y] = 0;
                }
            }
        }
        _map = (int[,]) _smoothMap.Clone();
    }

    int GetSurroundingWallCount(
        int gridX, 
        int gridY,
        int width,
        int height
    ) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += Math.Clamp(_map[neighbourX,neighbourY], 0, 1);
                    }
                } else {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    void GenerateTiles(
        int width,
        int height,
        Tilemap tilemap,        
        CellularAutomataGenData data
    ) {
        if (_map != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (_map[x,y] == 1) {
                        Vector3Int position = new(x, y);
                        tilemap.SetTile(position, data.Tile);
                    }
                }
            }
        }
    }

    //================================================================================================//
    //================================================================================================//

}

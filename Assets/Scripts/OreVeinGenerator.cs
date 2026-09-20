using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

enum OreBlockType {Block, Cluster}

public class OreVeinGenerator : MonoBehaviour {
    
    Vector3Int GetRandomPosition(
        int width,
        int height,
        Tilemap tilemap,
        System.Random random,
        OreVeinGenData data
    ) {
        Vector3Int position = new();
        int maxTries = 100;
        int tries = 0;

        do {
            if (tries >= maxTries) break;
            tries++;

            position = new Vector3Int(
                random.Next(0, width),
                random.Next(height - data.MinYHeight, height - data.MaxYHeight),
                0
            );
        } while (!data.OverrideAir && tilemap.GetTile(position) == null);
        
        return position;
    }

    OreBlockType GetRandomOreBlockType(
        System.Random random,
        OreVeinGenData data
    ) {
        int randomNum = random.Next(0, 100);
        if (randomNum <= data.OreBlockChance) {
            return OreBlockType.Block;
        } else {
            return OreBlockType.Cluster;
        }
    }

    void SpawnOre(
        Tilemap tilemap,
        System.Random random,
        Vector3Int position,
        OreVeinGenData data
    ) {
        OreBlockType oreBlockType = GetRandomOreBlockType(random, data);
        TileBase tile = (oreBlockType == OreBlockType.Block)? data.OreBlockTile: data.ClusterBlockTile;
        tilemap.SetTile(position, tile);
    }

    List<Vector3Int> GetAdjacentList(
        System.Random random
    ) {
        List<Vector3Int> adjacentList = new() {
            [0] = new(-1, 0),
            [1] = new(1, 0),
            [2] = new(0, -1),
            [3] = new(0, 1)
        };

        int n = adjacentList.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (adjacentList[n], adjacentList[k]) = (adjacentList[k], adjacentList[n]);
        }

        return adjacentList;
    }

    void AddNeighbourPosition(
        int width,
        int height,
        System.Random random,
        Vector3Int position,
        List<Vector3Int> spawnPositions
    ) {
        List<Vector3Int> adjacentList = GetAdjacentList(random);
        foreach (Vector3Int adjacentVector in adjacentList) {
            Vector3Int adjacentPosition = position + adjacentVector;
            if (adjacentPosition.x < 0 && adjacentPosition.x >= width) continue;
            if (adjacentPosition.y < 0 && adjacentPosition.y >= height) continue;
            spawnPositions.Add(adjacentPosition);
        }
    }

    void SpawnOreVein(
        int width,
        int height,
        Tilemap tilemap,
        System.Random random,
        OreVeinGenData data
    ) {
        int chance = data.InitialChance;
        List<Vector3Int> spawnPositions = new();

        // Initial Block
        int randomNum = random.Next(0, 100);

        if (randomNum <= chance) {
            Vector3Int initialPosition = GetRandomPosition(width, height, tilemap, random, data);
            SpawnOre(tilemap, random, initialPosition, data);

            chance -= data.ChanceDepletionRate;
            AddNeighbourPosition(width, height, random, initialPosition, spawnPositions);
        } else return;


        foreach (Vector3Int position in spawnPositions) {
            randomNum = random.Next(0, 100);

            if (randomNum <= chance) {
                SpawnOre(tilemap, random, position, data);
                chance -= data.ChanceDepletionRate;
                AddNeighbourPosition(width, height, random, position, spawnPositions);
            } else return;
        }
    }

    public void SpawnOreVeins(
        int width,
        int height,
        Tilemap tilemap,
        System.Random random,
        OreVeinGenData data       
    )
    {
        int veinAmount = random.Next(data.MinVeinAmount, data.MaxVeinAmount);
        for (int i = 0; i < veinAmount; i++) {
            SpawnOreVein(width, height, tilemap, random, data);
        }
    }
}

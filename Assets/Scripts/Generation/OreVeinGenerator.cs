using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

enum OreBlockType {Block, Cluster}

public class OreVeinGenerator {
    
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
                random.Next(height - data.MaxYHeight, height - data.MinYHeight),
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
            new Vector3Int(-1, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(0, 1, 0)
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

    void GenerateOreVein(
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

        for (int i = 0; i < spawnPositions.Count; i++) {
            Vector3Int position = spawnPositions[i];
            randomNum = random.Next(0, 100);

            if (randomNum <= chance) {
                SpawnOre(tilemap, random, position, data);
                chance -= data.ChanceDepletionRate;
                AddNeighbourPosition(width, height, random, position, spawnPositions);
            } else break;
        }
    }

    public void GenerateOreVeins(
        int width,
        int height,
        System.Random random,
        Tilemap tilemap,
        List<OreVeinGenData> datas
    ) {
        foreach (OreVeinGenData data in datas) {
            if (data.MinYHeight > height) continue;

            int veinAmount = random.Next(data.MinVeinAmount, data.MaxVeinAmount);
            for (int i = 0; i < veinAmount; i++) {
                GenerateOreVein(width, height, tilemap, random, data);
            }               
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum LightCrystalSize {Small, Medium, Large}

public class LightCrystalGenerator : MonoBehaviour {

    //================================================================================================//
    //================================================================================================//

    public int LightCrystalAmount;
    public DoorGenerator DoorGenerator;
    public List<LightCrystal> LightCrystalDatas;
    public TileBase WoodenPlatformTile;

    List<Vector3Int> _lightCrystalPositions = new();

    //================================================================================================//
    //================================================================================================//

    Vector3Int GetRandomPosition(
        int width,
        int height,
        System.Random random
    ) {
        int randomX = random.Next(0, width - 1);
        int randomY = random.Next(DoorGenerator.MinYExitFromBottom, height - DoorGenerator.MinYEntrance);
        return new(randomX, randomY);
    }

    LightCrystal GetRandomSize(
        System.Random random
    ) {
        foreach (LightCrystal lightCrystal in LightCrystalDatas){
            int randomNum = random.Next(0, 100);
            if (lightCrystal.Chance <= randomNum) return lightCrystal;
        }
        return LightCrystalDatas[0];
    }
    
    void GenerateLightCrystal(
        int width,
        int height,
        System.Random random,
        Tilemap tilemap
    ) {
        Vector3Int cellPosition = GetRandomPosition(width, height, random);
        LightCrystal lightCrystalData = GetRandomSize(random);
        _lightCrystalPositions.Add(cellPosition);

        TileBase currentTile = tilemap.GetTile(cellPosition);
        TileBase currentTileUnderneath = tilemap.GetTile(cellPosition - new Vector3Int(0, -1));

        if (currentTileUnderneath != null && currentTile == null) {
            tilemap.SetTile(cellPosition, lightCrystalData.Tile);
            return;
        } else if (currentTileUnderneath == null) {
            for (int x = cellPosition.x - 1; x <= cellPosition.x + 1; x++) {
                for (int y = cellPosition.y - 1; y <= cellPosition.y + 1; y++) {
                    if (y == cellPosition.y - 1) {
                        tilemap.SetTile(new(x, y), WoodenPlatformTile);
                    } else tilemap.SetTile(new(x, y), null);
                }
            }
        } else {
            for (int x = cellPosition.x - 1; x <= cellPosition.x + 1; x++) {
                for (int y = cellPosition.y + 1; y >= cellPosition.y; y--) {
                    tilemap.SetTile(new(x, y), null);
                }
            }
        }

        tilemap.SetTile(cellPosition, lightCrystalData.Tile);
    }

    void UpdateWoodenPlatforms(
        Tilemap tilemap
    ) {
        foreach (Vector3Int cellPosition in _lightCrystalPositions) {
            Vector3Int positionUnderneath = cellPosition + new Vector3Int(0, -1);
            TileBase currentTileUnderneath = tilemap.GetTile(positionUnderneath);
            if (currentTileUnderneath == null) tilemap.SetTile(positionUnderneath, WoodenPlatformTile);
        }
    }

    public void GenerateAllLightCrystals(
        int width,
        int height,
        System.Random random,
        Tilemap tilemap
    ) {
        for (int i = 0; i < LightCrystalAmount; i++) {
            GenerateLightCrystal(width, height, random, tilemap);
        }  
        UpdateWoodenPlatforms(tilemap);
    }

    //================================================================================================//
    //================================================================================================//
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileData {
    public int Health;
    public int MaxHealth;
}

public class BlockService : MonoBehaviour {
    
    //================================================================================================//
    //================================================================================================//
    
    public Tilemap Tilemap;
    public List<BlockData> BlockDatas;
    public Pickaxe Pickaxe;

    public LightCrystalGenerator LightCrystalGenerator;
    public LightService LightService;

    public Tilemap MiningIndicatorTilemap;
    public int MiningDistance = 3;
    public TileBase CanMineTile;
    public TileBase CannotMineTile;
    
    Dictionary<Vector3Int, TileData> _tileDatas = new();
    Dictionary<TileBase, BlockData> _cachedBlockDatas = new();

    float _mineRepeatRate = 0.5f;
    float _timer = 0f;

    //================================================================================================//
    //================================================================================================//

    void Update() {
        UpdateCursor();
        if (Mouse.current.leftButton.isPressed) {
            _timer += Time.deltaTime;
            Pickaxe.StartSwinging();

            if (_timer >= _mineRepeatRate) {
                MineBlock();
                _timer = 0f;
            }
        } else {
            Pickaxe.StopSwinging();
            _timer = 0f;
        }
    }
    
    //================================================================================================//
    //================================================================================================//

    void UpdateCursor() {
        Vector3Int cellPosition = GetCellPositionOnMouse();
        MiningIndicatorTilemap.ClearAllTiles();
        if (Tilemap.GetTile(cellPosition) == null) return;

        bool isInDistance = IsBlockInDistance(cellPosition);
        MiningIndicatorTilemap.SetTile(cellPosition, isInDistance? CanMineTile: CannotMineTile);
    }

    bool IsBlockInDistance(Vector3Int cellPosition) {
        Vector3Int playerCellPosition = Tilemap.WorldToCell(Pickaxe.transform.position);
        if (Mathf.Abs(cellPosition.x - playerCellPosition.x) > MiningDistance) return false;
        if (Mathf.Abs(cellPosition.y - playerCellPosition.y) > MiningDistance) return false;
        return true;
    }

    Vector3Int GetCellPositionOnMouse() {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int cellPosition = Tilemap.WorldToCell(worldPosition);
        return cellPosition;
    }

    void MineBlock() {
        Vector3Int cellPosition = GetCellPositionOnMouse();
        if (Tilemap.GetTile(cellPosition) == null) return;

        if (IsALightCrystal(Tilemap.GetTile(cellPosition))) {
            MineLightCrystal(cellPosition);
            return;    
        }

        bool isTileDataExist = _tileDatas.ContainsKey(cellPosition);
        TileData tileData;

        if (isTileDataExist) {
            tileData = _tileDatas[cellPosition];
            tileData.Health -= 1;
        } else {
            TileBase tile = Tilemap.GetTile(cellPosition);
            tileData = new() {
                Health = GetBlockDataFromTile(tile).MaxHealth[(int) Pickaxe.Material] - 1,
                MaxHealth = GetBlockDataFromTile(tile).MaxHealth[(int) Pickaxe.Material],
            };
            _tileDatas[cellPosition] = tileData;
        }

        if (tileData.Health <= 0) {
            Tilemap.SetTile(cellPosition, null);
        }
    }

    bool IsALightCrystal(TileBase tile) {
        foreach (LightCrystal lightCrystal in LightCrystalGenerator.LightCrystalDatas) {
            if (tile != lightCrystal.Tile) continue;
            return true;
        }
        return false;
    }

    void MineLightCrystal(Vector3Int cellPosition) {
        TileBase tile = Tilemap.GetTile(cellPosition);

        foreach (LightCrystal lightCrystal in LightCrystalGenerator.LightCrystalDatas) {
            if (lightCrystal.Tile != tile) continue;
            Tilemap.SetTile(cellPosition, null);
            LightService.AdjustFuel(lightCrystal.Fuel);
        }
    }

    BlockData GetBlockDataFromTile(TileBase tile) {
        if (_cachedBlockDatas.ContainsKey(tile)) return _cachedBlockDatas[tile];

        foreach (BlockData data in BlockDatas) {
            if (data.Tile != tile) continue;
            return data;
        }
        return BlockDatas[0];
    }

    //================================================================================================//
    //================================================================================================//
}

using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorGenerator : MonoBehaviour
{

    public int MinYEntrance;
    public int MaxYEntrance;
    public int MinYExitFromBottom;
    public int MaxYExitFromBottom;

    public TileBase EntranceTile;
    public TileBase PlatformTile;
    public TileBase ExitTile;

    Vector3Int GenerateDoor(
        int width,
        int minYPos,
        int maxYPos,
        Tilemap tilemap,
        TileBase tile,
        System.Random random
    ) {
        int randomXPos = random.Next(5, width - 6);
        int randomYPos = random.Next(minYPos, maxYPos);
        Vector3Int position = new(randomXPos, randomYPos);
        tilemap.SetTile(position, tile);

        for (int x = position.x - 2; x <= position.x + 2; x++) {
            for (int y = position.y - 2; y <= position.y + 2; y++) {
                Vector3Int cellPosition = new(x, y);

                if (x != position.x - 2 && x != position.x + 2 && y == position.y - 1) {
                    tilemap.SetTile(cellPosition, PlatformTile);
                } else if (x != position.x || y != position.y) {
                    tilemap.SetTile(cellPosition, null);
                }
            }
        }

        return position;
    }

    public (Vector3Int, Vector3Int) GenerateDoors(
        int width,
        int height,
        Tilemap tilemap,
        System.Random random
    ) {
        Vector3Int entrancePosition = GenerateDoor(
            width, 
            height - MaxYEntrance, height - MinYEntrance, 
            tilemap, EntranceTile, random
        );
        Vector3Int exitPosition = GenerateDoor(
            width, 
            MinYExitFromBottom, MaxYExitFromBottom, 
            tilemap, ExitTile, random
        );
        return (entrancePosition, exitPosition);
    }
}

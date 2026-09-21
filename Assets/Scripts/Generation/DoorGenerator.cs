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

    public Vector3Int GenerateEntrance(
        int width,
        int height,
        Tilemap tilemap,
        System.Random random
    ) {
        int randomXPos = random.Next(5, width - 6);
        int randomYPos = random.Next(height - MinYEntrance, height - MaxYEntrance);
        Vector3Int entrancePosition = new(randomXPos, randomYPos);
        tilemap.SetTile(entrancePosition, EntranceTile);

        for (int x = entrancePosition.x - 2; x <= entrancePosition.x + 2; x++) {
            for (int y = entrancePosition.y - 2; y <= entrancePosition.y + 2; y++) {
                Vector3Int cellPosition = new(x, y);

                if (x != entrancePosition.x - 2 && x != entrancePosition.x + 2 && y == entrancePosition.y - 1) {
                    tilemap.SetTile(cellPosition, PlatformTile);
                } else if (x != entrancePosition.x && y != entrancePosition.y) {
                    tilemap.SetTile(cellPosition, null);
                }
            }
        }

        return entrancePosition;
    }

}

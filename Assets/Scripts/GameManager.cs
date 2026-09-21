using UnityEngine;

public class GameManager : MonoBehaviour
{

    public MapGenerator MapGenerator;
    public Player player;

    void StartRound()
    {
        MapGenerator.GenerateCave();
        player.Initialize(MapGenerator.EntrancePosition);
    }

    void Start()
    {
        StartRound();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public static Generation Instance;


    [Header("RoomConfiguration")]
    [Space]
    public int mapWidth = 7;
    public int mapHeight = 7;
    public int roomsToGenerate = 12;

    [Space]
    [Header("SpawnChances")]
    [Space]

    public float enemySpawnChance;
    public float coinSpawnChance;
    public float healthSpawnChance;

    [Space]

    public int maxEnemiesPerRoom;
    public int maxCoinsPerRoom;
    public int maxHealthPerRoom;

    private int roomCount;
    private bool roomsInstantiated;

    private Vector2 firstRoomPos;

    // A 2D boolean array to map out the level
    private bool[,] map;
    // the room prefab to instantiate 
    public GameObject roomPrefab;

    private List<Room> roomObjects = new List<Room>();
    private int randomSeed;



    private void Awake()
    {
        Instance = this;
    }



    public void OnPlayerMove()
    {
        Vector3 playerPos = FindObjectOfType<Player>().transform.position;
        Vector2 roomPos = new Vector2(((int)playerPos.x + 6) / 12, ((int)playerPos.y + 6) / 12);

        UI.instance.miniMap.texture = MapTextureGenerator.Generate(map, roomPos);
    }

    public void Generate()
    {
        map = new bool[mapWidth, mapHeight];
        CheckRoom(3,3, 0, Vector2.zero, true);
        InstantiateRooms();


        // Find the player in the scene, and position them inside the first room.    

        FindObjectOfType<Player>().transform.position = firstRoomPos * 12;

        UI.instance.miniMap.texture = MapTextureGenerator.Generate(map, firstRoomPos);
    }

    // check to see if we can place a room in the center of the map. Since this is the first room, there will be no branch nor a general direction. 
    void CheckRoom (int x, int y, int remaining, Vector2 generalDirection, bool firstRoom = false)
    {
        if (roomCount >= roomsToGenerate)
        {
            return;
        }

        if (x <0 || x > mapWidth -1 || y <0 || y > mapHeight - 1)
        {
            return;
        }

        if (firstRoom == false && remaining <= 0)
        {
            return;
        }

        if (map[x,y] == true)
        {
            return;
        }

        if (firstRoom == true)
        {
            firstRoomPos = new Vector2(x,y);
        }

        roomCount++;
        map[x,y] = true;

        bool north = Random.value > (generalDirection == Vector2.up ? 0.2f : 0.8f);

        bool south = Random.value > (generalDirection == Vector2.down ? .2f : .8f) ;

        bool east = Random.value > (generalDirection == Vector2.right ? .2f : .8f);

        bool west = Random.value > (generalDirection == Vector2.left ? .2f : .8f );

        int maxRemaining = roomsToGenerate / 4;

        if (north || firstRoom)
        {
            CheckRoom(x, y + 1, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.up : generalDirection);
        }

        if (south || firstRoom)
        {
            CheckRoom(x, y - 1, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.down : generalDirection);
        }

        if (east || firstRoom)
        {
            CheckRoom(x + 1, y, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.right : generalDirection);
        }

        if (west || firstRoom)
        {
            CheckRoom(x -1, y, firstRoom ? maxRemaining : remaining - 1, firstRoom ? Vector2.left : generalDirection);
        }

        
    }

    void InstantiateRooms()
    {
        if (roomsInstantiated)
        {
            return;
        }

        roomsInstantiated = true;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (map[x, y] == false)
                {
                    continue;
                }

                    GameObject roomObject = Instantiate (roomPrefab, new Vector3(x,y,0) * 12, Quaternion.identity);
                    Room room = roomObject.GetComponent<Room>();

                if (y < mapHeight -1 && map[x, y +1] == true)
                {
                    room.GetNorthWall().gameObject.SetActive(false);
                    room.GetNorthDoor().gameObject.SetActive(true);
                }

                if (y > 0 && map[x, y -1 ] == true)
                {
                    room.GetSoutWall().gameObject.SetActive(false);
                    room.GetSouthDoor().gameObject.SetActive(true);
                }

                if (x < mapWidth && map[x +1, y] == true)
                {
                    room.GetEastDoor().gameObject.SetActive(true);
                    room.GetEastWall().gameObject.SetActive(false);
                }

                if (x > 0 && map[x -1, y ] == true)
                {
                    room.GetWestDoor().gameObject.SetActive(true);
                    room.GetWestWall().gameObject.SetActive(false);
                }

                if (firstRoomPos != new Vector2(x, y))
                {
                    room.GenerateInterior();
                }

                roomObjects.Add(room);

            }
        }

        CalculateKeyAndExit();
    }

    void CalculateKeyAndExit()
    {
        float maxDist = 0;
        Room a = null;
        Room b = null;

        foreach (Room aRoom in roomObjects)
        {
            foreach (Room bRoom in roomObjects)
            {
                float dist = Vector3.Distance(aRoom.transform.position, bRoom.transform.position);

                if (dist > maxDist)
                {
                    a = aRoom;
                    b = bRoom;
                    maxDist = dist;
                }
            }
        }

        a.SpawnPrefab(a.KeyPrefabInstantiate());
        b.SpawnPrefab(b.ExitPrefabInstantiate());
    }
}

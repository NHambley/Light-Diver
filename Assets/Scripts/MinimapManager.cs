using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour {

    public GameObject room;
    private int[,] map;
    private GameObject[,] mapObj;
    private Vector2 curRoom;
    public Vector2 CurrentRoom
    {
        get { return curRoom; }
    }

    public PlayerController player;

    public void PopulateMinimap(int[,] mapGet)
    {
        this.map = mapGet;
        mapObj = new GameObject[Constants.MAP_DEFAULT_WIDTH, Constants.MAP_DEFAULT_HEIGHT];

        float minimapWidth = this.GetComponent<Renderer>().bounds.size.x;
        Vector3 minimapPos = this.transform.position;

        for (int i = 0; i < Constants.MAP_DEFAULT_HEIGHT; i++)
        {
            for (int j = 0; j < Constants.MAP_DEFAULT_WIDTH; j++)
            {
                if (map[j, i] == 1)
                {
                    Vector3 roomPos = new Vector3(
                        (minimapPos.x - minimapWidth / 2) + (j * minimapWidth / Constants.MAP_DEFAULT_WIDTH) + ((minimapWidth / Constants.MAP_DEFAULT_WIDTH) / 2), 
                        (minimapPos.y + minimapWidth / 2) - (i * minimapWidth / Constants.MAP_DEFAULT_HEIGHT) - ((minimapWidth / Constants.MAP_DEFAULT_HEIGHT) / 2),
                        Constants.MINIMAP_Z_POS);
                    GameObject newRoom = Instantiate(room, this.transform);
                    newRoom.transform.position = roomPos;

                    mapObj[j, i] = newRoom;
                }
            }
        }
    }

    public void UpdateMinimapObj(int indexX, int indexY, int cur)
    {
        this.curRoom = new Vector2(indexX, indexY);
        this.player.SetRoomPos(this.curRoom);
        mapObj[indexX, indexY].GetComponent<MinimapIcon>().UpdateIcon(cur);
    }
}

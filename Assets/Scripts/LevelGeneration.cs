using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

    public int minSize, maxSize;

    public GameObject minimap;
    public GameObject background;
    public GameObject backdrop;

    private int[,] map;
    private int maxDepth;
    private int startingPosition;
    [HideInInspector]
    public List<GameObject> walls;

    public GameObject[] enemies;
    public GameObject[] bosses;
    public float[] enemySpawnChance;

    public int maxEnemyPerRoom;

    // Use this for initialization
    void Start()
    {
        map = new int[Constants.MAP_DEFAULT_WIDTH, Constants.MAP_DEFAULT_HEIGHT];

        this.GenerateLevel();
        this.SetupObjects();

        this.minimap.GetComponent<MinimapManager>().PopulateMinimap(map);

        walls = new List<GameObject>();

    }

    private void GenerateLevel()
    {

        int mapSize = Random.Range(minSize, maxSize);
        int depthLevel = Mathf.FloorToInt(mapSize * Constants.GENERATION_DEPTH_MODIFIER);
        List<int> tilesForLayer = new List<int>();
        this.maxDepth = depthLevel;
#if UNITY_EDITOR
        Debug.Log("Map Size: " + mapSize);
        Debug.Log("Depth Level: " + depthLevel);
#endif

        for (int i = 0; i < depthLevel - 1; i++)
        {
            float percentTake = Random.Range(0.4f, 0.6f);
            int take = Mathf.FloorToInt(mapSize * percentTake);
            if (take > 10)
            {
                take = 10;
            }

            mapSize -= take;
            tilesForLayer.Add(take);
        }

        if (mapSize <= 10)
        {
            tilesForLayer.Add(mapSize);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning("SOMETHING HAS GONE WRONG WITH WORLD GENERATION!!!");
        }
#endif


        int startingPos = Random.Range(0, Constants.MAP_DEFAULT_WIDTH);
        this.startingPosition = startingPos;
        int leftMost = startingPos - 1, rightMost = startingPos + 1;

        this.map[startingPos, 0] = 1;

        for (int i = 0; i < depthLevel; i++)
        {
            for (int j = 0; j < tilesForLayer[i] - 1; j++)
            {
                byte pickRight = (byte)Random.Range(0, 1);
                if ((pickRight == 0 && rightMost < Constants.MAP_DEFAULT_WIDTH) || leftMost < 0)
                {
                    this.map[rightMost, i] = 1;
                    rightMost++;
                }
                else
                {
                    this.map[leftMost, i] = 1;
                    leftMost--;
                }
            }
            int startPos = Random.Range(leftMost+1, rightMost-1);
            leftMost = startPos - 1;
            rightMost = startPos + 1;
            this.map[startPos, i + 1] = 1;
        }
    }

    private void SpawnEnemies(Vector2 pos, float width, float height, int indexX, int indexY)
    {
        int numToSpawn = Random.Range(0, this.maxEnemyPerRoom);

        for (int i = 0; i < numToSpawn; i++)
        {
            float x = Random.Range(pos.x - width/4, pos.x + width/4);
            float y = Random.Range(pos.y - height/4, pos.y + height/4);
            Vector2 enemyPos = new Vector2(x, y);

            GameObject enemyPick=null;
            float ranChance = Random.Range(0f, 1f);

            for (int j = 0; j < this.enemySpawnChance.Length; j++)
            {
                if (ranChance <= this.enemySpawnChance[j])
                {
                    enemyPick = this.enemies[j];
                }
            }
            if (enemyPick == null)
            {
                Debug.LogWarning("Enemy spawn chance not set up correctly, defaulting to basic enemy");
                enemyPick = this.enemies[0];
            }

            GameObject enemy = Instantiate(enemyPick);
            enemy.transform.position = enemyPos;
            enemy.GetComponent<EnemyInfo>().SetUp(indexX, indexY);
        }
        
        if (indexY == this.maxDepth)
        {
            int choice = Random.Range(0, this.bosses.Length - 1);
            GameObject boss = Instantiate(bosses[choice]);
            boss.transform.position = pos;
            boss.GetComponent<EnemyInfo>().SetUp(indexX, indexY);
        }
    }

    private void SetupObjects()
    {
        float backgroundWidth = this.background.GetComponent<Renderer>().bounds.size.x;
        float backgroundHeight = this.background.GetComponent<Renderer>().bounds.size.y;
        GameObject level = GameObject.Find(Constants.GAMEOBJECT_LEVEL);

        float startingPosXOffset = backgroundWidth * startingPosition;

        for (int i = 0; i < Constants.MAP_DEFAULT_HEIGHT; i++)
        {
            for (int j = 0; j < Constants.MAP_DEFAULT_WIDTH; j++)
            {
                if (map[j,i] == 1)
                {
                    GameObject levelPiece = Instantiate(this.background, level.transform);

                    Vector3 pos = new Vector3((j * backgroundWidth) - startingPosXOffset,
                                              -(i * backgroundHeight),
                                              Constants.LEVEL_Z_POS);

                    levelPiece.transform.position = pos;
                    levelPiece.GetComponent<LevelPieceInfo>().SetUp(j, i);

                    //top door open
                    if (i > 0 && map[j,i-1] == 1)
                    {
                        levelPiece.transform.Find(Constants.BACKGROUND_TOP_DOOR).gameObject.SetActive(false);
                    }
                    //bottom door open
                    if (i < Constants.MAP_DEFAULT_HEIGHT-1 && map[j, i + 1] == 1)
                    {
                        levelPiece.transform.Find(Constants.BACKGROUND_BOTTOM_DOOR).gameObject.SetActive(false);
                    }
                    //left door open
                    if (j > 0 && map[j-1,i] == 1)
                    {
                        levelPiece.transform.Find(Constants.BACKGROUND_LEFT_DOOR).gameObject.SetActive(false);
                    }
                    //right door open
                    if (j < Constants.MAP_DEFAULT_WIDTH - 1 && map[j + 1, i] == 1)
                    {
                        levelPiece.transform.Find(Constants.BACKGROUND_RIGHT_DOOR).gameObject.SetActive(false);
                    }

                    //spawn enemies
                    if (!(i == 0 && j == this.startingPosition))
                    {
                        this.SpawnEnemies(pos, backgroundWidth, backgroundHeight, j, i);
                    }

                    //spawn in backtiles
                    if (i == 0 || i >0 && map[j,i-1] == 0)
                    {
                        GameObject backdropObj = Instantiate(this.backdrop, level.transform);
                        Vector3 backdropPos = new Vector3(((j) * backgroundWidth) - startingPosXOffset,
                                              -((i-1) * backgroundHeight),
                                              Constants.LEVEL_Z_POS);

                        backdropObj.transform.position = backdropPos;
                    }
                    if (i == Constants.MAP_DEFAULT_HEIGHT-1 || i < Constants.MAP_DEFAULT_HEIGHT-1 && map[j, i + 1] == 0)
                    {
                        GameObject backdropObj = Instantiate(this.backdrop, level.transform);
                        Vector3 backdropPos = new Vector3(((j) * backgroundWidth) - startingPosXOffset,
                                              -((i + 1) * backgroundHeight),
                                              Constants.LEVEL_Z_POS);

                        backdropObj.transform.position = backdropPos;
                    }
                    if (j == 0 || j > 0 && map[j-1, i] == 0)
                    {
                        GameObject backdropObj = Instantiate(this.backdrop, level.transform);
                        Vector3 backdropPos = new Vector3(((j-1) * backgroundWidth) - startingPosXOffset,
                                              -((i) * backgroundHeight),
                                              Constants.LEVEL_Z_POS);

                        backdropObj.transform.position = backdropPos;
                    }
                    if (j == Constants.MAP_DEFAULT_HEIGHT - 1 || j < Constants.MAP_DEFAULT_HEIGHT - 1 && map[j+1, i] == 0)
                    {
                        GameObject backdropObj = Instantiate(this.backdrop, level.transform);
                        Vector3 backdropPos = new Vector3(((j + 1) * backgroundWidth) - startingPosXOffset,
                                              -((i) * backgroundHeight),
                                              Constants.LEVEL_Z_POS);

                        backdropObj.transform.position = backdropPos;
                    }
                    if ((j == Constants.MAP_DEFAULT_HEIGHT - 1 || j < Constants.MAP_DEFAULT_HEIGHT - 1 && map[j + 1, i] == 0) &&
                        (i == 0 || i > 0 && map[j, i - 1] == 0))
                    {
                        GameObject backdropObj = Instantiate(this.backdrop, level.transform);
                        Vector3 backdropPos = new Vector3(((j + 1) * backgroundWidth) - startingPosXOffset,
                                              -((i - 1) * backgroundHeight),
                                              Constants.LEVEL_Z_POS);

                        backdropObj.transform.position = backdropPos;
                    }
                    if ((j == 0 || j > 0 && map[j - 1, i] == 0) &&
                        (i == 0 || i > 0 && map[j, i - 1] == 0))
                    {
                        GameObject backdropObj = Instantiate(this.backdrop, level.transform);
                        Vector3 backdropPos = new Vector3(((j - 1) * backgroundWidth) - startingPosXOffset,
                                              -((i - 1) * backgroundHeight),
                                              Constants.LEVEL_Z_POS);

                        backdropObj.transform.position = backdropPos;
                    }
                    if ((j == 0 || j > 0 && map[j - 1, i] == 0) &&
                       (i == Constants.MAP_DEFAULT_HEIGHT - 1 || i < Constants.MAP_DEFAULT_HEIGHT - 1 && map[j, i + 1] == 0))
                    {
                        GameObject backdropObj = Instantiate(this.backdrop, level.transform);
                        Vector3 backdropPos = new Vector3(((j - 1) * backgroundWidth) - startingPosXOffset,
                                              -((i + 1) * backgroundHeight),
                                              Constants.LEVEL_Z_POS);

                        backdropObj.transform.position = backdropPos;
                    }
                    if ((j == Constants.MAP_DEFAULT_HEIGHT - 1 || j < Constants.MAP_DEFAULT_HEIGHT - 1 && map[j + 1, i] == 0) &&
                        (i == Constants.MAP_DEFAULT_HEIGHT - 1 || i < Constants.MAP_DEFAULT_HEIGHT - 1 && map[j, i + 1] == 0))
                    {
                        GameObject backdropObj = Instantiate(this.backdrop, level.transform);
                        Vector3 backdropPos = new Vector3(((j + 1) * backgroundWidth) - startingPosXOffset,
                                              -((i + 1) * backgroundHeight),
                                              Constants.LEVEL_Z_POS);

                        backdropObj.transform.position = backdropPos;
                    }

                    foreach (Transform child in levelPiece.transform)
                    {
                        if(child.gameObject.activeInHierarchy != false)
                        {
                            walls.Add(child.gameObject);
                        }
                    }
                }
            }
        }

    }
}

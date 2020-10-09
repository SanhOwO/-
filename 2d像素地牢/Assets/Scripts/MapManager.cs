using GameEngine.Instance;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //长宽
    public int cols = 8;
    public int rows = 8;
    //障碍物,食物,敌人
    public int minWallCount = 5;
    public int maxWallCount = 9;
    public int minFoodCount = 1;
    public int maxFoodCount = 5;
    public int maxEnemyCount = 3;
    //各个部分组件
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;//储存地图里固定的物体的父级的Transform
    private List<Vector3> gridPosition = new List<Vector3>();//用来克隆敌人食物障碍物位置
    private GameObject dynamicObject;//每关需要重新生成的物体父级

    private void Start()
    {
        boardHolder = new GameObject("Board").transform;
        dynamicObject = new GameObject("DynamicObj");
    }
    void Update()
    {

    }
    public void InitScene()
    {
        Debug.Log("Init");
        SenceSetup();
        InitGridPos();

    }
    public void ResetScene(int lv)
    {


        LayoutObjectAtRandom(wallTiles, minWallCount, maxWallCount);
        LayoutObjectAtRandom(foodTiles, minFoodCount, maxFoodCount);
        int enemyCount = Mathf.Clamp(lv, 0, maxEnemyCount);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
    }
    //制造场地
    private void SenceSetup()
    {
        //制作地板和边墙,-1 +1是边墙
        for (int i = -1; i < cols + 1; i++)
        {
            for (int j = -1; j < rows + 1; j++)
            {
                if (i == -1 || i == cols || j == -1 || j == rows)
                {
                    GameObject tempOuterWall = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    var outerWall = GameObject.Instantiate(tempOuterWall, new Vector3(i, j, 0), Quaternion.identity);
                    outerWall.transform.SetParent(boardHolder);
                }
                else
                {
                    GameObject tempFloor = floorTiles[Random.Range(0, floorTiles.Length)];
                    var floor = GameObject.Instantiate(tempFloor, new Vector3(i, j, 0), Quaternion.identity);
                    floor.transform.SetParent(boardHolder);
                }
            }
        }
        LayoutObjectAtRandom(wallTiles, minWallCount, maxWallCount);
        LayoutObjectAtRandom(foodTiles, minFoodCount, maxFoodCount);
    }
    private void InitGridPos()
    {
        gridPosition.Clear();
        for (int x = 1; x < cols - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPosition.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //根据最小数量和最大数量确认这一关要生成的个数
        int objectCount = 0;
        if (minimum == maximum)
        {
            objectCount = minimum;
        }
        else
        {
            objectCount = Random.Range(minimum, maximum + 1);
        }

        for (int i = 0; i < objectCount; i++)
        {
            //随机取出一个位置
            Vector3 randomPos = RandomPosition();
            //随机取得要克隆的物体
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            //实例化他
            GameObject.Instantiate(tileChoice, randomPos, Quaternion.identity).transform.SetParent(dynamicObject.transform);
        }
    }
    /// <summary>
    /// 随机从List里面取出一个位置使用，主要给后面随机生成障碍物和食物用的
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomPosition()
    {
        //获取一个随机的索引值
        int randomIndex = Random.Range(0, gridPosition.Count);
        //通过索引取到位置信息
        Vector3 randomPos = gridPosition[randomIndex];
        //把用了的位置移除掉
        gridPosition.RemoveAt(randomIndex);
        //返回取到的位置
        return randomPos;
    }
}

using GameEngine.Instance;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Start,
    Pause,
    Stop
}
public class GameManager : MonoBehaviour
{
    private int level = 0;

    Button button;
    Image bg;
    bool isInitScene;//是否需要初始化场景
    Text levelText;
    GameState state;
    List<Enemy> enemies = new List<Enemy>();
    Player player;

    public GameObject playerPrefab;
    public bool PlayersTurn; //玩家回合
    public bool canEnemyMove;//怪物是否可以移动
    public float turnDelay = 0.1f;//移动时间
    void Start()
    {
        bg = GameObject.Find("BG").GetComponent<Image>();
        button = GameObject.Find("StartBtn").GetComponent<Button>();
        levelText = bg.transform.Find("Date").GetComponent<Text>();
        state = GameState.Stop;
        button.onClick.AddListener(StartGame);
    }
    private void StartGame()
    {
        Debug.Log("Start");
        if(state == GameState.Stop)
        {
            MapManager mapMgr = GameObject.Find("MapManager").GetComponent<MapManager>();
            enemies.Clear();
            state = GameState.Pause;
            button.gameObject.SetActive(false);
            Debug.Log(button);
            StartCoroutine(EnterScene());//携程 背景渐渐透明
            PlayersTurn = true;
            canEnemyMove = true;
            if (isInitScene == true)
            {//刚开始, 需要初始化场景(地板,围墙),调用方法,克隆角色
                isInitScene = false;
                //MapManager.Instance.InitScene();
                mapMgr.InitScene();
                player = GameObject.Instantiate(playerPrefab).GetComponent<Player>();
            }
            else //完成一关后, 不需要重置整个场景
                mapMgr.InitScene();
                //MapManager.Instance.ResetScene(level);
        }
    }

    private IEnumerator EnterScene()
    {
        levelText.gameObject.SetActive(true);
        bg.gameObject.SetActive(true);
        bg.CrossFadeAlpha(0, 1, false);

        levelText.text = "第" + (level+1) + "关";
        yield return new WaitForSeconds(1f);
        levelText.gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
        state = GameState.Start;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

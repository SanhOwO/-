using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float force = 4;
    private float stageMaxDistance = 2;

    public GameObject initStage;
    public GameObject[] stageList;
    public Text ScoreText;
    public GameObject particle;
    public Transform head;
    public Transform body;
    public GameObject gameOver;
    public Text finalPoint;
    public Button reStartButton;
    public AudioSource audioSource;
    public AudioClip successAudio, addForceAudio, startAudio, fallAudio;
    public Rigidbody rig;
    public int point;

    //玩家输入
    private bool enableInput = true;
    private float startAddForceTime; //按下蓄力的开始时间

    private int lastPoint = 1; //上一次得分


    //台阶设定
    private GameObject currentStage;
    private Vector3 stageInitPos;
    private Vector3 stageInitScale;
    private Transform nextStage;
    //相机相对位置
    private Vector3 cameraRelatePosition;

    //生成台阶方向 上或右
    Vector3[] directions = new Vector3[] { new Vector3(-1,0,0), new Vector3(0,0,1)};


    void Start()
    {
        currentStage = initStage;
        //相机-当前人物位置 =  相机的相对位置
        cameraRelatePosition = Camera.main.transform.position - transform.position;
        stageInitPos = initStage.transform.position;
        stageInitScale = initStage.transform.localScale;
        reStartButton.onClick.AddListener(() => { SceneManager.LoadScene(0); });
        rig.centerOfMass = Vector3.zero;//设置质量中心(防止翻车)
        SpawnStage();
        PlayAudio(startAudio);
    }
    void Update()
    {
        rig.WakeUp();
        if (enableInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayAudio(addForceAudio);
                startAddForceTime = Time.time;
                particle.SetActive(true);
            }
            if (Input.GetMouseButton(0))//按下每一帧都返回true
            {
                if(currentStage.transform.localScale.y > 0.4f)
                {
                    body.transform.localScale += new Vector3(1.5f, -1f, 1.5f) * 0.05f * Time.deltaTime;
                    head.transform.position += new Vector3(0, -1, 0) * 0.1f * Time.deltaTime;
                    currentStage.transform.localScale += new Vector3(0, -1, 0) * 0.15f * Time.deltaTime;
                    currentStage.transform.position += new Vector3(0, -1, 0) * 0.15f * Time.deltaTime;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log(currentStage);
                GetComponent<AudioSource>().Stop();
                particle.SetActive(false);
                var pressTime = Time.time - startAddForceTime;
                if(pressTime > 1)
                    pressTime = 1;
                Jump(pressTime);
                body.transform.DOScale(0.1f, 0.2f);//插件,0.2秒内便会0.1大小 看player预设参数
                head.transform.DOLocalMoveY(0.29f, 0.2f);
                currentStage.transform.DOScaleY(stageInitScale.y, 0.4f);
                currentStage.transform.DOLocalMoveY(stageInitPos.y, 0.4f);
            }
        }
    }
    private void SpawnStage()
    {
        GameObject stage = stageList[Random.Range(0, stageList.Length)];
        nextStage = GameObject.Instantiate(stage).transform;
        nextStage.transform.position = currentStage.transform.position + directions[Random.Range(0, directions.Length)] * Random.Range(1.2f, stageMaxDistance);
        var randomScale = Random.Range(0.5f, 1);
        nextStage.transform.localScale = new Vector3(randomScale, 0.5f, randomScale);
        nextStage.GetComponent<Renderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f),Random.Range(0, 1f));
    }
    private void Jump(float pressTime)
    {
        enableInput = false;
        Vector3 jumpDirection = (nextStage.position - transform.position).normalized;
        jumpDirection.y = 0;
        Debug.Log(jumpDirection);
        rig.AddForce(new Vector3(0, 5f, 0) + jumpDirection*pressTime*force, ForceMode.Impulse);
        //方向不同 转动角度不同
        transform.DOLocalRotate(new Vector3(-360* jumpDirection.x, 0, -360* jumpDirection.z), 0.6f, RotateMode.LocalAxisAdd);

    }
    private void OnCollisionEnter(Collision collision)
    {
        rig.Sleep();
        Debug.Log(collision.transform.tag);
        if (gameOver.activeInHierarchy)
            return;
        if (!collision.gameObject.CompareTag("Stage"))
        {
            PlayerDie();
            return;
        }
        if(currentStage == collision.transform.parent.gameObject)
        {
            PlayAudio(successAudio);
            MoveCamera();
            enableInput = true;
            return;
        }
        PlayAudio(successAudio);
        MoveCamera();
        AddPoint();
        //Transform centerPoint = collision.transform.Find("CenterPoint");
        currentStage = collision.transform.parent.gameObject;
        SpawnStage();
        enableInput = true;
    }
    private void AddPoint()
    {
        Vector3 hitPoint = transform.position;
        hitPoint.y = currentStage.transform.position.y;
        float targetDistance = Vector3.Distance(hitPoint, currentStage.transform.position);
        if (targetDistance < 0.1f)
            lastPoint *= 2;
        else
            lastPoint = 1;
        point += lastPoint;
        ScoreText.text = "得分: " + point;
    }
    private void MoveCamera()
    {
        //用1秒移动到下一个目标的相对位置
        Camera.main.transform.DOMove(transform.position + cameraRelatePosition, 1);

    }
    private void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    private void PlayerDie()
    {
        Debug.Log("Player die");
        PlayAudio(fallAudio);
        GameOver();
    }
    /// <summary>
    /// 结束游戏
    /// PC 死亡直接调用
    /// 移动端 看完广告调用
    /// </summary>
    private void GameOver()
    {
        enableInput = false;
        finalPoint.text = "最终得分: " + point;
        reStartButton.gameObject.SetActive(true);
        finalPoint.gameObject.SetActive(true);
        gameOver.SetActive(true);
    }
}

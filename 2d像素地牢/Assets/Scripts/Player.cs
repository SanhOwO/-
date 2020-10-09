using GameEngine.Instance;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private int pointPerFood;
    private int pointPerSoda;
    private int wallDamage;
    private Text foodTest;
    private Animator animator;
    private BoxCollider2D collider2D;
    private int food = 100;
    GameManager gameManager;
    public LayerMask layer;
    //public AudioClip[] audioClips = new AudioClip[7];

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = transform.GetComponent<Animator>();
        foodTest = GameObject.Find("Food").GetComponent<Text>();
        foodTest.text = "剩余食物 " + food;
        collider2D = transform.GetComponent<BoxCollider2D>();
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

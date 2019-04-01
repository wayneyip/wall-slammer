using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager instance; 

    public GameObject playerGO;
    
    //UI References
    public GameObject GameOverPanel;

    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;
    public bool isGameOver;

    //Layer Masks
    public LayerMask wallLayer;
    public LayerMask enemyLayer;

    //Map Data
    public float unitLength = 1.0f;



    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Destroyed duplicated GameManager");
            Destroy(gameObject);
            return;
        }
        instance = this;

        OnGameStart.AddListener(_OnGameStart);
        OnGameOver.AddListener(_OnGameOver);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }


    public void ResetGame()
    {
        OnGameStart.Invoke();
    }

    private void StartGame()
    {
        OnGameStart.Invoke();
    }


    private void _OnGameStart()
    {
        if (GameOverPanel != null)
        {
            GameOverPanel.SetActive(false);
        }
    }

    private void _OnGameOver()
    {
        if(GameOverPanel != null)
        {
            GameOverPanel.SetActive(true);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance; 

    public GameObject playerGO;
    public GameObject bossGO;
    public ShakeBehavior shaker;
    private AudioSource playerAudio;
    
    //UI References
    public GameObject gameOverPanel;
    public TextMeshProUGUI failText;
    public TextMeshProUGUI failText2;
  public TextMeshProUGUI successText;
  public TextMeshProUGUI successText2;
  public TextMeshProUGUI timer;

    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;
    public UnityEvent OnReachedGoal;
    public bool isGameOver;
    public bool hasReachedGoal;

    //Layer Masks
    public LayerMask wallLayer;
    public LayerMask breakableLayer;
    public LayerMask enemyLayer;
    public LayerMask spikeLayer;
    public LayerMask witchLayer; 

    private float startTime;

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

        playerAudio = playerGO.GetComponent<AudioSource>();
  }

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && hasReachedGoal)
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex > 4)
            {
                Destroy(GameObject.FindGameObjectWithTag("Music"));
            }
            SceneManager.LoadScene(nextSceneIndex);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    timer.text = GetDisplayTime();
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
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            isGameOver = false;
            hasReachedGoal = false;
            startTime = Time.time;
            timer.gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayMusic();
    }
  }

    private void _OnGameOver()
    {
        if (isGameOver)
        {
            return;
        }

        if(gameOverPanel != null)
        {
            timer.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            if (!hasReachedGoal)
            {
                failText.gameObject.SetActive(true);
                failText2.gameObject.SetActive(true);
                successText.gameObject.SetActive(false);
                successText2.gameObject.SetActive(false);
            }
            else
            {
                failText.gameObject.SetActive(false);
                failText2.gameObject.SetActive(false);
                successText.text = "Your time:\n" + GetDisplayTime();
                successText.gameObject.SetActive(true);
                successText2.gameObject.SetActive(true);
            }

            isGameOver = true;
        }
    }

    public void PlayCollisionEffect()
    {
        shaker.TriggerShake();
        playerAudio.PlayOneShot(playerAudio.clip);
    }

    private string GetDisplayTime()
  {

    float timeDiff = Time.time - startTime;
    int timeSec = (int)timeDiff;
    int timeMillisec = (int)((timeDiff - timeSec) * 100);
    string zero = "";
    if (timeMillisec < 10)
    {
      zero = "0";
    }
    return timeSec + ":" + zero + timeMillisec;
  }

}

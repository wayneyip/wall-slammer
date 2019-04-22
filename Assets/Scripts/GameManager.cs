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
    public ShakeBehavior shaker;
    private AudioSource audioSource;
    
    //UI References
    public GameObject gameOverPanel;
    public TextMeshProUGUI failText;
    public TextMeshProUGUI successText;
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

        audioSource = GetComponent<AudioSource>();
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
            SceneManager.LoadScene(nextSceneIndex);
        }

    if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

        timer.text = "Time: " + (Time.time - startTime);
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
                successText.gameObject.SetActive(false);
            }
            else
            {
                failText.gameObject.SetActive(false);
                successText.text = "Your time: " + (Time.time - startTime);
                successText.gameObject.SetActive(true);
            }

            isGameOver = true;
        }
    }

    public void PlayCollisionEffect()
    {
        shaker.TriggerShake();
        audioSource.PlayOneShot(audioSource.clip);
    }

}

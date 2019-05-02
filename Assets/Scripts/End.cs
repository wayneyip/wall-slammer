using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Return))
      {
        Destroy(GameObject.FindGameObjectWithTag("Music"));
        int nextSceneIndex = 0;
        SceneManager.LoadScene(nextSceneIndex);
      }
    }
}

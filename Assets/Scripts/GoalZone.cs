using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.instance.playerGO)
        {
            GameManager.instance.hasReachedGoal = true;
            GameManager.instance.OnGameOver.Invoke();
        }    
    }

}

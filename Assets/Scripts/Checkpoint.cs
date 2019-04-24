using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public Transform playerCheckpointTransform;
    public Transform bossCheckpointTransform;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == GameManager.instance.playerGO)
        {
            GameManager.instance.playerGO.GetComponent<Player>().initialPosition = playerCheckpointTransform.position;
            GameManager.instance.playerGO.GetComponent<Player>().useGravity = GameManager.instance.playerGO.GetComponent<Player>().allowGravity;
            GameManager.instance.bossGO.GetComponent<Boss>().initialPosition = bossCheckpointTransform.position;
        }

    }
}

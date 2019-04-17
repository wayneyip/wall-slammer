using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    private MeshRenderer mr;
    private Collider myCollider;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        myCollider = GetComponent<Collider>();

        GameManager.instance.OnGameStart.AddListener(_OnGameStart);
        GameManager.instance.OnGameOver.AddListener(_OnGameOver);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Boss>() != null)
        {
            mr.enabled = false;
            myCollider.enabled = false;
            GameManager.instance.PlayCollisionEffect();
        }
    }

    private void _OnGameStart()
    {
        mr.enabled = true;
        myCollider.enabled = true;
    }

    private void _OnGameOver()
    {

    }
}

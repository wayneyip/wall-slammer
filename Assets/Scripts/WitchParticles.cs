using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchParticles : MonoBehaviour
{
    ParticleSystem system; 

    // Start is called before the first frame update
    void Start()
    {
        system = GetComponent<ParticleSystem>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GameManager.instance.playerGO.GetComponent<Player>().isWitchTime())
        {
            system.Play();
        }
    }
}

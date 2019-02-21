using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    public bool isBoxing = false;

    void OnTriggerEnter(Collider other)
    {
        if (isBoxing && other.tag == "Zombie")
        {
            other.GetComponent<AITargetScript>().kill();
        }
    }
}

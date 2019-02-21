using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
   
    Animator animator;
    private HitEnemy enemyScript;
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyScript = GetComponentInChildren<HitEnemy>();
        if (enemyScript != null)
        {
            enemyScript.isBoxing = false;
        }
    }


    void Update()
    {
        bool boxing = Input.GetKey(KeyCode.B);
        animator.SetBool("isBoxing", boxing);
        if (enemyScript != null)
        {
            enemyScript.isBoxing = boxing;
        }
    }

    
}




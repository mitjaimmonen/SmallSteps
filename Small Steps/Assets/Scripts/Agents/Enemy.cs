using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    PathNavigator pathFinder;
    Animator animator;


    private void Awake()
    {
        pathFinder = GetComponent<PathNavigator>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           // StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        pathFinder.SetAttacking(true);
        animator.SetTrigger("Attack");
        yield return null;
    }

}

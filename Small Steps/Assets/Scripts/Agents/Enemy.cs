using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    PathNavigator pathFinder;
    bool isAttacking;
    public Animator tentacleAnimator;


    private void Awake()
    {
        pathFinder = GetComponentInParent<PathNavigator>();
        tentacleAnimator = GetComponent<Animator>();
     
    }

    private void LateUpdate()
    {
        RotateToTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void RotateToTarget()
    {
        Quaternion targetRot = Quaternion.LookRotation(pathFinder.target.transform.position - transform.position);
        transform.rotation = targetRot;
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

    }

    private IEnumerator AttackRoutine()
    {      
        pathFinder.SetAttacking(true);
        tentacleAnimator.SetTrigger("Attack");
        isAttacking = true;
        yield return new WaitForSeconds(tentacleAnimator.GetCurrentAnimatorStateInfo(0).length);

        pathFinder.SetAttacking(false);

        isAttacking = false;


    }

}

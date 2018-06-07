using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    PathNavigator pathFinder;
    bool isAttacking;
    bool hitting;
    public Animator tentacleAnimator;

    [FMODUnity.EventRef] public string attackSound, hitSound;


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

        if (other.gameObject.tag == "Player" && hitting)
        {
            hitting = false;
            FMODUnity.RuntimeManager.PlayOneShot(hitSound, other.transform.position);
            other.GetComponentInParent<PlayerMachine>().GetHit(1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }

        if (other.gameObject.tag == "Player" && hitting)
        {
            hitting = false;
            FMODUnity.RuntimeManager.PlayOneShot(hitSound, other.transform.position);
            other.GetComponentInParent<PlayerMachine>().GetHit(1);
        }

    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Player" && hitting)
    //    {
    //        other.GetComponent<PlayerMachine>().GetHit(1);
    //    }
    //}

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
        yield return new WaitForSeconds(tentacleAnimator.GetCurrentAnimatorStateInfo(0).length/2);
        hitting = true;
        FMODUnity.RuntimeManager.PlayOneShotAttached(attackSound, this.gameObject);
        yield return new WaitForSeconds(tentacleAnimator.GetCurrentAnimatorStateInfo(0).length / 2);
        pathFinder.SetAttacking(false);
        isAttacking = false;
        hitting = false;


    }

}

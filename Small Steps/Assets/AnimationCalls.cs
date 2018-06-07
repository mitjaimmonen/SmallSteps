using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCalls : MonoBehaviour {

	PlayerMachine playerMachine;

	void Awake()
	{
		playerMachine = GetComponentInParent<PlayerMachine>();
	}
    public void AttackEnd()
    {
		Debug.Log(playerMachine);
        playerMachine.IsAttacking = false;
    }
}

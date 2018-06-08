using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetCatcher : MonoBehaviour {

	public bool isAttacking;
	[FMODUnity.EventRef] public string catchSound;

	void OnTriggerStay(Collider col)
	{
		// Debug.Log("TriggerEnter, isAttacking: " + isAttacking);
		if (isAttacking)
		{
			if (col.gameObject.layer == LayerMask.NameToLayer("Satellite"))
			{
				FMODUnity.RuntimeManager.PlayOneShot(catchSound, col.transform.position);
				Destroy(col.gameObject);
			}
		}

	}
}

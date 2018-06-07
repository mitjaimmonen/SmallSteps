using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectHere : MonoBehaviour {

	public GameObject objectToSpawn;
	public bool keepDetatched;
	GameObject obj = null;
	void Awake () {
		obj = Instantiate(objectToSpawn, transform.position, transform.rotation);
		if (!keepDetatched)
			obj.transform.parent = this.transform;
	}
	void Update()
	{
		if (keepDetatched && obj)
			obj.transform.position = transform.position;
	}

	void OnDestroy()
	{
		if (obj)
		{
			var ps = obj.GetComponent<ParticleSystem>();
			if (ps)
				ps.Stop();
		}

	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    public bool isReachable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Walkable")
        {
            isReachable = true;
        }

        if (other.gameObject.tag == "NotWalkable")
        {
            isReachable = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Walkable")
        {
            isReachable = true;
        }

        if (other.gameObject.tag == "NotWalkable")
        {
            isReachable = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isReachable = false;
    }

}

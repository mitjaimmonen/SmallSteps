using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    public bool isReachable;
    public bool grounded;

    private void OnTriggerEnter(Collider other)
    {
        grounded = true;

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
        grounded = true;

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
        grounded = false;
    }

}

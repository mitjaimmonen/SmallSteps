using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    public bool isReachable;
    public bool grounded = true;


    private void Update()
    {
        if (!grounded)
        {
            CheckGroundType();
        }
    }

    private void CheckGroundType()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 25f))
        {            
            if (hit.transform.gameObject.tag == "NotWalkable")
                isReachable = false;
            else
                isReachable = true;
        }
        else
            isReachable = false;
    }
    private void OnTriggerEnter(Collider other)
    {       

        if (other.gameObject.tag == "Walkable")
        {
            isReachable = true;
            grounded = true;
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
            grounded = true;
        }

        if (other.gameObject.tag == "NotWalkable")
        {
            isReachable = false;
            grounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       // isReachable = false;
        grounded = false;
    }

}

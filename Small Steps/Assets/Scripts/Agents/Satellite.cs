using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour
{

    public GameObject planet;
    public Transform center;
    public Vector3 axis;
    public Vector3 desiredPosition;
    public LevelManager levelManager;
    public float scoreValue = 9;
    public float planetRadius;
    public float spawnRangemin;
    public float spawnRangemax;
    public float radius;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;

    void Start()
    {
        center = planet.transform;
        axis = RandomDirection();
        radius = RandomRadius();
        transform.position = (transform.position - center.position).normalized * radius + center.position;



    }

    void Update()
    {
        transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
        desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }

    private float RandomRadius()
    {        
        float radiusModifier = Random.Range(spawnRangemin, spawnRangemax);

        return planetRadius + radiusModifier;
    }

    private Vector3 RandomDirection()
    {
        Vector3 randomAxis = new Vector3();
        int randomindex = Random.Range(0, 8);
        switch (randomindex)
        {
            case 0:
                randomAxis = Vector3.up;
                break;
            case 1:
                randomAxis = Vector3.down;
                break;
            case 2:
                randomAxis = Vector3.right;
                break;
            case 3:
                randomAxis = Vector3.left;
                break;
            case 4:
                randomAxis = Vector3.up + Vector3.left;
                break;
            case 5:
                randomAxis = Vector3.down + Vector3.left;
                break;
            case 6:
                randomAxis = Vector3.up + Vector3.right;
                break;
            case 7:
                randomAxis = Vector3.down + Vector3.right;
                break;
        }

        return randomAxis;
    }

    private void OnDestroy()
    {
        levelManager.OnSatelitteCaught(scoreValue);
    }

}

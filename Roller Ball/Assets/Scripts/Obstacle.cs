using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool isTurning = false;
    public bool backward = false;
    public float turnSpeed = 20;
    public static float moveSpeed = 22;
    
    void FixedUpdate()
    {
        Move();

        if(isTurning)
        {
            Turning();
        }

        Destroy(this, 15);
    }

    void Move()
    {
        transform.position += -transform.forward * moveSpeed * Time.deltaTime;
    }

    void Turning()
    {
        Vector3 v = Vector3.forward;
            if(backward == false)
                v = Vector3.forward;
            else
                v = -Vector3.forward;

        transform.RotateAround(transform.position, v, turnSpeed * Time.deltaTime);
    }

}

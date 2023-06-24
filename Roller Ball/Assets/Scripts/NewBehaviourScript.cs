using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]Transform start, end;

    float time;

    void Update()
    {
        float t = Mathf.SmoothStep(0, 1, time/5);
        print(t);
        transform.position = Vector3.Lerp(start.position, end.position, t);
        time += Time.deltaTime;
    }
}

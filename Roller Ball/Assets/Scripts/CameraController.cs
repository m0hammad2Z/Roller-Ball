using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private AnimationCurve rotateCurve, moveCurve;
    [SerializeField] private float rotateSpeed, moveSpeed, minAngle, maxAngle, minPos, maxPos;

    [SerializeField]float duration, amplitude;
    //[SerializeField] Vector3 noiseVector;

    private Vector3 ballAngle, cameraAngle;
    private Vector3 ballPos, cameraPos;

    float fov;
    private void Start()
    {
        fov = GetComponent<Camera>().fieldOfView;
    }

    void LateUpdate()
    {

        //BEGIN ROTATION
        ballAngle = ball.eulerAngles;
        cameraAngle = transform.eulerAngles;

        float angle = ballAngle.z;
        if (angle > 180f)
            angle -= 360f;
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        cameraAngle.z = angle;

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, cameraAngle, rotateCurve.Evaluate(Time.time) * rotateSpeed *Time.time );
        //END ROTATION



        //BEGIN POSITION
        ballPos = ball.GetChild(0).position;
        cameraPos = transform.position;

        float pos = ballPos.x;
        pos = Mathf.Clamp(pos, minPos, maxPos);
        cameraPos.x = pos;

        transform.position = Vector3.Lerp(transform.position, cameraPos, moveCurve.Evaluate(Time.time) * moveSpeed * Time.time);
        //END POSITION

        if(GetComponent<Camera>().fieldOfView > fov)
        {
            GetComponent<Camera>().fieldOfView -= Time.deltaTime;
        }
    }




    public IEnumerator Shake()
    {
        Vector3 camPos = transform.position;
        float currentFov = GetComponent<Camera>().fieldOfView;

        float elapsed = 0.0f;
        while(elapsed < duration)
        {
            Vector3 xyz = new Vector3(Random.Range(-1f, 1f) * amplitude, Random.Range(-1f, 1f) * amplitude);
            transform.position = Vector3.Lerp(transform.position, camPos + xyz, moveCurve.Evaluate(Time.time) * 2);


            float fov2 = Mathf.SmoothStep(currentFov, currentFov + ((6 + Random.Range(0.3f, 1.3f)) - Mathf.Abs(currentFov - fov)), elapsed / duration);
            GetComponent<Camera>().fieldOfView = fov2;

            elapsed += Time.deltaTime;

            yield return null;
        }


        //GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(GetComponent<Camera>().fieldOfView, updateFov + 0.2f, moveCurve.Evaluate(Time.deltaTime*20));

        transform.position = Vector3.Lerp(transform.position, camPos, moveCurve.Evaluate(Time.time) * 2);

    }

}

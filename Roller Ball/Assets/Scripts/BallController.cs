using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BallController : MonoBehaviour
{
    Game gameInputActions;
    float startTouchValue, currentTouchValue;
    bool isTouch;

    public GameManager gameManager;
    public Transform cylinder, childBall;
    public float ballTurnSpeedTouch, ballTurnSpeedPC, maxAngle, minAngle;

    [SerializeField] CameraController cam;
    
    void Awake()
    {
        gameInputActions = new Game();
        gameInputActions.Enable();

        gameInputActions.GameMap.TouchPress.started += startTouch;
        gameInputActions.GameMap.TouchPress.canceled += cancelTouch;

    }

    private void startTouch(InputAction.CallbackContext ctx)
    {
        isTouch = true;
    }

    private void cancelTouch(InputAction.CallbackContext ctx)
    {
        isTouch = false;
    }


    private void FixedUpdate()
    {
        Rotate();
    }


    void Rotate()
    {
        startTouchValue = gameInputActions.GameMap.Starttouchpos.ReadValue<float>();
        currentTouchValue = gameInputActions.GameMap.Currenttouchpos.ReadValue<float>();

        if (gameInputActions.GameMap.RightArrow.ReadValue<float>() > 0)
        {
            transform.RotateAround(cylinder.transform.position, Vector3.forward, -(ballTurnSpeedPC * Time.deltaTime));
            //Debug.Log("Left" + gameInputActions.GameMap.RightArrow.ReadValue<float>());
        }
        else if(gameInputActions.GameMap.LeftArrow.ReadValue<float>() > 0)
        {
            transform.RotateAround(cylinder.transform.position, Vector3.forward, +(ballTurnSpeedPC * Time.deltaTime));
            //Debug.Log("Left" + gameInputActions.GameMap.LeftArrow.ReadValue<float>());
        }

        //Free rotation
        if (isTouch)
        {
            float angle = -(currentTouchValue - startTouchValue) * ballTurnSpeedTouch * Time.deltaTime;
            transform.RotateAround(cylinder.transform.position, Vector3.forward, angle);
            
        }
        Vector3 clampedEulerAngles = transform.eulerAngles;

        // Clamping Z-axis
        float clampedAngleZ = clampedEulerAngles.z;
        print(clampedAngleZ);
        if (clampedAngleZ > 180f)
            clampedAngleZ -= 360f;
        clampedAngleZ = Mathf.Clamp(clampedAngleZ, minAngle, maxAngle);

        clampedEulerAngles.z = clampedAngleZ;

        transform.eulerAngles = clampedEulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            childBall.gameObject.SetActive(false);
            gameManager.OnGameOver();
        }
        if (other.gameObject.tag == "Vibrate")
        {
            //gameManager.GetComponent<AudioSource>().clip = gameManager.passObstecale;
            //gameManager.GetComponent<AudioSource>().Play();
            StartCoroutine(cam.Shake());
            
            Destroy(other.gameObject, 0.5f);
        }
        if (other.gameObject.tag == "EndPoint")
        {
            gameManager.OnLevelComplete();
            gameManager.GetComponent<AudioSource>().clip = gameManager.completeLevel;
            gameManager.GetComponent<AudioSource>().Play();
        }
        if (other.gameObject.tag == "clone")
        {
            Destroy(other.gameObject, 0.5f);
        }
    }


}

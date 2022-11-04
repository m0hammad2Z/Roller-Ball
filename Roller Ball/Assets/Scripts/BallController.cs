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
    public float ballTurnSpeedTouch, ballTurnSpeedPC;


    //float angle, counter = 0.3f;

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

    //private void Update()
    //{
    //    counter = counter - 1 * Time.deltaTime;

    //}


    private void FixedUpdate()
    {
        //if(counter <= 0)
            Rotate();

        //if (!isTouch)
        //{
        //    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), ballTurnSpeed);
        //    GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), ballTurnSpeed));
        //    startTouchValue = 0;
        //    currentTouchValue = 0;
        //}
    }


    void Rotate()
    {
        startTouchValue = gameInputActions.GameMap.Starttouchpos.ReadValue<float>();
        currentTouchValue = gameInputActions.GameMap.Currenttouchpos.ReadValue<float>();


        //if ((currentTouchValue - startTouchValue) > 0 && isTouch)
        //{
        //    angle = transform.rotation.eulerAngles.z + 90; counter = 0.37f;
        //}
        //else if ((currentTouchValue - startTouchValue) < 0 && isTouch)
        //{
        //    angle = transform.rotation.eulerAngles.z - 90; counter = 0.37f;
        //}

        if (gameInputActions.GameMap.RightArrow.ReadValue<float>() > 0)
        {
            transform.RotateAround(cylinder.transform.position, Vector3.forward, -(ballTurnSpeedPC * Time.deltaTime));
            Debug.Log("Left" + gameInputActions.GameMap.RightArrow.ReadValue<float>());
        }
        else if(gameInputActions.GameMap.LeftArrow.ReadValue<float>() > 0)
        {
            transform.RotateAround(cylinder.transform.position, Vector3.forward, +(ballTurnSpeedPC * Time.deltaTime));
            Debug.Log("Left" + gameInputActions.GameMap.LeftArrow.ReadValue<float>());
        }

        //Free rotation
        if (isTouch)
            transform.RotateAround(cylinder.transform.position, Vector3.forward, (currentTouchValue - startTouchValue) * ballTurnSpeedTouch * Time.deltaTime);

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
            gameManager.GetComponent<AudioSource>().clip = gameManager.passObstecale;
            gameManager.GetComponent<AudioSource>().Play();
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

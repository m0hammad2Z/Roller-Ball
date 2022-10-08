using UnityEngine;


public class RotateBall : MonoBehaviour {

   void Update(){
    transform.Rotate(Vector3.up * Obstacle.moveSpeed * 0.7f);
   }    
}
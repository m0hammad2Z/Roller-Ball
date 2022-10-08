using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPoint : MonoBehaviour
{
     void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);
    }
 void OnTriggerExit(Collider other) {
    Destroy(other.gameObject);
}
}

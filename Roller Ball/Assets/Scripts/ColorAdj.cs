using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorAdj : MonoBehaviour
{
    Volume volume;
    ColorAdjustments colorAdjustments;

    float offset;
    [SerializeField] float speed, distanceUPDown;
    void Start()
    {
        volume = GetComponent<Volume>();

        volume.profile.TryGet(out colorAdjustments);

        offset = Random.Range(colorAdjustments.hueShift.min, colorAdjustments.hueShift.max);

    }

   
    void Update()
    {
        colorAdjustments.hueShift.value = offset + Mathf.Sin(speed * Time.time) * distanceUPDown;
    }
}

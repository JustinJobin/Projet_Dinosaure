using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProieSim1 : MonoBehaviour
{
    private const float vitesseNormale = 1f;
    private const float vitesseFuite = 13.9f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * vitesseFuite);
    }
}

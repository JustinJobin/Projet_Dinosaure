using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProieSim1 : MonoBehaviour
{
    private const float vitesseFuite = 13.9f;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * vitesseFuite);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProieSim2 : MonoBehaviour
{
    private const float vitesseNormale = 1f;
    private const float vitesseFuite = 13.9f;
    private const float rayon = 0.25f;
    private const double pi = System.Math.PI;
    private float angleMax = 0;
    private int chrono = 0;
    private bool attendu = true;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Deplacement", 0, 0.05f);
        InvokeRepeating("Chronometre", 0, 1);
    }

    private void Deplacement()
    {
        float angle = angleMax;
        if (chrono % 3 == 0 && chrono > 0)
        {

            if (chrono % 2 == 0)
            {
                angleMax = Convert.ToSingle(-(vitesseFuite * 0.05 / (2 * rayon)));//radians
            }
            else
            {
                angleMax = Convert.ToSingle(vitesseFuite * 0.05 / (2 * rayon));//radians
            }
        }
        print(angleMax);

        if (angle == angleMax)
        {
            transform.Rotate(new Vector3(0, Convert.ToSingle(360 * angleMax / (pi * 2)), 0));
        }

        if (angleMax == 0)
        {
            transform.Translate(new Vector3(0, 0, 1) * 0.05f * vitesseFuite);
        }
        else
        {
            transform.Translate(new Vector3(0, 0, 1) * Convert.ToSingle(2 * rayon * System.Math.Sin(angleMax)));
        }
        //transform.Translate(new Vector3(0, 0, 1) * 0.05f * vitesseFuite);
    }

    private void Chronometre()
    {
        chrono++;
    }
}

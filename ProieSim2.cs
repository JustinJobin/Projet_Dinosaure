using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ProieSim2 : MonoBehaviour
{
    //private const float vitesseNormale = 1f;
    private const float vitesseFuite = 13.9f;
    //private float distancUTurn;
    private float distancUTurn;
    //private const float rayon = 0.5f;
    private const float rayon = 5f;
    private const double pi = System.Math.PI;
    private float angleMax = 0;
    private GameObject pred;
    bool modePasFuite = true;
    private float rotation;

    // Start is called before the first frame update
    void Start()
    {
        StreamReader LireFichier = new StreamReader("k.txt");
        string ligne = string.Empty;
        while ((ligne = LireFichier.ReadLine()) != null)// Tant que la ligne n'est pas null
        {
            distancUTurn = int.Parse(ligne);
        }
        LireFichier.Close();

        pred = GameObject.Find("Pred");
        InvokeRepeating("Deplacement", 0, 0.05f);
    }

    private void Deplacement()
    {
        if (angleMax == 0)
        {
            transform.Translate(new Vector3(0, 0, 1) * 0.05f * vitesseFuite);
        }
        else
        {
            transform.Rotate(new Vector3(0, Convert.ToSingle(360 * angleMax / (pi * 2)), 0));
            transform.Translate(new Vector3(0, 0, 1) * Convert.ToSingle(System.Math.Abs(2 * rayon * System.Math.Sin(angleMax))));
        }

        if (Vector3.Distance(transform.position, pred.transform.position) < distancUTurn && modePasFuite)
        {
            int x = UnityEngine.Random.Range(1, 3);
            if(x==2)
            {
                angleMax = Convert.ToSingle(vitesseFuite * 0.05 / (2 * rayon));//radians
            }
            else
            {
                angleMax = -Convert.ToSingle(vitesseFuite * 0.05 / (2 * rayon));//radians
            }
            modePasFuite = false;
        }

        if (modePasFuite == false && rotation > 180)
        {
            angleMax = 0;
        }
        rotation += System.Math.Abs(Convert.ToSingle(360 * angleMax / (pi * 2)));
    }
}

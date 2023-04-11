using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class ProieSim4 : MonoBehaviour
{
    // Technique de fuite en zigzag pour la proie
    
    
    private const double pi = System.Math.PI;
    private GameObject pred;
    private bool modePasFuite = true;
    private bool premierVirage = false;
    private float rotation;
    private float currentAngle = 0;
    private float angleMax;
    private bool droitTourner = true;
    private float momentFinVirage = 0;
    private float chrono = 0;
    private float compteARebours = 15;
    private int nbVirages = 0;


    //paramètres
    private float tempsAttProchVirage = 1f;
    private int nbVirageMax = 4;
    private float disDebutVirage = 20;
    //private float rayon = 0.5f;
    private const float rayon = 3f;
    //private const float vitesseNormale = 1f;
    private const float vitesseFuite = 13.9f;


    void Start()
    {
        //tempsAttProchVirage = UnityEngine.Random.Range(5, 25) / 10;
        //nbVirageMax = UnityEngine.Random.Range(1, 8);
        //disDebutVirage = UnityEngine.Random.Range(14, 15);

        pred = GameObject.Find("Pred");
        angleMax = Convert.ToSingle(vitesseFuite * 0.008f / (2 * rayon));//radians
        InvokeRepeating("Deplacement", 0, 0.008f);
        //InvokeRepeating("chronometre", 0, 1);
    }

    //private void chronometre()
    //{
    //    if (compteARebours >= 0)
    //    {
    //        compteARebours--;
    //    }

    //    if (compteARebours == 0)
    //    {
    //        StreamWriter EcrireVictoire = new StreamWriter("RésultatsSim2.txt", true);
    //        EcrireVictoire.WriteLine("Proie " + 0 + " " + Vector3.Distance(pred.transform.position, transform.position) + " " +
    //            25 + " " + disDebutVirage + " " + tempsAttProchVirage + " " + nbVirageMax);
    //        EcrireVictoire.Flush();
    //        EcrireVictoire.Close();
    //        Invoke("RetourEchantillon", 0);
    //    }
    //}

    void Deplacement()
    {
        if (chrono > momentFinVirage + tempsAttProchVirage && !modePasFuite)
        {
            droitTourner = true;
        }

        if (Vector3.Distance(transform.position, pred.transform.position) < disDebutVirage && modePasFuite)
        {
            modePasFuite = false;
            premierVirage = true;
            currentAngle = angleMax;
        }


        if (currentAngle == 0)
        {
            transform.Translate(new Vector3(0, 0, 1) * 0.008f * vitesseFuite);
        }
        else
        {
            if(droitTourner)
            {
                transform.Rotate(new Vector3(0, Convert.ToSingle(360 * currentAngle / (pi * 2)), 0));
            }
            transform.Translate(new Vector3(0, 0, 1) * Convert.ToSingle(2 * rayon * System.Math.Abs(System.Math.Sin(currentAngle))));
        }

        if (!modePasFuite && droitTourner)
        {
            rotation = rotation + Convert.ToSingle(360 * currentAngle / (pi * 2));
        }


        if (nbVirages < nbVirageMax)
        {
            if (rotation > 45 && premierVirage)
            {
                currentAngle = -angleMax;
                rotation = 0;
                premierVirage = false;
                momentFinVirage = chrono;
                droitTourner = false;
                nbVirages++;
            }

            if (rotation > 90)
            {
                currentAngle = -angleMax;
                rotation = 0;
                momentFinVirage = chrono;
                droitTourner = false;
                nbVirages++;
            }
            else
            if (rotation < -90)
            {
                currentAngle = angleMax;
                rotation = 0;
                momentFinVirage = chrono;
                droitTourner = false;
                nbVirages++;
            }
        }
        else
        {
            currentAngle = 0;
        }
        chrono += 0.008f;
        print(transform.localEulerAngles);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "pred")
    //    {
    //        StreamWriter EcrireVictoire = new StreamWriter("RésultatsSim2.txt", true);
    //        EcrireVictoire.WriteLine("Preda " + (15-compteARebours) + " " + 0 + " " +
    //                        25 + " " + disDebutVirage + " " + tempsAttProchVirage + " " + nbVirageMax);
    //        EcrireVictoire.Flush();
    //        EcrireVictoire.Close();
    //        Invoke("RetourEchantillon", 0);
    //    }
    //}
    //private void RetourEchantillon()
    //{
    //    SceneManager.LoadScene("Intermediaire");
    //}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Code pour le prédateur du bas de la simulation 3, le code est identique au prédateur du haut, (voir PredASim3)
public class PredBSim3 : MonoBehaviour
{
    public Text message_Alerte;
    private GameObject proie;
    private float angleMax = 0;
    private float currentAngle = 0;
    private float currentAngleSign = 0;
    private Vector3 targetDir;
    private Vector3 dirInitProie;
    private int chrono = 15;
    private bool attaque = false;
    private float gap = 10;


    private const float vitesseChasse = 16.7f;
    private const float rayon = 15f;
    private const double pi = System.Math.PI;

    // Start is called before the first frame update
    void Start()
    {
        proie = GameObject.Find("Proie");
        dirInitProie = proie.transform.forward;

        transform.position = new Vector3(proie.transform.position.x - UnityEngine.Random.Range(Convert.ToInt32(Math.Sqrt(225 - Math.Pow(gap, 2))),
            Convert.ToInt32(Math.Sqrt(1849 - Math.Pow(gap, 2)))),
            0, proie.transform.position.z - gap);

        InvokeRepeating("Mouvement", 0, 0.05f);
        InvokeRepeating("chronometre", 0, 1);
    }
    private void chronometre()
    {
        if (chrono >= 0)
        {
            chrono--;
        }

        //if (chrono == 0)
        //{
        //    StreamWriter EcrireDefaite = new StreamWriter("RésultatsSim3.txt", true);
        //    EcrireDefaite.WriteLine(" Proie  " + "0".PadLeft(4));
        //    EcrireDefaite.Flush();
        //    EcrireDefaite.Close();
        //    Invoke("RetourEchantillon", 0);
        //}
    }

    private void Mouvement()
    {
        if ((dirInitProie != proie.transform.forward) && !attaque)
        {
            attaque = true;
        }


        if (attaque)
        {
            targetDir = proie.transform.position - transform.position;
        }
        else
        {
            targetDir = new Vector3(proie.transform.position.x, 0, 0);
        }

        if (chrono < 5 || proie.transform.position.x < transform.position.x)
        {
            targetDir = proie.transform.position - transform.position;
        }


        // mouvement du prédateur
        angleMax = Convert.ToSingle(vitesseChasse * 0.05 / (2 * rayon));//radians
        currentAngle = Vector3.Angle(transform.forward, targetDir);//degrés
        currentAngle = Convert.ToSingle(currentAngle * 2 * pi / 360);//radians
        currentAngleSign = Vector3.SignedAngle(transform.forward, targetDir, Vector3.up);//angle signé(sert à savoir si le prédateur
                                                                                         //doit tourner à droite ou à gauche)
        if (currentAngle > angleMax)
        {
            RotationPred(angleMax, currentAngleSign);
            transform.Translate(new Vector3(0, 0, 1) * Convert.ToSingle(2 * rayon * System.Math.Sin(angleMax)));
        }
        else
        {
            RotationPred(currentAngle, currentAngleSign);
            if (currentAngle == 0)
            {
                transform.Translate(new Vector3(0, 0, 1) * 0.05f * vitesseChasse);
            }
            else
            {
                transform.Translate(new Vector3(0, 0, 1) * 0.05f * vitesseChasse * Convert.ToSingle(System.Math.Sin(currentAngle) / currentAngle));
            }
        }
        
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "proie")
    //    {
    //        message_Alerte.text = "Prédateur win";
    //        StreamWriter EcrireVictoire = new StreamWriter("RésultatsSim3.txt", true);
    //        EcrireVictoire.WriteLine(" Preda2 " +
    //            Convert.ToString(15 - chrono).PadLeft(4));
    //        EcrireVictoire.Flush();
    //        EcrireVictoire.Close();
    //        Invoke("RetourEchantillon", 0);
    //        //Invoke("RetourAccueil", 0);
    //    }
    //}

    //private void RetourAccueil()
    //{
    //    SceneManager.LoadScene("Scene1");
    //}

    private void RotationPred(float angleCourant, float angleCourantSign)
    {
        if (angleCourantSign < 0)
        {
            transform.Rotate(new Vector3(0, Convert.ToSingle(360 * -angleCourant / (pi * 2)), 0));
        }
        else
        {
            transform.Rotate(new Vector3(0, Convert.ToSingle(360 * angleCourant / (pi * 2)), 0));
        }
    }

    //private void RetourEchantillon()
    //{
    //    SceneManager.LoadScene("Intermediaire");
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;


public class PredSim6 : MonoBehaviour
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
    private float targetEnZ;
    private bool parEnHaut = false;
    private float gap = 5;
    //private float distancUTurn;


    //private const float vitesseNormale = 1f;
    private const float vitesseChasse = 16.7f;
    //private const float rayon = 1.5f;
    private const float rayon = 9f;
    private const double pi = System.Math.PI;

    // Start is called before the first frame update
    void Start()
    {
        //StreamReader LireFichier = new StreamReader("k.txt");
        //string ligne = string.Empty;
        //while ((ligne = LireFichier.ReadLine()) != null)// Tant que la ligne n'est pas null
        //{
        //    distancUTurn = int.Parse(ligne);
        //}
        //LireFichier.Close();

        proie = GameObject.Find("Proie");
        dirInitProie = proie.transform.forward;

        int x = UnityEngine.Random.Range(1, 3);
        if(x==1)
        {
            targetEnZ = transform.position.z + gap;
            parEnHaut = true;
        }
        else
        {
            targetEnZ = transform.position.z - gap;
            parEnHaut = false;
        }
        InvokeRepeating("Mouvement", 0, 0.05f);
        InvokeRepeating("chronometre", 0, 1);
    }
    private void chronometre()
    {
        if (chrono >= 0)
        {
            chrono--;
        }

        if (chrono == 0)
        {
            //StreamWriter EcrireDefaite = new StreamWriter("kTest.txt", true);
            //EcrireDefaite.WriteLine(distancUTurn+" Success");
            //EcrireDefaite.Flush();
            //EcrireDefaite.Close();
            //Invoke("RetourEchantillon", 0);
        }


    }

    private void Mouvement()
    {
        if ((dirInitProie != proie.transform.forward) && !attaque)
        {
            attaque = true;
        }

        if(parEnHaut)
        {
            if (!attaque && transform.position.z > targetEnZ)
            {
                targetDir = new Vector3(proie.transform.position.x, 0, 0);
            }

            if (!attaque && transform.position.z < targetEnZ)
            {
                targetDir = new Vector3(proie.transform.position.x, 0, gap);
            }
        }
        else
        {
            if (!attaque && transform.position.z < targetEnZ)
            {
                targetDir = new Vector3(proie.transform.position.x, 0, 0);
            }

            if (!attaque && transform.position.z > targetEnZ)
            {
                targetDir = new Vector3(proie.transform.position.x, 0, -gap);
            }
        }
        

        if (attaque)
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
        //transform.Rotate(new Vector3(0, Convert.ToSingle(360 * currentAngle / (pi * 2)), 0));
        //transform.Translate(new Vector3(0, 0, 1) * Convert.ToSingle(2 * rayon * System.Math.Sin(currentAngle)));
        //transform.LookAt(proie.transform.position);
        //transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * vitesseChasse);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "proie")
        {
            message_Alerte.text = "Prédateur win";
            //StreamWriter EcrireVictoire = new StreamWriter("kTest.txt", true);
            //EcrireVictoire.WriteLine(distancUTurn+" Failed");
            //EcrireVictoire.Flush();
            //EcrireVictoire.Close();
            //Invoke("RetourEchantillon", 0);
            //Invoke("RetourAccueil", 0);
        }
    }

    private void RetourAccueil()
    {
        SceneManager.LoadScene("Scene1");
    }

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

    private void RetourEchantillon()
    {
        SceneManager.LoadScene("Intermediaire");
    }
}

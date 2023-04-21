using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//Gestion du prédateur du haut dans la simulation 3
public class PredASim3 : MonoBehaviour
{
    public Text message_Alerte;//message sur l'écran
    private GameObject proie;//la proie
    private float angleMax = 0;//angle maximum que le prédateur peut tourner à chaque itération
    private float currentAngle = 0;//angle courant qu'il a entre son vecteur direction et celui de la proie
    private float currentAngleSign = 0;//le même angle que plus haut, mais signé
    private Vector3 targetDir;//vecteur direction idéale du prédateur
    private Vector3 dirInitProie;//vecteur direction initiale de la proie 
    private int chrono = 15;//chrono
    private bool attaque = false;//si le prédateur est en mode attaque ou pas
    private float gap = 10;// le gap en z entre lui et la proie lorsqu'il n'est pas en mode attaque


    private const float vitesseChasse = 16.7f;//sa vitesse
    private const float rayon = 15f;//son rayon minimum pour ses virages
    private const double pi = System.Math.PI;//pi

    //on définit les variables initiales (proie,la direction initiale de la proie)
    void Start()
    {
        proie = GameObject.Find("Proie");
        dirInitProie = proie.transform.forward;

        //on place le prédateur a une distance entre 15 et 43 derrière la proie en gardant une position de 10 de plus
        //que la proie en z, donc, on utilise pythagore pour trouver sa position en x
        transform.position = new Vector3(proie.transform.position.x - UnityEngine.Random.Range(Convert.ToInt32(Math.Sqrt(225 - Math.Pow(gap, 2))),
            Convert.ToInt32(Math.Sqrt(1849 - Math.Pow(gap, 2)))),
            0, proie.transform.position.z + 10);

        
        InvokeRepeating("Mouvement", 0, 0.05f);//il bouge 
        InvokeRepeating("chronometre", 0, 1);//gestion du chrono
    }

    //gestion du chrono
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

    //mouvement du prédateur
    private void Mouvement()
    {
        // Une grande partie du mouvement des prédateurs est expliqué dans la simulation 2, je vais expliquer qu'est-ce qui change ici
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

        //Si il ne reste plus beaucoup de temps, on le prédateur dépasse la proie, le prédateur fonce dessus
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

    ////Si le prédateur attrape la proie, on affiche que le prédateur gagne et on revient à l'accueil
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "proie")
    //    {
    //        message_Alerte.text = "Prédateur win";
    //        StreamWriter EcrireVictoire = new StreamWriter("RésultatsSim3.txt", true);
    //        EcrireVictoire.WriteLine(" Preda1 " +
    //            Convert.ToString(15 - chrono).PadLeft(4));
    //        EcrireVictoire.Flush();
    //        EcrireVictoire.Close();
    //        Invoke("RetourEchantillon", 0);
    //        //Invoke("RetourAccueil", 0);
    //    }
    //}

    ////retour à l'accueil
    //private void RetourAccueil()
    //{
    //    SceneManager.LoadScene("Scene1");
    //}

    //on le fait tourner à droite ou à gauche à chaque itération, dépendamment du signe de angleCourantSign
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

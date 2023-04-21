using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.IO;


//Code du prédateur de la simulation 6
//Ce bloc de code ressemble beaucoup à celui de la simulation 2, il y a seulement la stratégie qu'il faut ajouter
//Je vais que décrire ce qui a été ajouter
public class PredSim6 : MonoBehaviour
{
    private bool predGagne = false;
    private bool proieTouchable = true;
    public Text message_Alerte;
    private GameObject proie;
    private float angleMax = 0;
    private float currentAngle = 0;
    private float currentAngleSign = 0;
    private Vector3 targetDir;
    private Vector3 dirInitProie;
    private int chrono = 15;
    private bool attaque = false;
    private float targetEnZ;//target en z qu'il faut avoir lorsqu'on est en mode stratégie
    private bool parEnHaut = false;//stratégie par la gauche ou la droite de la proie (droite=bas,gauche=haut)
    private float gap = 10;//distance en z entre la proie et le prédateur lors de la stratégies


    private const float vitesseChasse = 16.7f;
    private const float rayon = 15f;
    private const double pi = System.Math.PI;

    void Start()
    {

        proie = GameObject.Find("Proie");
        dirInitProie = proie.transform.forward;
        
        //on choisit si le prédateur va y aller par en haut ou en bas
        int x = UnityEngine.Random.Range(1, 3);
        if(x==1)
        {
            targetEnZ = transform.position.z + gap;//par en haut
            parEnHaut = true;
        }
        else
        {
            targetEnZ = transform.position.z - gap;//par en bas
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

        if (chrono <= 0 && predGagne == false)
        {
            //StreamWriter EcrireDefaite = new StreamWriter("RésultatsSim4.txt", true);
            //EcrireDefaite.WriteLine(" Proie " + "0".PadLeft(4) +
            //    Convert.ToString(Convert.ToInt32(Vector3.Distance(transform.position, proie.transform.position))).PadLeft(4));
            //EcrireDefaite.Flush();
            //EcrireDefaite.Close();
            proieTouchable = false;
            message_Alerte.text = "Proie Win";
            Invoke("RetourAccueil", 2);
        }
    }

    private void Mouvement()
    {
        if ((dirInitProie != proie.transform.forward) && !attaque)
        {
            attaque = true;
        }

        //si le prédateur va par en haut, son vecteur direction va être vers le position de proie,
        //mais un décalage de 10 en z vers le haut, et -10 vers de le bas respectivement
        //Tout cela si le prédateur n'est pas en mode attaque
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
        
        //si il est en mode attaque, il fonce sur la proie
        if (attaque)
        {
            targetDir = proie.transform.position - transform.position;
        }

        // si il reste 5 secondes ou il dépasse la proie, il fonce sur cette dernière
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "proie" && proieTouchable)
        {
            message_Alerte.text = "Prédateur win";
            predGagne = true;
            //StreamWriter EcrireVictoire = new StreamWriter("RésultatsSim4.txt", true);
            //EcrireVictoire.WriteLine(" Preda " +
            //    Convert.ToString(15 - chrono).PadLeft(4) + "0".PadLeft(4));
            //EcrireVictoire.Flush();
            //EcrireVictoire.Close();
            //Invoke("RetourEchantillon", 0);
            Invoke("RetourAccueil", 2);
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

    //private void RetourEchantillon()
    //{
    //    SceneManager.LoadScene("Intermediaire");
    //}
}

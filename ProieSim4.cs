using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Code pour la proie de la simulation 4 (zigzags)
public class ProieSim4 : MonoBehaviour
{
    private bool proieTouchable = true;
    private bool predGagne = false;
    public Text message_Alerte;
    public Text Affichage_DistDeb;
    public Text Affichage_DistUturn;

    // Technique de fuite en zigzag pour la proie
    private const double pi = System.Math.PI;//pi
    private GameObject pred;//prédateur
    private bool modePasFuite = true;//mode fuite ou pas
    private bool premierVirage = false;//premier virage ou pas
    private bool finPremierVirage = false;//fin du premier virage ou pas
    private float rotation;//banque de rotation (même principe que la simulation 2)
    private float currentAngle = 0;//angle courant
    private float angleMax;//angle maximum selon le rayon de rotation
    private bool droitTourner = true;//si la proie a le droit de tourner ou pas
    private float momentFinVirage = 0;//temps à la fin du virage
    private float chrono = 0;//chrono
    private float compteARebours = 15;
    private int nbVirages = 0;


    //paramètres
    private float tempsAttProchVirage = 1f;
    private int nbVirageMax = 5;
    private float disDebutVirage;
    private float distancDebut;
    private const float rayon = 5f;
    //private const float vitesseNormale = 1f;
    private const float vitesseFuite = 13.9f;


    void Start()
    {
        //Même principe que la simulation 2, on randomise les distances et on trouve le prédateur
        pred = GameObject.Find("Pred");

        distancDebut = UnityEngine.Random.Range(15, 43);
        transform.position = new Vector3(pred.transform.position.x + distancDebut, 0, pred.transform.position.z);

        disDebutVirage = UnityEngine.Random.Range(0, Convert.ToInt32(distancDebut) + 1);

        Affichage_DistDeb.text = Affichage_DistDeb.text + distancDebut + " m";
        Affichage_DistUturn.text = Affichage_DistUturn.text + disDebutVirage + " m";

        //StreamWriter EcrireDefaite = new StreamWriter("RésultatsSim4.txt", true);
        //EcrireDefaite.Write(Convert.ToString(distancDebut).PadLeft(4) + Convert.ToString(disDebutVirage).PadLeft(4));
        //EcrireDefaite.Flush();
        //EcrireDefaite.Close();

        angleMax = Convert.ToSingle(vitesseFuite * 0.008f / (2 * rayon));//radians
        InvokeRepeating("Deplacement", 0, 0.008f);
        InvokeRepeating("chronometre", 0, 1);
    }

    private void chronometre()
    {
        if (compteARebours >= 0)
        {
            compteARebours--;
        }

        if (compteARebours <= 0 && predGagne == false)
        {
            proieTouchable = false;
            message_Alerte.text = "Proie Win";
            Invoke("RetourAccueil", 2);
        }
    }

    void Deplacement()
    {
        //si c'est la fin du premier virage et que le chrono a dépassé le temps d'attente, on vire de nouveau
        if(finPremierVirage)
        {
            if (chrono > momentFinVirage + (tempsAttProchVirage/2) && !modePasFuite)
            {
                finPremierVirage = false;
                droitTourner = true;
            }
        }
        else
        {
            //même principe si ce n'est pas le premier virage
            if (chrono > momentFinVirage + tempsAttProchVirage && !modePasFuite)
            {
                droitTourner = true;
            }
        }
        
        //si la distance entre entre les deux est inférieur à disDébutVirage, on commence les virages et on met la proie en mode Fuite
        if (Vector3.Distance(transform.position, pred.transform.position) < disDebutVirage && modePasFuite)
        {
            modePasFuite = false;
            premierVirage = true;
            currentAngle = angleMax;
        }

        //Rotation et déplacement de la proie
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

        //Condition pour les virages
        //Si le nombres de virages est au maximum de nombre de virages maximum, on arrête de virer
        if (nbVirages < nbVirageMax)
        {
            //banque de rotation pour le premier virage.
            //En effet, le premier virage est différent
            //des autres virages car la proie tourne
            //seulement de 45 degrés et non de 90 degrés.
            if (rotation > 45 && premierVirage)
            {
                currentAngle = -angleMax;
                rotation = 0;
                premierVirage = false;
                finPremierVirage = true;
                momentFinVirage = chrono;
                droitTourner = false;
                nbVirages++;
            }

            //pour les autres virages, même principe que pour la simulation 2,
            //sauf qu'on enregistre le moment où il y a fin du virage et on compile le nombre de virages
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pred" && proieTouchable)
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

    //private void RetourEchantillon()
    //{
    //    SceneManager.LoadScene("Intermediaire");
    //}

}

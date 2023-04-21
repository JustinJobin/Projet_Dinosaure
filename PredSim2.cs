using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

//code pour le pr�dateur de la simulation 2
public class PredSim2 : MonoBehaviour
{
    public Text message_Alerte;//message � l'�cran
    private GameObject proie;//proie
    private float angleMax = 0;//angle maximum que le pr�dateur peut tourner � chaque it�ration
    private float currentAngle = 0;//l'angle courant qui devrait tourner � une it�ration donn�e
    private float currentAngleSign = 0;//son angle, mais sign�
    private Vector3 targetDir;//son vecteur direction
    private int chrono = 15;//chrono
    private bool proieTouchable = true;
    private bool predGagne = false;


    private const float vitesseChasse = 16.7f;//sa vitesse
    private const float rayon = 15f;// son rayon
    private const double pi = System.Math.PI;//pi

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
        proie = GameObject.Find("Proie");//on trouve la proie
        
        //on d�place le pr�dateur et on g�re le chrono
        InvokeRepeating("Mouvement", 0, 0.05f);
        InvokeRepeating("chronometre", 0, 1);
    }

    //chrono
    private void chronometre()
    {
        if (chrono >= 0)
        {
            chrono--;
        }

        if (chrono <= 0 && predGagne == false)
        {
            //StreamWriter EcrireDefaite = new StreamWriter("R�sultatsSim4.txt", true);
            //EcrireDefaite.WriteLine(" Proie " + "0".PadLeft(4) +
            //    Convert.ToString(Convert.ToInt32(Vector3.Distance(transform.position, proie.transform.position))).PadLeft(4));
            //EcrireDefaite.Flush();
            //EcrireDefaite.Close();
            proieTouchable = false;
            message_Alerte.text = "Proie Win";
            Invoke("RetourAccueil", 2);
        }
    }

    //mouvement du pr�dateur
    private void Mouvement()
    {
        angleMax = Convert.ToSingle(vitesseChasse * 0.05 / (2 * rayon));//calcul de l'angle maximum qu'il peut
                                                                        //tourner selon son rayon minimum (radians)
        targetDir = proie.transform.position - transform.position;//son vecteur id�ale 
        currentAngle = Vector3.Angle(transform.forward, targetDir);//l'angle courant qu'il a entre sa direction et celui de la proie (degr�s)
        currentAngle = Convert.ToSingle(currentAngle * 2 * pi / 360);//transformation en radians
        currentAngleSign = Vector3.SignedAngle(transform.forward, targetDir, Vector3.up);//angle sign�(sert � savoir si le pr�dateur
                                                                                         //doit tourner � droite ou � gauche)
        //si son angle courant est plus grand que l'angle maximum, on utilise l'anglemax, sinon on prend l'angle courant pour se d�placer
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
            message_Alerte.text = "Pr�dateur win";
            predGagne = true;
            //StreamWriter EcrireVictoire = new StreamWriter("R�sultatsSim4.txt", true);
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

    //rotation du pr�dateur qui d�pend de gauche ou de droite
    private void RotationPred(float angleCourant,float angleCourantSign)
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

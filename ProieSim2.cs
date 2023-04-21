using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class ProieSim2 : MonoBehaviour
{
    public Text Affichage_DistDeb;
    public Text Affichage_DistUturn;

    private const float vitesseFuite = 13.9f;//sa vitesse
    private float distancUTurn;// sa distance entre lui et le pr�dateur lorsqu'il ex�cute son uturn
    private float distancDebut;//distance entre les deux au d�but
    private const float rayon = 5f;// son rayon
    private const double pi = System.Math.PI;//pi
    private float angleMax = 0;//son angle maximum qu'il peut tourner � chaque it�ration d�pendamment de son rayon minimum
    private GameObject pred;//le pr�dateur
    bool modePasFuite = true;//si il n'est pas en mode fuite
    private float rotation;//banque pour la rotation

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

        //on trouve le pr�dateur, on place la proie en avant du pr�dateur selon une distance randomis�e,
        //on randomise aussi la distance au moment que la proie fait son uturn
        pred = GameObject.Find("Pred");

        distancDebut = UnityEngine.Random.Range(15, 43);
        transform.position = new Vector3(pred.transform.position.x + distancDebut, 0, pred.transform.position.z);

        distancUTurn = UnityEngine.Random.Range(0, Convert.ToInt32(distancDebut) + 1);

        Affichage_DistDeb.text = Affichage_DistDeb.text + distancDebut + " m";
        Affichage_DistUturn.text = Affichage_DistUturn.text + distancUTurn + " m";

        //StreamWriter EcrireDefaite = new StreamWriter("R�sultatsSim6.txt", true);
        //EcrireDefaite.Write(Convert.ToString(distancDebut).PadLeft(4) + Convert.ToString(distancUTurn).PadLeft(4));
        //EcrireDefaite.Flush();
        //EcrireDefaite.Close();

        InvokeRepeating("Deplacement", 0, 0.05f);
    }

    private void Deplacement()
    {
        //le d�placemnt suit un peu le d�placement du pr�dateur (voir PredSim2)
        if (angleMax == 0)
        {
            transform.Translate(new Vector3(0, 0, 1) * 0.05f * vitesseFuite);
        }
        else
        {
            transform.Rotate(new Vector3(0, Convert.ToSingle(360 * angleMax / (pi * 2)), 0));
            transform.Translate(new Vector3(0, 0, 1) * Convert.ToSingle(System.Math.Abs(2 * rayon * System.Math.Sin(angleMax))));
        }

        //si la distance uturn est plus petite, on le met en mode Fuite et il commence � tourner en utilisant angleMax
        if (Vector3.Distance(transform.position, pred.transform.position) < distancUTurn && modePasFuite)
        {
            //on g�n�re si la proie fait son uturn vers la droite ou la gauche
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

        //si la proie a compl�tement touner en 180 degr�s, on la remet en ligne droite.
        if (modePasFuite == false && rotation > 180)
        {
            angleMax = 0;
        }

        //on compile dans la banque de rotation la rotation � chaque it�ration de la proie
        rotation += System.Math.Abs(Convert.ToSingle(360 * angleMax / (pi * 2)));
    }
}

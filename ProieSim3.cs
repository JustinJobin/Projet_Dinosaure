using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Code de la proie pour la simulation 3, je n'ai pas commenter puisque c'est un gros
//récapitulatif de toute les autres simulations. En effet, j'ai rassemblé toutes les
//stratégies de la proie et j'ai randomisé ces stratégies. Donc, il exécute une stratégie
//de manière aléatoire. Soit ligne droite, uturn ou bien zigzag. Pour les descriptions
//des stratégies, voir PredSim1 pour la ligne droite, voir PredSim2 pour le uturn et
//voir PredSim4 pour les zigzags.
public class ProieSim3 : MonoBehaviour
{
    private bool proieTouchable = true;
    private bool predGagne = false;
    private int compte = 15;//comme le chrono dans les autres simulations
    public Text message_Alerte;
    public Text Affichage_DistDebPred1;
    public Text Affichage_DistDebPred2;
    public Text Affichage_DistUturn;
    public Text Technique;

    private const float vitesseFuite = 13.9f;
    private float distancUTurn;

    private const float rayon = 5f;
    private const double pi = System.Math.PI;
    private float angleMax = 0;
    private GameObject pred1;
    private GameObject pred2;

    bool modePasFuite = true;
    private float rotation;
    private bool ligneDroite = false;
    private bool zigzag = false;
    private bool uturn = false;
    private bool premierIteration = true;

    //variable pour les zigzags
    private float angleMaxZigzag;
    private bool finPremierVirage;
    private float chrono;
    private float momentFinVirage;
    private float tempsAttProchVirage = 1f;
    private bool droitTourner;
    private bool premierVirage;
    private float currentAngle;
    private int nbVirages;
    private int nbVirageMax = 5;

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

        // choix de la stratégie
        int choix = UnityEngine.Random.Range(1, 4);

        StreamWriter EcrireTechnique = new StreamWriter("RésultatsSim3.txt", true);

        if (choix == 1)
        {
            ligneDroite = true;
            EcrireTechnique.Write("Ligne Droite".PadLeft(15));
            Technique.text = "Statégie : Ligne Droite";
        }
        else
            if (choix == 2)
        {
            zigzag = true;
            angleMaxZigzag = Convert.ToSingle(vitesseFuite * 0.05f / (2 * rayon));
            EcrireTechnique.Write("Zigzag".PadLeft(15));
            Technique.text = "Statégie : Zigzag";
        }
        else
            if (choix == 3)
        {
            uturn=true;
            EcrireTechnique.Write("UTurn".PadLeft(15));
            Technique.text = "Statégie : UTurn";
        }

        EcrireTechnique.Flush();
        EcrireTechnique.Close();

        pred1 = GameObject.Find("Pred");
        pred2 = GameObject.Find("Pred2");

        InvokeRepeating("Deplacement", 0, 0.05f);
        InvokeRepeating("chronometre", 0, 1);
    }

    private void chronometre()
    {
        if (compte >= 0)
        {
            compte--;
        }

        if (compte <= 0 && predGagne == false)
        {
            proieTouchable = false;
            message_Alerte.text = "Proie Win";
            Invoke("RetourAccueil", 2);
        }
    }

    private void Deplacement()
    {
        if(premierIteration)
        {
            premierIteration = false;

            float distancPred1 = 0;
            float distancPred2 = 0;

            distancPred1 = Vector3.Distance(transform.position, pred1.transform.position);
            distancPred2 = Vector3.Distance(transform.position, pred2.transform.position);

            distancPred1 = Convert.ToInt32(distancPred1);
            distancPred2 = Convert.ToInt32(distancPred2);

            distancUTurn = UnityEngine.Random.Range(0, Convert.ToInt32(Math.Min(distancPred1, distancPred2)) + 1);


            if(ligneDroite)
            {
                distancUTurn = 0;
            }

            Affichage_DistDebPred1.text = Affichage_DistDebPred1.text + distancPred1 + " m";
            Affichage_DistDebPred2.text = Affichage_DistDebPred2.text + distancPred2 + " m";
            Affichage_DistUturn.text = Affichage_DistUturn.text + distancUTurn + " m";

            StreamWriter EcrireDistanceDebut = new StreamWriter("RésultatsSim3.txt", true);
            EcrireDistanceDebut.Write(Convert.ToString(distancPred1).PadLeft(4) + Convert.ToString(distancPred2).PadLeft(4)
                + Convert.ToString(distancUTurn).PadLeft(4));
            EcrireDistanceDebut.Flush();
            EcrireDistanceDebut.Close();
        }

        //si la stratégie est la ligne droite
        if(ligneDroite)
        {
            transform.Translate(Vector3.forward * 0.05f * vitesseFuite);
        }

        //si la stratégie est le uturn
        if (uturn)
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

            if ((Vector3.Distance(transform.position, pred1.transform.position) < distancUTurn || Vector3.Distance(transform.position, pred2.transform.position) < distancUTurn) && modePasFuite)
            {
                int x = UnityEngine.Random.Range(1, 3);
                if (x == 2)
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

        // si sa stratégie est les zigzags
        if(zigzag)
        {
            if (finPremierVirage)
            {
                if (chrono > momentFinVirage + (tempsAttProchVirage / 2) && !modePasFuite)
                {
                    finPremierVirage = false;
                    droitTourner = true;
                }
            }
            else
            {
                if (chrono > momentFinVirage + tempsAttProchVirage && !modePasFuite)
                {
                    droitTourner = true;
                }
            }


            if ((Vector3.Distance(transform.position, pred1.transform.position) < distancUTurn || Vector3.Distance(transform.position, pred2.transform.position) < distancUTurn) && modePasFuite)
            {
                modePasFuite = false;
                premierVirage = true;
                currentAngle = angleMaxZigzag;
            }


            if (currentAngle == 0)
            {
                transform.Translate(new Vector3(0, 0, 1) * 0.05f * vitesseFuite);
            }
            else
            {
                if (droitTourner)
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
                    currentAngle = -angleMaxZigzag;
                    rotation = 0;
                    premierVirage = false;
                    finPremierVirage = true;
                    momentFinVirage = chrono;
                    droitTourner = false;
                    nbVirages++;
                }

                if (rotation > 90)
                {
                    currentAngle = -angleMaxZigzag;
                    rotation = 0;
                    momentFinVirage = chrono;
                    droitTourner = false;
                    nbVirages++;
                }
                else
                if (rotation < -90)
                {
                    currentAngle = angleMaxZigzag;
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
            chrono += 0.05f;
        }
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
}

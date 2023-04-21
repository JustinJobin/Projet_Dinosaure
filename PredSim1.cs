using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


//Code pour le prédateur de la simulation 1
public class PredSim1 : MonoBehaviour
{
    public Text message;
    public Text Affichage_Distance;
    private const float vitesseChasse = 16.7f;
    private float chrono = 15;
    private float distanceDebut = 0;
    private GameObject proie;
    private bool proieTouchable = true;
    private bool predGagne = false;

    private void Start()
    {
        //on randomise la distance entre les deux
        proie = GameObject.Find("Proie");
        distanceDebut = Random.Range(15, 50);
        Affichage_Distance.text = Affichage_Distance.text + distanceDebut + " m";
        Vector3 vecteurDistance = new Vector3(distanceDebut, 0, 0);
        proie.transform.position = transform.position + vecteurDistance;
        InvokeRepeating("chronometre", 0, 0.05f);
        
    }
    private void chronometre()
    {
        if (chrono >= 0)
        {
            chrono = chrono - 0.05f;
        }

        if (chrono <= 0 && predGagne == false)
        {
            //StreamWriter EcrireDefaite = new StreamWriter("RésultatsSim1.txt", true);
            //EcrireDefaite.WriteLine("Proie " + 0 + " " + Vector3.Distance(proie.transform.position, transform.position) + " " + distanceDebut);
            //EcrireDefaite.Flush();
            //EcrireDefaite.Close();
            proieTouchable = false;
            message.text = "Proie Win";
            Invoke("RetourAccueil", 2);
        }
    }

    //Si le prédateur touche la proie, on affiche que le prédateur a gagné
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "proie" && proieTouchable)
        {
            predGagne = true;
            message.text = "Prédateur win";
            //StreamWriter EcrireVictoire = new StreamWriter("RésultatsSim1.txt",true);
            //EcrireVictoire.WriteLine("Preda " + (15 - chrono) + " " + 0 + " " + distanceDebut);
            //EcrireVictoire.Flush();
            //EcrireVictoire.Close();
            //Invoke("RetourEchantillon", 0);
            Invoke("RetourAccueil", 2);
        }
    }

    //déplacement du prédateur à chaque itération
    void Update()
    {
        //translation du prédateur dans sa direction fois sa vitesse fois temps entre chaque itération
        transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * vitesseChasse);
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

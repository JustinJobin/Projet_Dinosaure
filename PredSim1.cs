using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


//Code pour le pr�dateur de la simulation 1
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
            //StreamWriter EcrireDefaite = new StreamWriter("R�sultatsSim1.txt", true);
            //EcrireDefaite.WriteLine("Proie " + 0 + " " + Vector3.Distance(proie.transform.position, transform.position) + " " + distanceDebut);
            //EcrireDefaite.Flush();
            //EcrireDefaite.Close();
            proieTouchable = false;
            message.text = "Proie Win";
            Invoke("RetourAccueil", 2);
        }
    }

    //Si le pr�dateur touche la proie, on affiche que le pr�dateur a gagn�
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "proie" && proieTouchable)
        {
            predGagne = true;
            message.text = "Pr�dateur win";
            //StreamWriter EcrireVictoire = new StreamWriter("R�sultatsSim1.txt",true);
            //EcrireVictoire.WriteLine("Preda " + (15 - chrono) + " " + 0 + " " + distanceDebut);
            //EcrireVictoire.Flush();
            //EcrireVictoire.Close();
            //Invoke("RetourEchantillon", 0);
            Invoke("RetourAccueil", 2);
        }
    }

    //d�placement du pr�dateur � chaque it�ration
    void Update()
    {
        //translation du pr�dateur dans sa direction fois sa vitesse fois temps entre chaque it�ration
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

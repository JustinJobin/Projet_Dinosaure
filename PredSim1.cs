using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class PredSim1 : MonoBehaviour
{
    public Text message;
    private const float vitesseNormale = 1f;
    private const float vitesseChasse = 16.7f;
    private int chrono = 15;
    private float distanceDebut = 0;
    private GameObject proie;

    private void Start()
    {
        proie = GameObject.Find("Proie");
        distanceDebut = Random.Range(15, 50);
        Vector3 vecteurDistance = new Vector3(distanceDebut, 0, 0);
        proie.transform.position = transform.position + vecteurDistance;
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
        //    StreamWriter EcrireDefaite = new StreamWriter("RésultatsSim1.txt", true);
        //    EcrireDefaite.WriteLine("Proie " + 0 + " " + Vector3.Distance(proie.transform.position, transform.position) + " " + distanceDebut);
        //    EcrireDefaite.Flush();
        //    EcrireDefaite.Close();
        //    Invoke("RetourEchantillon", 0);
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="proie")
        {
            message.text = "Prédateur win";
            //StreamWriter EcrireVictoire = new StreamWriter("RésultatsSim1.txt",true);
            //EcrireVictoire.WriteLine("Preda " + (15 - chrono) + " " + 0 + " " + distanceDebut);
            //EcrireVictoire.Flush();
            //EcrireVictoire.Close();
            //Invoke("RetourEchantillon", 0);
            Invoke("RetourAccueil", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (chrono != 0)
        {
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * vitesseChasse);
        }
        else
        {
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * vitesseNormale);
        }
    }

    private void RetourAccueil()
    {
        SceneManager.LoadScene("Scene1");
    }

    private void RetourEchantillon()
    {
        SceneManager.LoadScene("Intermediaire");
    }
}

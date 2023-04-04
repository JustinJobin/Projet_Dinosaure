using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PredSim1 : MonoBehaviour
{
    public Text message;
    private const float vitesseNormale = 1f;
    private const float vitesseChasse = 16.7f;
    private int chrono = 15;

    private void Start()
    {
        InvokeRepeating("chronometre", 0, 1);
    }
    private void chronometre()
    {
        if (chrono > 0)
        {
            chrono--;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="proie")
        {
            message.text = "Prédateur win";
            Invoke("RetourAccueil", 2);
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
}

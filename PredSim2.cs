using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PredSim2 : MonoBehaviour
{
    public Text message_Alerte;
    private GameObject proie;
    private float angleMax = 0;
    private float currentAngle = 0;
    private float currentAngleSign = 0;
    private Vector3 targetDir;

    //private const float vitesseNormale = 1f;
    private const float vitesseChasse = 16.7f;
    private const float rayon = 0.75f;
    //private const float rayon = 5f;
    private const double pi = System.Math.PI;

    // Start is called before the first frame update
    void Start()
    {
        proie = GameObject.Find("Proie");
        transform.position.Set(5, 0, 100);
        InvokeRepeating("Mouvement", 0, 0.05f);
    }

    private void Mouvement()
    {
        angleMax = Convert.ToSingle(vitesseChasse * 0.05 / (2 * rayon));//radians
        targetDir = proie.transform.position - transform.position;
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
        //transform.Rotate(new Vector3(0, Convert.ToSingle(360 * currentAngle / (pi * 2)), 0));
        //transform.Translate(new Vector3(0, 0, 1) * Convert.ToSingle(2 * rayon * System.Math.Sin(currentAngle)));
        //transform.LookAt(proie.transform.position);
        //transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * vitesseChasse);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "proie")
        {
            message_Alerte.text = "Prédateur win";
            Invoke("RetourAccueil", 2);
        }
    }

    private void RetourAccueil()
    {
        SceneManager.LoadScene("Scene1");
    }

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
}

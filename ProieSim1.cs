using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code de la proie de la simulation 1
public class ProieSim1 : MonoBehaviour
{
    private const float vitesseFuite = 13.9f;//sa vitesse

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * vitesseFuite);//son déplacement direction fois
                                                                             //vitesse fois temps entre les itérations
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class FPC : FirstPersonController
{

    public Image compass;
    
    public new void Update()
    {
        Vector3 look =  Vector3.up;
        float angle = 0.0f;
        this.gameObject.transform.rotation.ToAngleAxis(out angle, out look);


        //Debug.Log(look);
        //Debug.Log(angle);
        if(look.y < 0)
        {
            compass.rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {

            compass.rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }
        base.Update();


    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("hit Goal");
            SceneManager.LoadScene("win");
        }


    }
}


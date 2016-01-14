using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPC : FirstPersonController
{

    public new void Update()
    {
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


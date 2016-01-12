using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPC :  FirstPersonController
    {

public new void Update()
    {
        base.Update();

        foreach (Enemy en in FindObjectsOfType<Enemy>())
        {
            en.brodcastPosition(this.getPosition());
        }

    }

    private Vector3 getPosition()
    {
        Vector3 position = new Vector3();
        int x = (int)this.gameObject.transform.localPosition.x;
        position.x = x + 0.5f;
        int z = (int)this.gameObject.transform.localPosition.z;
        position.z = z + 0.5f;

        return position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("hit Goal");
            SceneManager.LoadScene("win");
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hit Enemy");
            SceneManager.LoadScene("lose");
        }
       
    }
}


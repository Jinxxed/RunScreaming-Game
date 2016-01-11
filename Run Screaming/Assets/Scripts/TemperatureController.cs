using UnityEngine;
using System.Collections;

public class Temperature : MonoBehaviour
{
    public float temperature { get; private set; }

    void Start()
    {
        StartCoroutine(Heat());
    }

    IEnumerator Cool()
    {
        while (true)
        {
            yield return new WaitForSeconds(30);
            temperature -= 1;
        }
    }

    IEnumerator Heat()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            temperature += 1;
        }
    }

}
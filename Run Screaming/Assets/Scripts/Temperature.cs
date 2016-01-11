using UnityEngine;
using UnityEngine.SceneManagement;

public class Temperature : MonoBehaviour
{
    float timer = 0;
    float end = 180;
    int temperature = 7;
    int threashold = 10;

    void Start()
    {
        
    }
 
    void Update()
    {
        timer += Time.deltaTime;
    }

    void OnGUI()
    {
        if (timer >= end)
        {
            SceneManager.LoadScene("lose");
        }
        else
        {
            int newTemperature = temperature + ((int)timer / threashold);
            GUI.Label(new Rect(20, 20, 200, 100),
                                    "Beer temperature: " + newTemperature);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Temperature : MonoBehaviour
{
    public Image base_temperature_background;
    public Image current_temperature;
    public Sprite[] temperature_levels;

    float timer = 0;
    float end = 15;
    int temperature = 7;
    int threashold = 15;
    int state = 0;

    void Start()
    {
        base_temperature_background.GetComponent<CanvasRenderer>().SetAlpha(0.25f);
    }
 
    void Update()
    {
        timer += Time.deltaTime;
    }

    void OnGUI()
    {
        //TODO change the current_temperature image to next level...

            float newTemperature = temperature + (timer / threashold);
            GUI.Label(new Rect(20, 20, 200, 100),
                                    string.Format("Beer temperature: {0,4:F2}°C",newTemperature));

        state = 2;

        Sprite spr = temperature_levels[state];
        current_temperature.sprite = spr;
        Vector2 new_size;
        switch(state)
        {
            case 1:
                new_size = new Vector2(25, 40);
                break;
            case 2:
                new_size = new Vector2(25, 50);
                break;
            case 3:
                new_size = new Vector2(25, 60);
                break;
            case 4:
                new_size = new Vector2(25, 60);
                break;
            case 5:
                new_size = new Vector2(25, 60);
                break;
            case 6:
                new_size = new Vector2(25, 60);
                break;
            default:
                new_size = new Vector2(25, 20);
                break;
    }
        current_temperature.rectTransform.sizeDelta = new_size;




        if (newTemperature > end)
        {
            SceneManager.LoadScene("lose");
        }
    }
}
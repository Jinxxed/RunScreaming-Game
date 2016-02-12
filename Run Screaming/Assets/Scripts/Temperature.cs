using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Temperature : MonoBehaviour
{
    public Image base_temperature_background;
    public Image current_temperature;
    public Sprite[] temperature_levels;

    float timer = 0;
    float end = 180;
    int state = 0;

    void Start()
    {
        base_temperature_background.GetComponent<CanvasRenderer>().SetAlpha(0.25f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 150)
        {
            state = 5;
        } else if (timer > 120) {
            state = 4;
        } else if (timer > 90)
        {
            state = 3;
        } else if (timer > 60) {
            state = 2;
        } else if (timer > 30)
        {
            state = 1;
        }
        
    }

    void OnGUI()
    {
        //TODO change the current_temperature image to next level

        Sprite spr = temperature_levels[state];
        current_temperature.sprite = spr;
        Vector2 new_size;
        switch(state)
        {
            case 1:
                new_size = new Vector2(25, 37);
                break;
            case 2:
                new_size = new Vector2(25, 52);
                break;
            case 3:
                new_size = new Vector2(25, 68);
                break;
            case 4:
                new_size = new Vector2(25, 83);
                break;
            case 5:
                new_size = new Vector2(25, 100);
                break;
            default:
                new_size = new Vector2(25, 20);
                break;
    }
        current_temperature.rectTransform.sizeDelta = new_size;

        if (timer > end)
        {
            SceneManager.LoadScene("lose");
        }
    }
}
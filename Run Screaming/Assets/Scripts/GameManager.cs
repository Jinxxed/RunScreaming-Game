using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{

    public Maze mazePrefab;
    public Camera mainCamera;

    private Maze mazeInstance;

    private void takeScreenshot()
    {
        int width = (mazeInstance.size.x + 20) * 20;
        int height = (mazeInstance.size.z + 20) * 20;

        mainCamera.pixelRect = new Rect(0, 0, width, height);
        RenderTexture rt = new RenderTexture(width, height, 24);

        mainCamera.targetTexture = rt;


        RenderTexture.active = rt;

        mainCamera.Render();
        mainCamera.targetTexture = null;

        Texture2D screen = new Texture2D(width, height);
        screen.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        byte[] bytes = screen.EncodeToPNG();

        string filename = Application.dataPath + "/MazeScreen.png";
        System.IO.File.WriteAllBytes(filename, bytes);

        Debug.Log("Created Maze Plot, saved it to " + filename);
    }

    private void Start()
    {
        BeginGame();
        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartGame();
        }
    }

    public void switchCamera()
    {
        if (mazeInstance == null)
        {
            throw new System.Exception("Maze is null!");

        }
        GameObject fps = FindObjectOfType<FirstPersonController>().gameObject;
        if (fps == null)
        {
            throw new System.Exception("Camera not found!");
        }
        if (mainCamera.enabled)
        {
            mainCamera.enabled = false;
            fps.GetComponentInChildren<Camera>().enabled = true;
        }
        else
        {
            fps.GetComponentInChildren<Camera>().enabled = false;
            mainCamera.enabled = true;
            mainCamera.gameObject.SetActive(true);
        }

    }

    private void BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Generate();
        takeScreenshot();
        mazeInstance.disableGoalObj();
    }

    private void RestartGame()
    {
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
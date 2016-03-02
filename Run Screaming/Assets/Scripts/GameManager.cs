using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;

    private void takeScreenshot(string filename)
    {


        int width = 800;
        int height = 800;

        //mainCamera.pixelRect = new Rect(0, 0, width, height);
        RenderTexture rt = new RenderTexture(width, height, 24);
        mainCamera.backgroundColor = Color.white;

        mainCamera.targetTexture = rt;

        RenderTexture.active = rt;

        mainCamera.Render();
        mainCamera.targetTexture = null;

        Texture2D screen = new Texture2D(width, height);
        screen.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        byte[] bytes = screen.EncodeToPNG();


        RenderTexture.active = null;

        string fullFilename = Application.dataPath + "/mazePlots/" + filename + ".png";
        
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {

            System.IO.File.Delete(fullFilename);
            System.IO.File.WriteAllBytes(fullFilename, bytes);
        }
        
        

        Debug.Log("Created Maze Plot '" + filename + "', saved it to " + fullFilename);

    }

    private void Start()
    {
        BeginGame();
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void switchCamera()
    {
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
        //spawn randomly
        GameObject fpc = FindObjectOfType<FirstPersonController>().gameObject;
        if (fpc == null)
        {
            throw new System.Exception("Camera not found!");
        }

        float xPos = ((int) UnityEngine.Random.Range(-10, 9)) + 0.5f;
        float yPos = ((int) UnityEngine.Random.Range(-10, 9)) + 0.5f;

        fpc.transform.position = new Vector3(xPos, 1, yPos);


        #region PLOTS
        GameObject decoObjects = GameObject.Find("decoObjects");
        GameObject barriersObjects = GameObject.Find("barriers");
        GameObject maze = GameObject.Find("Maze(Clone)");
        GameObject goal = GameObject.Find("Goal(Clone)");
        GameObject grid = GameObject.Find("grid");
        GameObject directionalLight = GameObject.Find("Directional Light");

        if (decoObjects == null || barriersObjects == null || goal == null ||
            maze == null || grid == null || directionalLight == null)
        {
            throw new System.Exception("Not all needed objects found! (decoObjects, barriers, Maze(Clone), Goal(Clone), grid, Directional Light)");
        }

        fpc.SetActive(false);
        Quaternion lightRotationBackup = directionalLight.transform.rotation;

        directionalLight.transform.rotation = Quaternion.Euler(90, 0, 0);

        //change y position of grid, so that it is visible
        grid.transform.position = new Vector3(0, 0, 0);



        //show maze only
        decoObjects.SetActive(false);
        barriersObjects.SetActive(false);
        maze.SetActive(true);
        goal.SetActive(true);
        grid.SetActive(false);

        takeScreenshot("mazeOnly");

        //objects only
        decoObjects.SetActive(true);
        barriersObjects.SetActive(false);
        maze.SetActive(false);
        goal.SetActive(false);
        grid.SetActive(true);

        takeScreenshot("objectsOnly");

        //barriers only
        decoObjects.SetActive(false);
        barriersObjects.SetActive(true);
        maze.SetActive(false);
        goal.SetActive(false);
        grid.SetActive(true);

        takeScreenshot("barriersOnly");

        //RESET playable settings
        fpc.SetActive(true);
        decoObjects.SetActive(true);
        barriersObjects.SetActive(true);
        maze.SetActive(true);
        goal.SetActive(true);
        grid.SetActive(false);

        takeScreenshot("debugPlot");

        directionalLight.transform.rotation = lightRotationBackup;
        #endregion

    }

    private void RestartGame()
    {
        BeginGame();
    }
}

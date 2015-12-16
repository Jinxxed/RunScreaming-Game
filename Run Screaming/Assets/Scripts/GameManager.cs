using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;
    public Camera mainCamera;

	private Maze mazeInstance;

	private void Start () {
		BeginGame();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
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
        if(mainCamera.enabled)
        {
            fps.GetComponentInChildren<Camera>().enabled = true;
            mainCamera.enabled = false;
            mainCamera.gameObject.SetActive(false);

        }
        else
        {
            mainCamera.enabled = true;
            mainCamera.gameObject.SetActive(true);
            fps.GetComponentInChildren<Camera>().enabled = false;
        }
        
    }

	private void BeginGame () {
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Generate();
	}

	private void RestartGame () {
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    //public GameObject loadingScreen;
    public CameraZoomOut cameraZoomOut;
    public void PlayGame()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        cameraZoomOut.ZoomOut();
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        operation.allowSceneActivation = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
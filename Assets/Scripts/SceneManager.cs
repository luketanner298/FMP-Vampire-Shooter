using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Exit()
    {
        // If we are in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        // If we are in a standalone build
#else
            Application.Quit();
#endif
    }

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log("Current Scene Name: " + currentScene.name);


    }

}
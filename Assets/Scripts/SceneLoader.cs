using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class endLevel : MonoBehaviour
{

    // The name of the next scene to load
    public string nextSceneName;

    private void OnCollisionEnter(Collision other)
    {
        // Check if the object entering the trigger is the player
        if (other.gameObject.tag == "Player")
        {
            // Delay for visual effect (optional)
            Invoke("LoadNextLevel", 2f);
        }
    }

    // Function to load the next scene
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }

}
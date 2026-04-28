using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public string SceneName;
    public BoxCollider Collider;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Win")
        {
            SceneManager.LoadScene(SceneName);
        }

    }


}
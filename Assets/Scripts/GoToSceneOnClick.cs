using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Button
using UnityEngine.SceneManagement; // SceneManager

public class GoToSceneOnClick : MonoBehaviour 
{
    public int sceneIndex; // Scene to be loaded
    
    void Start()
    {
		//Add On Click Event for the button
        GetComponent<Button> ().onClick.AddListener (() => {
            ClickEvent ();
        });
    }
    
    void ClickEvent()
    {
		// Switch Scene
        SceneManager.LoadScene (sceneIndex);
    }
}
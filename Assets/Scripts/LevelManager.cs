using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void LoadScene(string scene)
    {      
        SceneManager.LoadScene(scene);     
    }
	
    public void QuitGame()
    {
        Application.Quit();
    }
	
}

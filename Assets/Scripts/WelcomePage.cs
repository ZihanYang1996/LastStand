using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomePage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void QuitGame()
    {
        #if (UNITY_WEBGL)
        Application.ExternalEval("window.location.href = 'about:blank';");
        #else
        Application.Quit();
        #endif
    }
}

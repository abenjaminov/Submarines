using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("Scenes/Subs");
    }

    public void OnOptionsClicked()
    {
        
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
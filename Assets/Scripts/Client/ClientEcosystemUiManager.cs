using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientEcosystemUiManager : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene ("ViewerScreen");
    }
}

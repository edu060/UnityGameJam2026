using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Lets Play!");
        // Carregar a cena que a gente vai usar.
        // Possível implementação:
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        //SceneManager.LoadScene("Gun Testing");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit!");

        Application.Quit();
    }

}

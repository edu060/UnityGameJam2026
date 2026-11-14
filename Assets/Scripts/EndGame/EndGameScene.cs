using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScene : MonoBehaviour
{
    [Header("End Game Results")]
    [SerializeField] private TMP_Text _resultText;

    [Header("Buttons")]
    [SerializeField] private Button _newGame;
    [SerializeField] private Button _mainMenu;
    [SerializeField] private Button _quitGame;

    private void Start()
    {
        _newGame.onClick.AddListener(NewGame);
        _mainMenu.onClick.AddListener(MainMenu);
        _quitGame.onClick.AddListener(QuitGame);

        ShowResultText();
    }

    private void ShowResultText()
    {
        string newText = $"Results\n" +
            $"Total Score: {PointSystemData.LastTotalPoints}\n" +
            $"Max Score: {PointSystemData.MaxTotalPoints}";
    }

    private void NewGame()
    {
        SceneManager.LoadScene("L_ShopingMall");
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("L_MainMenu");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}

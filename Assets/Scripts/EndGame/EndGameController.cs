using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private EnemySpawnerManager _enemySpawnerManager;

    private void OnEnable()
    {
        if(_player == null)
            _player = GameObject.FindGameObjectWithTag("Player");

        _player.GetComponent<HealthController>().OnDie += GoToEndGameScreen;

    }

    private void GoToEndGameScreen()
    {
        PointSystemData.LastTotalPoints = _enemySpawnerManager.EnemiesKilledAmount * PointSystemData.PointsPerKill;

        if(PointSystemData.LastTotalPoints >= PointSystemData.MaxTotalPoints)
        {
            PointSystemData.MaxTotalPoints = PointSystemData.LastTotalPoints;
        }

        SceneManager.LoadScene("EndGameScene");
    }
}

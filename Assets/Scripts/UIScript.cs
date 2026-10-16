using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemiesKilledText;


    // Update is called once per frame
    private void FixedUpdate()
    {
        enemiesKilledText.text = ("You Killed " + WaveSystem.Instance.totalEnemiesKilled + " Enemies");
    }


    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemiesKilledText;


    // Update is called once per frame
    private void FixedUpdate()
    {
        enemiesKilledText.text = ("You Killed " + WaveSystem.Instance.enemiesKilled + " Enemies");
    }
}

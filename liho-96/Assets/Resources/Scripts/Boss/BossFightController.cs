using TMPro;
using UnityEngine;

public class BossFightController : MonoBehaviour
{
    public float fightSeconds = 120;
    public TextMeshProUGUI timer;

    private float _timeRemaining;

    private void Start()
    {
        _timeRemaining = fightSeconds;
    }

    private void Update()
    {
        UpdateTimeRemaining();
    }

    private void UpdateTimeRemaining()
    {
        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(_timeRemaining / 60);
            float seconds = Mathf.FloorToInt(_timeRemaining % 60);
            timer.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            Debug.Log("TIME RAN OUT!");
        }
    }
}
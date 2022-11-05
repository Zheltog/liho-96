using TMPro;
using UnityEngine;

public class BossFightController : MonoBehaviour
{
    public float fightSeconds = 120;
    public float activePhaseSeconds = 10;
    public TextMeshProUGUI timer;
    public Courier courier;

    public BossFightFrameTextController text;

    private float _timeRemainingBeforeEnd;
    private float _timeRemainingBeforeNextRest;

    private void Start()
    {
        _timeRemainingBeforeEnd = fightSeconds;
        _timeRemainingBeforeNextRest = activePhaseSeconds;
    }

    private void Update()
    {
        UpdateTimeRemainingBeforeNextRest();
        UpdateTimeRemainingBeforeEnd();
    }

    public void ReturnToActivePhase()
    {
        courier.ReturnToActivePhase();
    }

    private void UpdateTimeRemainingBeforeNextRest()
    {
        if (_timeRemainingBeforeNextRest > 0)
        {
            _timeRemainingBeforeNextRest -= Time.deltaTime;
        }
        else
        {
            Rest();
            _timeRemainingBeforeNextRest = activePhaseSeconds;
        }
    }

    private void UpdateTimeRemainingBeforeEnd()
    {
        if (_timeRemainingBeforeEnd > 0)
        {
            _timeRemainingBeforeEnd -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(_timeRemainingBeforeEnd / 60);
            float seconds = Mathf.FloorToInt(_timeRemainingBeforeEnd % 60);
            timer.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            Debug.Log("TIME RAN OUT!");
        }
    }

    private void Rest()
    {
        courier.Rest();
        text.NewText("Отдыхаем, мужики");
        // TODO: choice
    }
}
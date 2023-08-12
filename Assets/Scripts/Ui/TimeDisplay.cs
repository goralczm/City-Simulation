using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;

    private DayCycle.Time _time;

    private void Start()
    {
        _time = DayCycle.Time.Instance;
    }

    private void Update()
    {
        int hour = Mathf.FloorToInt(_time.time);
        string minute = _time.time.ToString("n2").Substring(2, 2);
        _timeText.SetText($"{hour}:{minute}");
    }
}

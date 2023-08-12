using UnityEngine;

public class Interactable : MonoBehaviour
{
    public MoodletType rewardMoodlet;
    public float rewardValue;
    public float duration;

    private void Awake()
    {
        InteractablesManager.Instance.interactables.Add(this);
    }
}

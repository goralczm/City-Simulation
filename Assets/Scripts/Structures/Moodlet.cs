using UnityEngine;

public enum MoodletType
{
    Food,
    Thirst,
    Social
}

[System.Serializable]
public class Moodlet
{
    public MoodletType type;
    public float value;
    public AnimationCurve weight;
}
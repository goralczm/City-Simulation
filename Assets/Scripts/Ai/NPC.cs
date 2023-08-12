using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private List<Moodlet> _moodlets;
    private Queue<Interactable> _activities;

    private bool _isBusy;

    private InteractablesManager _interactablesManager;
    private DayCycle.Time _time;

    private void Start()
    {
        _time = DayCycle.Time.Instance;
        _interactablesManager = InteractablesManager.Instance;

        _activities = new Queue<Interactable>();
    }

    private void Update()
    {
        foreach (Moodlet moodlet in _moodlets)
        {
            moodlet.value -= Time.deltaTime / _time.timeAcceleration;
            if (moodlet.value < -100)
            {
                Destroy(gameObject);
            }
        }

        if (_isBusy)
            return;

        if (_activities.Count == 0)
        {
            GetActivity();
            return;
        }

        Interactable interactable = _activities.Dequeue();
        StartCoroutine(DoAction(interactable));
        transform.position = interactable.transform.position;
        _interactablesManager.interactables.Remove(interactable);
        Destroy(interactable.gameObject);

        IEnumerator DoAction(Interactable action)
        {
            _isBusy = true;
            Moodlet rewardMoodlet = _moodlets[0];
            foreach (Moodlet moodlet in _moodlets)
            {
                if (action.rewardMoodlet == moodlet.type)
                {
                    rewardMoodlet = moodlet;
                    break;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(action.duration / 10 * _time.timeAcceleration);
                rewardMoodlet.value += action.rewardValue / 10;
                if (rewardMoodlet.value > 100)
                    rewardMoodlet.value = 100;
            }
            _isBusy = false;
        }
    }

    private void GetActivity()
    {
        Dictionary<Interactable, float> weightedInteraction = new Dictionary<Interactable, float>();
        foreach (Interactable interactable in _interactablesManager.interactables)
        {
            Moodlet suitableMoodlet = _moodlets[0];
            bool isSuitable = false;
            foreach (Moodlet moodlet in _moodlets)
            {
                if (moodlet.type == interactable.rewardMoodlet)
                {
                    suitableMoodlet = moodlet;
                    isSuitable = true;
                    break;
                }
            }
            if (!isSuitable)
                continue;

            float weight = interactable.rewardValue;
            float distance = Vector2.Distance(transform.position, interactable.transform.position);
            weight /= distance;
            weight *= suitableMoodlet.weight.Evaluate(suitableMoodlet.value);

            weightedInteraction.Add(interactable, weight);
        }

        if (weightedInteraction.Count == 0)
            return;

        var sortedInteractions = weightedInteraction.OrderBy(kvp => kvp.Value);
        _activities.Enqueue(sortedInteractions.Last().Key);
    }
}

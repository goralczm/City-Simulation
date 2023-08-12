using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    #region Singleton

    public static InteractablesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public List<Interactable> interactables;
}

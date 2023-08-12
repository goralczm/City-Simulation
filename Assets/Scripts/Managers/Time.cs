using UnityEngine;

namespace DayCycle
{
    public class Time : MonoBehaviour
    {
        #region Singleton

        public static Time Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        public float time;

        [Range(1f, 60f)] public float timeAcceleration;

        private void Update()
        {
            time += UnityEngine.Time.deltaTime / timeAcceleration;
            if (time >= 24)
                time = 0;
        }
    }
}

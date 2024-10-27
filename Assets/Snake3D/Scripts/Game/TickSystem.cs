using System;
using UnityEngine;

namespace Snake3D.Game
{
    public class TickSystem
    {
        public class OnTickEventArgs : EventArgs
        {
            public int tick;
        }
        
        public static event EventHandler<OnTickEventArgs> OnTick;

        public static float tickTimerMax = 1f;
        private static GameObject tickSystemObject;
        private static int tick;

        public static void Init()
        {
            if (tickSystemObject == null)
            {
                tickSystemObject = new GameObject("TickSystem");
                tickSystemObject.AddComponent<TickSystemObject>();
            }
        }

        public static int GetTick()
        {
            return tick;
        }
        
        private class TickSystemObject : MonoBehaviour
        {
            private float tickTimer;

            private void Awake()
            {
                tick = 0;
            }

            private void Update()
            {
                tickTimer += Time.deltaTime;
                if (tickTimer >= tickTimerMax)
                {
                    tickTimer -= tickTimerMax;
                    tick++;
                    if(OnTick != null) OnTick(this, new OnTickEventArgs { tick = tick });
                    Debug.Log("Tick");
                }
            }
        }
    }
}
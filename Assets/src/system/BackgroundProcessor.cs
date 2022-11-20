using System;
using UnityEngine;

namespace Game.System.Threads
{
    public class BackgroundComponent<T> : IComponent
    {
        private readonly Action action;
        private readonly int tickFrequency;
        private int elapsed = 0;

        public BackgroundComponent(Action action, int tickFrequency)
        {
            this.action = action;
            this.tickFrequency = tickFrequency;
        }

        public void Tick(GameSession session)
        {
            try
            {
                if (elapsed < tickFrequency) elapsed++;
                else
                {
                    elapsed = 0;
                    action();
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Exception occured from: " + typeof(T));
                Debug.LogException(ex);
                Debug.Log("Unregistering component" + typeof(BackgroundComponent<T>));
                session.Unregister<BackgroundComponent<T>>();
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SaveSystem.Runtime
{
    [CreateAssetMenu(menuName = "BerkcanKarabulut/SaveSystem/EventChannels/Saveable List Event Channel")]
    public class SaveableListEventChannelSO : ScriptableObject
    {
        public UnityAction<List<SaveableEntity>> onEventRaised;

        public void RaiseEvent(List<SaveableEntity> saveables)
        {
            if (onEventRaised != null)
                onEventRaised.Invoke(saveables);
        }
    }
}
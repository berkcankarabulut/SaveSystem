using System;
using System.Collections.Generic;
using EventBusHandler.Runtime;
using UnityEngine; 

namespace SaveHandler.Runtime
{
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = Guid.NewGuid().ToString();
        
        private List<ISaveable> saveableComponents;
 
        private void Awake()
        {
            saveableComponents = new List<ISaveable>(GetComponents<ISaveable>());
        }
        
        private void OnEnable()
        {  
            EventBus.Instance.Subscribe<RequestSaveablesEvent>(RegisterSaveable);
        } 
        private void OnDisable()
        { 
            EventBus.Instance.Unsubscribe<RequestSaveablesEvent>(RegisterSaveable);
        } 
        
        private void RegisterSaveable(RequestSaveablesEvent eventData)
        { 
            if (!eventData.Saveables.Contains(this)) eventData.Saveables.Add(this);
        }

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>(); 
            foreach (ISaveable saveable in saveableComponents)
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in saveableComponents)
            {
                string typeName = saveable.GetType().ToString();
                if (stateDict.TryGetValue(typeName, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
    }
}
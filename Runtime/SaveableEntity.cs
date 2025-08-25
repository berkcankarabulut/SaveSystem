using System.Collections.Generic; 
using UnityEngine; 

namespace SaveSystem.Runtime
{
  [ExecuteInEditMode]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        [Header("Listening On")]
        [SerializeField] private SaveableListEventChannelSO _onSaveablesRequested;

        static Dictionary<string, SaveableEntity> guidContainer = new Dictionary<string, SaveableEntity>();

        private void OnEnable()
        {
            _onSaveablesRequested.onEventRaised += OnSaveableRequested;
        }
        private void OnDisable()
        {
            _onSaveablesRequested.onEventRaised -= OnSaveableRequested;
        }

        private void OnSaveableRequested(List<SaveableEntity> saveables)
        {
            if (!saveables.Contains(this)) saveables.Add(this);
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            foreach (ISaveable saveable in GetOrderedSaveables())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            } 
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetOrderedSaveables())
            {
                string dataType = saveable.GetType().ToString();
                if (stateDict.ContainsKey(dataType))
                {
                    saveable.RestoreState(stateDict[dataType]);
                }
            }
        }

        private List<ISaveable> GetOrderedSaveables()
        {
            return new List<ISaveable>(GetComponents<ISaveable>());
        }

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

#if UNITY_EDITOR
        private void Update()
        {

            if (string.IsNullOrEmpty(gameObject.scene.path)) return;
            if (!Application.isEditor) return;


            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            UnityEditor.SerializedProperty property = serializedObject.FindProperty(nameof(uniqueIdentifier));

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            guidContainer[property.stringValue] = this;

        }
#endif
        private bool IsUnique(string newGuid)
        {
            if (!guidContainer.ContainsKey(newGuid)) return true;

            if (guidContainer[newGuid] == this) return true;

            if (guidContainer[newGuid] == null)
            {
                guidContainer.Remove(newGuid);
                return true;
            }

            if (guidContainer[newGuid].GetUniqueIdentifier() != newGuid)
            {
                guidContainer.Remove(newGuid);
                return true;
            }

            return false;
        }

    }
}
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveSystem.Runtime
{
    public class SavingSystem : MonoBehaviour
    {
        [Header("Broadcasting On...")]
        [SerializeField] private SaveableListEventChannelSO _onSaveablesRequested;

        public void Save(string fileName)
        {
            Dictionary<string, object> state = LoadFile(fileName);
            CaptureState(state);
            SaveFile(fileName, state);
        }

        public void Load(string fileName)
        {
            RestoreState(LoadFile(fileName));
        }

        public void Delete(string fileName)
        {
            string path = GetPathFromSaveFile(fileName);
            File.Delete(path);
        }

        public void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, state);
            }
        }

        public Dictionary<string, object> LoadFile(string loadFile)
        {
            string path = GetPathFromSaveFile(loadFile);


            if (!File.Exists(path)) return new Dictionary<string, object>();

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                Dictionary<string, object> state = (Dictionary<string, object>)binaryFormatter.Deserialize(stream);

                return state;
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            List<SaveableEntity> saveables = new List<SaveableEntity>();
            _onSaveablesRequested.RaiseEvent(saveables);

            for (int i = 0; i < saveables.Count; i++)
            {
                string saveableID = saveables[i].GetUniqueIdentifier();
                if (state.ContainsKey(saveableID)) state[saveableID] = saveables[i].CaptureState();
                else state.Add(saveableID, saveables[i].CaptureState());
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            if (state == null) return;

            List<SaveableEntity> saveables = new List<SaveableEntity>();
            _onSaveablesRequested.RaiseEvent(saveables);

            for (int i = 0; i < saveables.Count; i++)
            {
                string saveableID = saveables[i].GetUniqueIdentifier();
                if (state.ContainsKey(saveableID))
                {
                    saveables[i].RestoreState(state[saveableID]);
                }
            }

            print("Save Loaded");
        }

        private string GetPathFromSaveFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}
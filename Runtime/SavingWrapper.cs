using System;
using System.Collections;
using UnityEngine;

namespace SaveHandler.Runtime
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] string defaultSaveFile = "Xsave";
        [SerializeField] private SavingSystem _savingSystem;
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space)) Save();
        }

        public void Load()
        { 
            _savingSystem.Load(defaultSaveFile);
        }

        public void Save()
        {
            _savingSystem.Save(defaultSaveFile);
        }

        public void Delete()
        {
            _savingSystem.Delete(defaultSaveFile);
        }

        private void OnApplicationFocus(bool focusStatus)
        {
            // if (!focusStatus) Save();
        }
    }
}

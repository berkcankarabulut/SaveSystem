using System.Collections.Generic;

namespace SaveHandler.Runtime
{
    public struct SaveGameEvent {}
    public struct RequestSaveablesEvent 
    {
        public List<SaveableEntity> Saveables;
    }
}
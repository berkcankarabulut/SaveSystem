namespace SaveSystem.Runtime
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}
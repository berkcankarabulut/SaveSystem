namespace SaveHandler.Runtime
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}
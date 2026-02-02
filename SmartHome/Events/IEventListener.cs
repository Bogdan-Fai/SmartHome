namespace SmartHome.Events
{
    public interface IEventListener
    {
        void OnEvent(string eventName, object? payload);
    }

    public interface ISensor
    {
        bool TryRead(out decimal value);
    }
}
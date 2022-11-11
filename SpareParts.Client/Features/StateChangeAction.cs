namespace SpareParts.Client.Features
{
    public class StateChangeAction<T>
    {
        public StateChangeAction(T payload)
        {
            Payload = payload;
        }

        public T Payload { get; }
    }
}

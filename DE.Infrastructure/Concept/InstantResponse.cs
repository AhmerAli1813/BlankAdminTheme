namespace DE.Infrastructure.Concept
{
    public class InstantResponse<T> : Response
    {
        public T Payload { get; set; }

        public InstantResponse(T payload)
        {
            this.Payload = payload;
        }
    }
}

namespace DE.Infrastructure.Memory
{
    public interface IMemoryCacher
    {
        object GetValue(string key);
        T GetValue<T>(string key);
        bool Add(string key, object value, int durationInMinutes);
        void Remove(string key);
    }
}

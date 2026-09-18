namespace findexit_test.Services.Interfaces;

public interface ICacheService
{
    T? Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan? expirationTime = null);
    void Remove(string key);
}
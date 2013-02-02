namespace JobSystem.DataModel
{
    public interface IEntityIdProvider
    {
        string GetIdFor<T>();
    }
}
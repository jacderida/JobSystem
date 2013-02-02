namespace JobSystem.DataModel.Repositories
{
    public interface IEntityIdLookupRepository
    {
        string GetNextId(string typeName);
    }
}
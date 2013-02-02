namespace JobSystem.DataModel.Repositories
{
    public interface IReadWriteRepository<T_Entity, T_Key>
    {
        void Create(T_Entity entity);
        T_Entity GetById(T_Key id);
        void Update(T_Entity entity);
    }
}
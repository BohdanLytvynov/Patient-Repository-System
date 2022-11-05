namespace CRUDControllerLib.Interfaces
{
    public interface ICRUDController<TEntityVisual, TEntityStorage, TEntitySearchArgs>
    {
        Task AddAsync(TEntityStorage entity, IList<TEntityStorage> col);

        Task RemoveAsync(TEntityVisual entity, IList<TEntityStorage> col);

        Task EditAsync(TEntityVisual entity, IList<TEntityStorage> col);

        Task SearchAsync(IList<TEntityStorage> col, TEntitySearchArgs args);

        Task GetAllNotesAsync(IList<TEntityStorage> col);
        
    }
}
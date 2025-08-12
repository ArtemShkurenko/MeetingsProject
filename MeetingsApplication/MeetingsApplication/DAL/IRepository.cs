using MeetingsApplication.Models;

namespace MeetingsApplication.DAL
{
    public interface IRepository <TEntity>
        where TEntity : IRecord
    {
        TEntity GetRecordById(int Id);
        IEnumerable<TEntity> GetAll();
        void Create(TEntity entity);
    }
}

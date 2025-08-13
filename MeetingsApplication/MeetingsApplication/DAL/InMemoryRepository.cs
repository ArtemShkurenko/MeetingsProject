using MeetingsApplication.Models;
using System.Text.Json;

namespace MeetingsApplication.DAL
{
    public class InMemoryRepository<TEntity> : IRepository<TEntity>
        where TEntity : IRecord,new()
    {
        public List<TEntity> _records = new List<TEntity>();
        private int idCounter         = 1;
        internal TEntity DeepCopy(TEntity entity)
        {
            var json = JsonSerializer.Serialize(entity);
            return JsonSerializer.Deserialize<TEntity>(json);
        }
        public TEntity GetRecordById(int Id)
        {
            var entity = _records.FirstOrDefault(x => x.Id.Equals(Id));
            return entity;
        }
        public void Create(TEntity entity)
        {
            var entityCopy     = DeepCopy(entity);
                entityCopy.Id  = idCounter++;
            _records.Add(entityCopy);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _records.Select(DeepCopy);
        }
    }
}


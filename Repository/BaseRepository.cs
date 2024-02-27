using DataAccessLayer.Service;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public void Delete(object id)
        => BaseService<T>.Instance.Delete(id);

        public IEnumerable<T> GetAll()
        => BaseService<T>.Instance.GetAll();

        public T GetById(object id)
        => BaseService<T>.Instance.GetById(id);

        public void Insert(T obj)
        => BaseService<T>.Instance.Insert(obj);

        public void Save()
        => BaseService<T>.Instance.Save();  

        public void Update(T obj, object id)
        => BaseService<T>.Instance.Update(obj,id);
    }
}

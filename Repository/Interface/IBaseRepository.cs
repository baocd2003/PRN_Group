using BussinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        int GetTotalCount();
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(int pageNumber, int pageSize);
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj, object id);
        void Update(User user);
        void Delete(object id);
        void Save();
    }
}

using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Service
{
    public class BaseService<T> where T : class
    {
        private static BaseService<T> instance;
        public static readonly object instanceLock = new object();
        private readonly applicationDbContext _db = new applicationDbContext();
        private DbSet<T> table = null;
        public BaseService()
        {
            table = _db.Set<T>();
        }
        public static BaseService<T> Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance is null)
                    {
                        instance = new BaseService<T>();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<T> GetAll()
        {
            try
            {
                return table.ToList();
                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public void Insert(T obj)
        {
            //It will mark the Entity state as Added State
            table.Add(obj);
        }

        public void Update(T obj, object id)
        {
            // Find the entity's primary key properties
            var entityType = _db.Model.FindEntityType(typeof(T));
            var primaryKeyProperties = entityType.FindPrimaryKey().Properties;

            // Create a dictionary to hold the primary key values
            var keyValues = new Dictionary<string, object>();
            foreach (var property in primaryKeyProperties)
            {
                keyValues[property.Name] = id; // Assuming 'id' is the primary key value
            }

            // Look for any previously tracked entity with the same primary key values
            var entry = _db.ChangeTracker.Entries<T>().FirstOrDefault(e =>
            {
                foreach (var keyValue in keyValues)
                {
                    if (!e.Property(keyValue.Key).CurrentValue.Equals(keyValue.Value))
                    {
                        return false;
                    }
                }
                return true;
            });

            // If found, detach it
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }

            // Attach the updated entity
            _db.Attach(obj);

            // Set the state of the Entity as Modified
            _db.Entry(obj).State = EntityState.Modified;
        }


        //This method is going to remove the record from the table
        //It will receive the primary key value as an argument whose information needs to be removed from the table
        public void Delete(object id)
        {
            
            T existing = table.Find(id);          
            table.Remove(existing);
        }
       
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}

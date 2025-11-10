using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;


namespace SharpSeer.Interfaces
{
    public interface IService<T>
    {     
        void Create(T t);
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Update(T t);
        void Update(T t, int id);
        void Delete(T t);
    }
}

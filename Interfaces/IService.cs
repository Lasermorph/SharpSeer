using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;


namespace SharpSeer.Interfaces
{
    public interface IService<T>
    {     
        void Create(T t);
        IEnumerable<T> GetAll();
        void Update(T t);
        void Delete(T t);
    }
}

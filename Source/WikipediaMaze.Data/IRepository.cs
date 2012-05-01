using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace WikipediaMaze.Data
{
    public interface IRepository
    {
        ISession OpenSession();
        IQueryable<TModel> All<TModel>();
        void Save<TModel>(TModel model);
        void Update<TModel>(TModel model);
        void Delete<TModel>(TModel model);
        void Delete<TModel>(object id);
        ITransaction BeginTransaction();
        void Flush();
    }
}

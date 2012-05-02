using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace WikipediaMaze.Data
{
    public interface IRepository
    {
        IQueryable<TModel> All<TModel>();
        void Save<TModel>(TModel model);
        void Update<TModel>(TModel model);
        void Delete<TModel>(TModel model);
        void Delete<TModel>(object id);
    }
}

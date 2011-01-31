using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;
using FluentNHibernate.Cfg;

namespace WikipediaMaze.Data.NHibernate
{
    public class NHibernateRepository : IRepository
    {
        #region Fields

        private ISession _session;

        [ThreadStatic]
        private ISessionFactory _sessionFactory;

        [ThreadStatic]
        private bool _IsSessionFactoryInitialized;

        #endregion

        #region Constructors

        public NHibernateRepository() { }

        #endregion

        private ISessionFactory SessionFactory
        {
            get
            {
                if (!_IsSessionFactoryInitialized)
                {
                    _sessionFactory =  Fluently.Configure()
                                      .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008.ConnectionString(db => db.Is(WikipediaMaze.Core.Settings.WikipediaMazeConnection)))
                                      .Mappings(m => m.FluentMappings.AddFromAssemblyOf<IRepository>())
                                      .BuildSessionFactory();

                    _IsSessionFactoryInitialized = true;
                }

                return _sessionFactory;
            }
        }

        #region IRepository Implementation

        public ISession OpenSession()
        {
            _session = SessionFactory.OpenSession();   
            return _session;
        }

        public IQueryable<TModel> All<TModel>()
        {
            return _session.Linq<TModel>();
        }

        public void Save<TModel>(TModel model)
        {
            _session.Save(model);
        }
        public void Update<TModel>(TModel model)
        {
            _session.Update(model);
        }
        public void Delete<TModel>(TModel model)
        {
            _session.Delete(model);
        }

        public ITransaction BeginTransaction()
        {
            return _session.BeginTransaction();
        }
        public void Flush()
        {
            _session.Flush();
        }
        #endregion

    }
}

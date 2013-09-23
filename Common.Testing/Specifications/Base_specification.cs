namespace Boilerplate.Test {
    using System;
    using System.Data.Entity;
    using Machine.Fakes;
    using Machine.Specifications;
    using Microsoft.Practices.ServiceLocation;
    using NCommon.Data;
    using NCommon.Data.EntityFramework;
    using NCommon.State;
    using NCommon.Testing;
    using Boilerplate.Caching;
    using Boilerplate.Contexts;
    using Boilerplate.Test.Data;

    public class Base_specification : WithFakes {
        protected static IServiceLocator Locator;
        protected static UnitOfWorkScope Scope;
        protected static bool IsDataContextUnique = true;
        protected static ICacheManager Cache;

        static DataContext _context;
        protected static DataContext DataContext {
            get {
                if (_context == null) {
                    _context = new DataContext("TestDatabase");
                }
                return _context;
            }
        }

        Establish context = () => {
            Database.SetInitializer<DataContext>(new CreateClearDatabaseAlways());
            //DataContext.Database.Initialize(true);

            var unitOfWorkFactory = new EFUnitOfWorkFactory();
            unitOfWorkFactory.RegisterContextProvider(() => new DataContext("TestDatabase"));

            Locator = An<IServiceLocator>();
            Locator
                .WhenToldTo(x => x.GetInstance<IUnitOfWorkFactory>())
                .Return(unitOfWorkFactory);
            Locator
                .WhenToldTo(x => x.GetInstance<IState>())
                .Return(new FakeState());
            Cache = An<ICacheManager>();
            Locator
                .WhenToldTo(x => x.GetInstance<ICacheManager>())
                .Return(Cache);
            ServiceLocator.SetLocatorProvider(() => Locator);

            Scope = new UnitOfWorkScope(TransactionMode.Supress);
        };

        Cleanup cleanup = () => {
            Scope.Dispose();
            _context = null;
        };
    }
}
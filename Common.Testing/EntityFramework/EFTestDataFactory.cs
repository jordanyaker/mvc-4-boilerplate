namespace Boilerplate.Test.EntityFramework {
    using System;
    using System.Data.Objects;
    using System.Linq;
    using Boilerplate.Contexts;

    public class EFTestDataFactory : Disposable {
        readonly DataContext _context;

        public EFTestDataFactory(DataContext context) {
            _context = context;
        }

        public DataContext Context {
            get { return _context; }
        }

        public void Batch(Action<EFTestDataActions> action) {
            var dataActions = new EFTestDataActions(this);
            action(dataActions);
            _context.SaveChanges();
        }
    }
}
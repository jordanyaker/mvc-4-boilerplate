namespace Boilerplate.Test.Data {
    using Boilerplate.Contexts;
    using System.Data.Entity;
    using System.Transactions;

    public class CreateClearDatabaseAlways : IDatabaseInitializer<DataContext> {
        public void InitializeDatabase(DataContext context) {
            bool exists = false;
            using (new TransactionScope(TransactionScopeOption.Suppress)) {
                exists = context.Database.Exists();
            }

            //if (exists) {
            //    using (new TransactionScope(TransactionScopeOption.Suppress)) {
            //        context.Database.Delete();
            //        context.SaveChanges();
            //    }
            //}

            using (new TransactionScope(TransactionScopeOption.Suppress)) {
                try {
                    context.Database.CreateIfNotExists();
                } finally {
                }

                context.SaveChanges();
            }
        }
    }
}
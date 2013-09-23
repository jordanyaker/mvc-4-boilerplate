namespace Boilerplate.Domain {
    using System;
    using System.Linq;
    using NCommon.Data.EntityFramework;
    using StackExchange.Profiling;

    public class Account {
        // ------------------------------------------------------------------------------
        // Constructors
        // ------------------------------------------------------------------------------
        public Account() {
            this.Id = DateTime.UtcNow.Ticks;
            this.DateCreated = DateTime.UtcNow;
            this.Key = Guid.NewGuid().ToString().Replace("-", "");
            this.IsActive = true;
        }

        // ------------------------------------------------------------------------------
        // Properties
        // ------------------------------------------------------------------------------
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        // ------------------------------------------------------------------------------
        // Methods
        // ------------------------------------------------------------------------------
        public static Account Create() {
#if DEBUG
            using (MiniProfiler.Current.Step("Account.Create")) { 
#endif
                var account = new Account();

                new EFRepository<Account>()
                    .Add(account);

                return account;
#if DEBUG
            } 
#endif
        }
        public static Account GetByKey(string accountKey) {
 #if DEBUG
            using (MiniProfiler.Current.Step("Account.GetByKey")) { 
#endif
           string key = (accountKey ?? "").Trim();

            return new EFRepository<Account>()
                .FirstOrDefault(x => x.Key == key);
#if DEBUG
            } 
#endif
        }
        public static Account GetById(long id) {
#if DEBUG
            using (MiniProfiler.Current.Step("Account.GetById")) {
#endif
                return new EFRepository<Account>()
                .FirstOrDefault(x => x.Id == id);
#if DEBUG
            } 
#endif
        }
    }
}

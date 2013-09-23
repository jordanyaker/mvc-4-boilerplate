namespace Boilerplate.Domain {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using NCommon.Data.EntityFramework;
    using StackExchange.Profiling;

    public class User {
        // ------------------------------------------------------------------------------
        // Constructors
        // ------------------------------------------------------------------------------
        public User() {
            this.Id = DateTime.UtcNow.Ticks;
            this.DateCreated = DateTime.UtcNow;
            this.IsActive = true;
        }

        // ------------------------------------------------------------------------------
        // Properties
        // ------------------------------------------------------------------------------
        public long Id { get; set; }
        public long AccountId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordValue { get; set; }

        internal virtual Account WithAccount { get; set; }

        // ------------------------------------------------------------------------------
        // Methods
        // ------------------------------------------------------------------------------
        public static User Create(long accountId, string email, string name, string password) {
#if DEBUG
            using (MiniProfiler.Current.Step("User.Create")) {
#endif
                var user = new User {
                    AccountId = accountId,
                    Email = (email ?? "").Trim().ToLower(),
                    Name = (name ?? "").Trim()
                };
                user.SetPassword(password);

                new EFRepository<User>()
                    .Add(user);

                return user;
#if DEBUG
            }
#endif
        }
        public static User Create(string email, string name, string password) {
#if DEBUG
            using (MiniProfiler.Current.Step("User.Create")) {
#endif
                var account = Account.Create();

                var user = new User {
                    AccountId = account.Id,
                    WithAccount = account,
                    Email = (email ?? "").Trim().ToLower(),
                    Name = (name ?? "").Trim()
                };
                user.SetPassword(password);

                new EFRepository<User>()
                    .Add(user);

                return user;
#if DEBUG
            }
#endif
        }
        public static IEnumerable<User> GetAllByAccountId(long accountId) {
#if DEBUG
            using (MiniProfiler.Current.Step("User.GetAllByAccountId")) {
#endif
                return new EFRepository<User>()
                     .Where(x => x.AccountId == accountId)
                     .ToArray();
#if DEBUG
            }
#endif
        }
        public static User GetByAccountIdAndId(long accountId, long id) {
#if DEBUG
            using (MiniProfiler.Current.Step("User.GetByAccountIdAndId")) {
#endif
                return new EFRepository<User>()
                        .FirstOrDefault(x =>
                            x.AccountId == accountId &&
                            x.Id == id);
#if DEBUG
            }
#endif
        }
        public static User GetActiveByEmail(string email) {
#if DEBUG
            using (MiniProfiler.Current.Step("User.GetByEmail")) {
#endif
                string address = (email ?? "").ToLower().Trim();

                return new EFRepository<User>()
                    .FirstOrDefault(x => 
                        x.Email == email &&
                        x.IsActive == true);
#if DEBUG
            }
#endif
        }
        public static User GetByEmail(string email) {
#if DEBUG
            using (MiniProfiler.Current.Step("User.GetByEmail")) {
#endif
                string address = (email ?? "").ToLower().Trim();

                return new EFRepository<User>()
                    .FirstOrDefault(x => x.Email == email);
#if DEBUG
            }
#endif
        }
        public static User GetById(long id) {
#if DEBUG
            using (MiniProfiler.Current.Step("User.GetById")) {
#endif
                return new EFRepository<User>()
                           .FirstOrDefault(x => x.Id == id);
#if DEBUG
            }
#endif
        }
        public bool IsPassword(string value) {
#if DEBUG
            using (MiniProfiler.Current.Step("User.IsPassword")) {
#endif
                if (string.IsNullOrEmpty(value)) {
                    return false;
                }

                return (this.PasswordSalt.GetHashed() + value.GetHashed()).GetHashed() == (this.PasswordValue);
#if DEBUG
            }
#endif
        }
        public void SetPassword(string password) {
#if DEBUG
            using (MiniProfiler.Current.Step("User.SetPassword")) {
#endif
                if (string.IsNullOrWhiteSpace(password) == false) {
                    byte[] data = new byte[0x10];
                    using (var provider = new RNGCryptoServiceProvider()) {
                        provider.GetBytes(data);
                    }

                    this.PasswordSalt = Convert.ToBase64String(data);
                    this.PasswordValue = (this.PasswordSalt.GetHashed() + password.GetHashed()).GetHashed();
                }
#if DEBUG
            }
#endif
        }
    }
}

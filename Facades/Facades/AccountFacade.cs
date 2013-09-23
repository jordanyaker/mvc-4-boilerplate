namespace Boilerplate.Facades {
    using System.Collections.Generic;
    using System.Linq;
    using NCommon.Data;
    using StackExchange.Profiling;
    using Boilerplate.Caching;
    using Boilerplate.Domain;
    using Boilerplate.Validation;

    public interface IAccountFacade {
        FacadeResult CancelAccount(long id);
        Account GetAccountById(long id);
        Account GetAccountByKey(string key);
        User GetUserById(long userId);
        User GetUserByEmail(string email);
        FacadeResult<User> SignUp(string name, string email, string password);
        FacadeResult<User> SignIn(string email, string password);
        FacadeResult<User> UpdateUserPassword(long userId, string password);
        FacadeResult<User> UpdateUserProfile(long userId, string name, string email);
    }

    public class AccountFacade : IAccountFacade {
        // -------------------------------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------------------------------
        public FacadeResult CancelAccount(long id) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountFacade.CancelAccount")) {
#endif
                using (var context = new UnitOfWorkScope(TransactionMode.New)) {
                    var account = Account.GetById(id);

                    var validationResults = new AccountValidator()
                        .Validate(account);
                    if (validationResults.IsValid == true) {
                        account.IsActive = false;

                        User.GetAllByAccountId(id).ForEach(user => {
                            user.IsActive = false;

                            UserCacheManager.Put(user);
                        });

                        context.Commit();

                        AccountCacheManager.Put(account);

                        return new FacadeResult();
                    }

                    var error = validationResults.Errors
                        .First().ErrorMessage
                        .GetError();

                    return new FacadeResult(error);
#if DEBUG
                }
#endif
            }
        }
        public Account GetAccountById(long id) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountFacade.GetAccountById")) {
#endif
                var account = AccountCacheManager.Get(id);

                if (account == null) {
                    using (var context = new UnitOfWorkScope(TransactionMode.Supress)) {
                        account = Account.GetById(id);

                        if (account != null) {
                            AccountCacheManager.Put(account);
                        }
                    }
                }

                return account;
#if DEBUG
            }
#endif
        }
        public Account GetAccountByKey(string key) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountFacade.GetAccountByKey")) {
#endif
                var account = AccountCacheManager.Get(key);

                if (account == null) {
                    using (var context = new UnitOfWorkScope(TransactionMode.Supress)) {
                        account = Account.GetByKey(key);

                        if (account != null) {
                            AccountCacheManager.Put(account);
                        }
                    }
                }

                return account;
#if DEBUG
            }
#endif
        }
        public User GetUserByEmail(string email) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountFacade.GetUserByEmail")) {
#endif
                using (var context = new UnitOfWorkScope(TransactionMode.Supress)) {
                    return User.GetActiveByEmail(email);
                }
#if DEBUG
            }
#endif
        }
        public User GetUserById(long userId) {
#if DEBUG
            using (MiniProfiler.Current.Step("UserFacade.GetUserById")) {
#endif
                var user = UserCacheManager.Get(userId);

                if (user == null) {
                    using (var context = new UnitOfWorkScope(TransactionMode.Supress)) {
                        user = User.GetById(userId);

                        if (user != null) {
                            UserCacheManager.Put(user);
                        }
                    }
                }

                return user;
#if DEBUG
            }
#endif
        }
        public FacadeResult<User> SignIn(string email, string password) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountFacade.SignIn")) {
#endif
                using (var context = new UnitOfWorkScope(TransactionMode.Supress)) {
                    var user = User.GetActiveByEmail(email);

                    if (user != null && user.IsPassword(password)) {
                        UserCacheManager.Put(user);

                        return new FacadeResult<User>(user);
                    }

                    var error = new FacadeError(20010, "WithPassword", "The supplied email and/or password are invalid.");
                    return new FacadeResult<User>(error);
                }
#if DEBUG
            }
#endif
        }
        public FacadeResult<User> SignUp(string name, string email, string password) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountFacade.SignUp")) {
#endif
                using (var context = new UnitOfWorkScope(TransactionMode.New)) {
                    var user = User.Create(email, name, password);

                    var userValidation = new UserValidator()
                        .Validate(user);
                    if (userValidation.IsValid == false) {
                        var error = userValidation.Errors
                            .First().ErrorMessage
                            .GetError();

                        return new FacadeResult<User>(error);
                    }

                    context.Commit();

                    UserCacheManager.Put(user);

                    return new FacadeResult<User>(user);
                }
#if DEBUG
            }
#endif
        }
        public FacadeResult<User> UpdateUserPassword(long userId, string password) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountFacade.UpdateUserPassword")) {
#endif
                using (var context = new UnitOfWorkScope()) {
                    var user = User.GetById(userId);

                    if (user != null) {
                        user.SetPassword(password);
                    }

                    var validationResults = new UserValidator()
                        .Validate(user);
                    if (validationResults.IsValid) {
                        context.Commit();

                        UserCacheManager.Put(user);

                        return new FacadeResult<User>(user);
                    }

                    var error = validationResults.Errors
                        .First().ErrorMessage
                        .GetError();

                    return new FacadeResult<User>(error);
                }
#if DEBUG
            }
#endif
        }
        public FacadeResult<User> UpdateUserProfile(long userId, string name, string email) {
#if DEBUG
            using (MiniProfiler.Current.Step("AccountFacade.UpdateUserProfile")) {
#endif
                using (var context = new UnitOfWorkScope()) {
                    var user = User.GetById(userId);

                    if (user != null) {
                        user.Name = (name ?? "").Trim();
                        user.Email = (email ?? "").Trim().ToLower();
                    }

                    var validationResults = new UserValidator()
                        .Validate(user);
                    if (validationResults.IsValid) {
                        context.Commit();

                        UserCacheManager.Put(user);

                        return new FacadeResult<User>(user);
                    }

                    var error = validationResults.Errors
                        .First().ErrorMessage
                        .GetError();

                    return new FacadeResult<User>(error);
                }
            }
#if DEBUG
        }
#endif
    }
}

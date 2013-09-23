namespace Boilerplate.Web.Services {
    using System.Diagnostics;
    using System.Web.Security;

    public interface IAuthenticationService {
        void SignIn(string userName, bool createPersistentCookie = false);
        void SignOut();
    }

    public class FormsAuthenticationService : IAuthenticationService {
        public void SignIn(string userName, bool createPersistentCookie = false) {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
        public void SignOut() {
            FormsAuthentication.SignOut();
        }
    }
}
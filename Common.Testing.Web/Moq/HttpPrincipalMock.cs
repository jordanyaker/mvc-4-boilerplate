namespace Moq.Mvc {
    using System.Security.Principal;

    public class HttpPrincipalMock : Mock<IPrincipal> {
        public HttpPrincipalMock() {
            Identity = new HttpIdentityMock();
            SetupGet(u => u.Identity).Returns(Identity.Object);
        }

        public HttpIdentityMock Identity {
            get;
            private set;
        }

        public new void Verify() {
            Identity.Verify();
            base.Verify();
        }

        public new void VerifyAll() {
            Identity.VerifyAll();
            base.VerifyAll();
        }
    }
}
﻿//Copyright (c) 2007, Moq Contributors
//http://code.google.com/p/moq-contrib/
//All rights reserved.

//Redistribution and use in source and binary forms, 
//with or without modification, are permitted provided 
//that the following conditions are met:

//    * Redistributions of source code must retain the 
//    above copyright notice, this list of conditions and 
//    the following disclaimer.

//    * Redistributions in binary form must reproduce 
//    the above copyright notice, this list of conditions 
//    and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution.

//    * Neither the name of the Moq Contributors nor the 
//    names of its contributors may be used to endorse 
//    or promote products derived from this software 
//    without specific prior written permission.

//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
//CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
//MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
//BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
//INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
//OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
//SUCH DAMAGE.

//[This is the BSD license, see
// http://www.opensource.org/licenses/bsd-license.php]

using System.Web;

namespace Moq.Mvc {
    /// <summary>
    /// Complete object model for mocking the MVC Http context
    /// </summary>
    public class HttpContextMock : Mock<HttpContextBase> {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public HttpContextMock() {
            this.HttpApplicationState = new HttpApplicationStateMock();
            this.HttpRequest = new HttpRequestMock();
            this.HttpResponse = new HttpResponseMock();
            this.HttpServerUtility = new HttpServerUtilityMock();
            this.HttpSessionState = new HttpSessionStateMock();
            this.HttpUser = new HttpPrincipalMock();

            this.SetupGet(c => c.Application).Returns(this.HttpApplicationState.Object);
            this.SetupGet(c => c.Request).Returns(this.HttpRequest.Object);
            this.SetupGet(c => c.Response).Returns(this.HttpResponse.Object);
            this.SetupGet(c => c.Server).Returns(this.HttpServerUtility.Object);
            this.SetupGet(c => c.Session).Returns(this.HttpSessionState.Object);
            this.SetupGet(c => c.User).Returns(this.HttpUser.Object);
        }

        /// <summary>
        /// 
        /// </summary>
        public HttpApplicationStateMock HttpApplicationState { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpRequestMock HttpRequest { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpResponseMock HttpResponse { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpServerUtilityMock HttpServerUtility { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpSessionStateMock HttpSessionState { get; private set; }

        public HttpPrincipalMock HttpUser { get; private set; }

        /// <summary>
        /// Verify only the mock expectations marked as Verifiable
        /// </summary>
        public void VerifyContext() {
            this.HttpApplicationState.Verify();
            this.HttpRequest.Verify();
            this.HttpResponse.Verify();
            this.HttpServerUtility.Verify();
            this.HttpSessionState.Verify();
            this.HttpUser.Verify();
        }

        /// <summary>
        /// Very all the mock expectations
        /// </summary>
        public void VerifyContextsAll() {
            this.HttpApplicationState.VerifyAll();
            this.HttpRequest.VerifyAll();
            this.HttpResponse.VerifyAll();
            this.HttpServerUtility.VerifyAll();
            this.HttpSessionState.VerifyAll();
            this.HttpUser.VerifyAll();
        }
    }
}
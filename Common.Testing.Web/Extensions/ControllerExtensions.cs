using System.Web.Mvc;
using System.Web.Routing;
using Moq.Mvc;
using Moq;

public static class ControllerExtensions {
    // -------------------------------------------------------------------------------------
    // Constants
    // -------------------------------------------------------------------------------------
    const string AppPathModifier = "";

    // -------------------------------------------------------------------------------------
    // Methods
    // -------------------------------------------------------------------------------------
    public static HttpContextMock MockHttpContext(this Controller controller) {
        return MockHttpContext(controller, null, null, null);
    }
    public static HttpContextMock MockHttpContext(this Controller controller, string appPath, string requestPath, string httpMethod) {
        HttpContextMock httpContext = GetHttpContext(appPath, requestPath, httpMethod);

        controller.ControllerContext = new ControllerContext(httpContext.Object, new RouteData(), controller);
        controller.Url = new UrlHelper(controller.ControllerContext.RequestContext);

        return httpContext;
    }
    static HttpContextMock GetHttpContext() {
        return GetHttpContext(null, null, null);
    }
    static HttpContextMock GetHttpContext(string appPath, string requestPath, string httpMethod) {
        HttpContextMock httpContext = new HttpContextMock();

        if (!string.IsNullOrEmpty(appPath)) {
            httpContext.HttpRequest.SetupGet(r => r.ApplicationPath).Returns(appPath);
        } else {
            httpContext.HttpRequest.SetupGet(r => r.ApplicationPath).Returns("http://test.com");
        }

        if (!string.IsNullOrEmpty(requestPath)) {
            httpContext.HttpRequest.SetupGet(r => r.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
        }

        httpContext.HttpRequest.SetupGet(r => r.PathInfo).Returns(string.Empty);

        if (!string.IsNullOrEmpty(httpMethod)) {
            httpContext.HttpRequest.SetupGet(r => r.HttpMethod).Returns(httpMethod);
        }

        httpContext.HttpResponse.Setup(r => r.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => r.Contains(AppPathModifier) ? r : AppPathModifier + r);

        return httpContext;
    }
}

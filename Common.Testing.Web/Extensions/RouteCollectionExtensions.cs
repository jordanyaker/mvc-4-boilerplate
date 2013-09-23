using System.Web.Routing;
using Moq;
using Moq.Mvc;

public static class RouteCollectionExtensions {
    public static RouteData GetRouteDataFor(this RouteCollection collection, string url, bool isPost = false) {
        var context = new HttpContextMock();
        context.HttpRequest
            .SetupGet(r => r.ApplicationPath)
            .Returns("/");
        context.HttpRequest
            .SetupGet(r => r.AppRelativeCurrentExecutionFilePath)
            .Returns(url);
        context.HttpRequest
            .SetupGet(r => r.PathInfo)
            .Returns(string.Empty);

        if (isPost) {
            context.HttpRequest
                .SetupGet(r => r.HttpMethod)
                .Returns("POST");
        }

        context.HttpResponse
            .Setup(r => r.ApplyAppPathModifier(It.IsAny<string>()))
            .Returns<string>(r => {
                return r;
            });


        return collection.GetRouteData(context.Object);
    }
}

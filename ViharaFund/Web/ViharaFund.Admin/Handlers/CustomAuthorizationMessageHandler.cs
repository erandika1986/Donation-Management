using Microsoft.AspNetCore.Components;
using System.Net;

namespace ViharaFund.Admin.Handlers
{
    public class CustomAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly NavigationManager _navigation;

        public CustomAuthorizationMessageHandler(NavigationManager navigation)
        {
            _navigation = navigation;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Optional: save current URL before redirecting
                var returnUrl = _navigation.ToBaseRelativePath(_navigation.Uri);
                _navigation.NavigateTo($"login?returnUrl={returnUrl}", forceLoad: true);
            }

            return response;
        }
    }
}

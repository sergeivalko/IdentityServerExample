using System.Collections.Generic;
using IdentityServer4.Models;

namespace Auth.Infrastructure.Options
{
    public class IdentityOptions
    {
        public string LoginUrl { get; set; }
        public string IssuerUri { get; set; }
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<ApiResource> ApiResources { get; set; }
        public IEnumerable<ApiScope> ApiScopes { get; set; }
        public IEnumerable<IdentityResource> IdentityResources { get; set; }
    }
}
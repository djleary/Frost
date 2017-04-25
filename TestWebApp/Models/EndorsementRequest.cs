using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace Frost.Models
{
    public enum EndorsementRequestStatus
    {
        New,
        Approved,
        Rejected
    }

    public class EndorsementRequest
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Please enter a contact name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage="Please enter your organization's name.")]
        public string Organization { get; set; }

        public string LogoUri { get; set; }

        [Required(ErrorMessage="Please enter a uri for your application's OAuth2 entry point")]
        [Display(Name="Login Uri")]
        public string LoginUri { get; set; }

        [Required(ErrorMessage = "Please enter a uri for your application's OAuth2 JWT processing")]
        public List<string> RedirectUris { get; set; }

        [Required(ErrorMessage="Please enter a friendly name for your application")]
        public string Application { get; set; }

        [Display(Name ="Application URL")]
        [Required(ErrorMessage="Please enter a valid url for your application")]
        public string ApplicationUrl { get; set; }

        public EndorsementRequestStatus Status { get; set; }

        //public List<string> GrantTypes { get; set; }

        public JwtPayload ToPayload()
        {
            var payload = new JwtPayload();

            int issuedDate = (int)(DateTime.UtcNow
               .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
               .TotalMilliseconds);

            var expireDate = (int)(DateTime.UtcNow.AddYears(1).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);

            payload["software_id"] = Id;
            payload["iss"] = "http://tempuri.org/poet";
            payload["iat"] = issuedDate;
            payload["exp"] = expireDate;
            payload["client_name"] = Application;
            payload["client_uri"] = ApplicationUrl;
            payload["logo_uri"] = LogoUri;
            payload["initiate_login_uri"] = LoginUri;
            payload["redirect_uris"] = $"[{string.Join(",", RedirectUris.Select(u => $"\"{u}\""))}]";
            payload["token_endpoint_auth_method"] = "client_secret_post";
            //payload["grant_types"] = $"[{string.Join(",", GrantTypes.Select(u => $"\"{u}\""))}]";

            return payload;
        }
    }
}
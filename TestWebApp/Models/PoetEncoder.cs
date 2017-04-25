using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;

namespace Frost.Models
{
    public class PoetEncoder
    {
        public PoetEncoder(string certificateName, StoreLocation storeLocation, StoreName storeName)
        {
            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            var collection = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, certificateName, false);

            if (collection.Count == 0)
                throw new ArgumentException("Please specify a valid certificate name", nameof(certificateName));

            Certificate = collection[0];
        }

        protected X509Certificate2 Certificate { get; }

        public JwtSecurityToken Sign(EndorsementRequest request)
        {
            var header = new JwtHeader(new SigningCredentials(new X509SecurityKey(Certificate), "RS256"));
            return new JwtSecurityToken(header, request.ToPayload());
        }
    }
}
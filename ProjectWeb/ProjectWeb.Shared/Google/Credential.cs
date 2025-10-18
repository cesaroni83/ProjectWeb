using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Shared.Google
{
    public class Credential
    {
        [Key]
        public Guid UserId { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long? ExpiresInSeconds { get; set; }
        public string IdToken { get; set; }
        public DateTime IssuedUtc    { get; set; }

    }
}

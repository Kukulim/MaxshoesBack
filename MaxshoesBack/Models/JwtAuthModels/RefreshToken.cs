using System;

namespace MaxshoesBack.JwtAuth
{
    public class RefreshToken
    {
        public string UserName { get; set; }

        public string TokenString { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
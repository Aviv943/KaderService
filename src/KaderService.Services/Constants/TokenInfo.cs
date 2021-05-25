using System;

namespace KaderService.Services.Constants
{
    public class TokenInfo
    {
        public string Token { get; set; }

        public string UserId { get; set; }
        
        public DateTime Expiration { get; set; }
    }
}

using System;

namespace Core.Entities
{
    public class VerifiedUser
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public string Username { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
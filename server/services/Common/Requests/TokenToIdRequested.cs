using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests
{
    public class TokenToIdRequested
    {
        public string Token { get; set; } = string.Empty;
    }
    public class TokenToIdResponse
    {
        public string Id { get; set; } = string.Empty;
    }
}

using miniEcommerceApi.DTOs.AuthDTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniEcommerceApi.Tests.Builders
{
    public static class LoginRequestBuilder
    {
        public static LoginRequestDTO Default() => new LoginRequestDTO
        {
            Email = "test@test.com",
            Password = "testPassword1234"
        };
    }
}

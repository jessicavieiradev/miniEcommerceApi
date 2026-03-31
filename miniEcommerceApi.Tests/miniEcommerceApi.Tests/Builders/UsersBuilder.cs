using miniEcommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniEcommerceApi.Tests.Builders
{
    public static class UsersBuilder
    {
        public static Users Default() => new Users
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "testuser@test.com",
            IsActive = true
        };
    }
}

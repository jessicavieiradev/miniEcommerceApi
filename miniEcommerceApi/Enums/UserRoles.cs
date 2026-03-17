namespace miniEcommerceApi.Enums
{
    public class UserRoles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static readonly List<string> All = new() { Admin, Customer };
    }
}

using Domain.Abstraction;

namespace Domain.User
{
    public class UserErrors
    {
        public static Error NotFound = new("User.NotFound", "User not found");


    }
}

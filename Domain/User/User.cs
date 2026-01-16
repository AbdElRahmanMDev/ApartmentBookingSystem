using Domain.Abstraction;
using Domain.User.Events;

namespace Domain.User
{
    public sealed class User : Entity
    {
        //Make Constructor Private and expose it only through Function
        private User(Guid id,
            FirstName firstName,
            LastName lastName,
            Email email)
            : base(id)
        {

        }

        public FirstName FirstName { get; private set; }

        public LastName LastName { get; private set; }

        public Email Email { get; private set; }




        public static User Create(FirstName firstName, LastName lastName, Email email)
        {
            var user = new User(Guid.NewGuid(),
                firstName,
                lastName,
                email);

            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
            return user;


        }

    }
}

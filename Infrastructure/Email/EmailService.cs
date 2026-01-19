using Application.Abstraction.Email;

namespace Infrastructure.Email
{
    internal sealed class EmailService : IEmailService
    {
        public Task SendEmailAsync(Domain.User.Email email, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }
}

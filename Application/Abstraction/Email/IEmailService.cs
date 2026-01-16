using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(Domain.User.Email email, string subject, string body);


    }
}

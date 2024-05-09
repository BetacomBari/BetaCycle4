using BetaCycle4.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace BetaCycle4.Logic
{
    public interface IEmailService
    {
        void sendEmail(EmailModel emailModel);
    }
}

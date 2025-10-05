using ProjectWeb.Shared.Enums;

namespace ProjectWeb.API.Helper
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}

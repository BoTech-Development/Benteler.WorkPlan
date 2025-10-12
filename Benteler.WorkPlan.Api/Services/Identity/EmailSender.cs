using Benteler.WorkPlan.Api.Services.Identity;
using Benteler.WorkPlan.Api.SharedModels.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Benteler.WorkPlan.Api.Services.Identity;
public class EmailSender : IEmailSender<User>
{
   
    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        return Task.Run(()=>
        {
            string link = AppDomain.CurrentDomain +  " /html/auth/ConfirmEmail.html?UserName=" + user.UserName + "&ConfirmLink=" + HttpUtility.UrlEncode(confirmationLink);
            /*string content =
                "<!doctype html>\n<html lang=\"en\">\n  <head>\n    <meta charset=\"utf-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\n    <title>Bootstrap demo</title>\n    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH\" crossorigin=\"anonymous\">\n  </head>\n  <body>\n    <div class=\"col d-flex justify-content-center\">\n        <a class=\"d-inline-flex align-items-center mb-2 text-body-emphasis text-decoration-none\" href=\"https://www.botech.dev\" aria-label=\"BoTech.dev\">\n            <img src=\"https://docs.botech.dev/images/BoTechLogoNew.png\" width=\"230\"/>\n        </a>\n    </div>\n    <div class=\"col d-flex justify-content-center\">\n        <div class=\"card\" style=\"width: 18rem;\">\n           \n            <svg class=\"card-img-top\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\"><path d=\"M14 15C14 16.11 13.11 17 12 17C10.89 17 10 16.1 10 15C10 13.89 10.89 13 12 13C13.11 13 14 13.9 14 15M13.09 20C13.21 20.72 13.46 21.39 13.81 22H6C4.89 22 4 21.1 4 20V10C4 8.89 4.89 8 6 8H7V6C7 3.24 9.24 1 12 1S17 3.24 17 6V8H18C19.11 8 20 8.9 20 10V13.09C19.67 13.04 19.34 13 19 13C18.66 13 18.33 13.04 18 13.09V10H6V20H13.09M9 8H15V6C15 4.34 13.66 3 12 3S9 4.34 9 6V8M21.34 15.84L17.75 19.43L16.16 17.84L15 19L17.75 22L22.5 17.25L21.34 15.84Z\" /></svg>\n            <div class=\"card-body\">\n            <h5 class=\"card-title\">Are you the owner of this Email?</h5>\n            <p class=\"card-text\">\n                <p>Hi "
                + user.UserName +
                ",</p>\n                Someone has entered your Email by registering a new Account for botech.dev. Please do not click the button below if you are not sure whether you made the new Account on www.botech.dev</p>\n            <a href=\""
                + link +
                "\" class=\"btn btn-primary\">Confirm Email.</a>\n            </div>\n        </div>\n    </div>\n    <div class=\"container py-4 py-md-5 px-4 px-md-3 text-body-secondary\">\n        <div class=\"row\">\n          <div class=\"col-lg-3 mb-3\">\n            <a class=\"d-inline-flex align-items-center mb-2 text-body-emphasis text-decoration-none\" href=\"https://www.botech.dev\" aria-label=\"BoTech.dev\">\n              <img src=\"https://docs.botech.dev/images/BoTechLogoNew.png\" width=\"230\"/>\n            </a>\n            <ul class=\"list-unstyled small\">\n              <li class=\"mb-2\"><h5>Your Idea starts here.</h5></li>\n        \n            </ul>\n          </div>\n          <div class=\"col-6 col-lg-2 offset-lg-1 mb-3\">\n            <h5>Links</h5>\n            <ul class=\"list-unstyled\">\n              <li class=\"mb-2\"><a href=\"https://www.botech.dev\">Home</a></li>\n              <li class=\"mb-2\"><a href=\"https://docs.botech.dev\">Docs</a></li>\n              <li class=\"mb-2\"><a href=\"https://github.com/BoTech-Development/BoTech.DesignerForAvalonia/\">BoTech.DesignerForAvalonia</a></li>\n              <li class=\"mb-2\"><a href=\"https://github.com/BoTech-Development/ISim\">ISim</a></li>\n            </ul>\n          </div>\n          <div class=\"col-6 col-lg-2 mb-3\">\n            <h5>Community</h5>\n            <ul class=\"list-unstyled\">\n              <li class=\"mb-2\"><a href=\"https://github.com/BoTech-Development/\" target=\"_blank\" rel=\"noopener\">GitHub</a></li>\n              <li class=\"mb-2\"><a href=\"https://github.com/BoTech-Development/BoTech.DesignerForAvalonia/discussions\" target=\"_blank\" rel=\"noopener\">Discussions</a></li>\n            </ul>\n          </div>\n          <div class=\"col-6 col-lg-2 mb-3\">\n            <h5>About</h5>\n            <ul class=\"list-unstyled\">\n              <li class=\"mb-2\"><a href=\"https://aka.botech.dev/go/Privacy\" target=\"_blank\" rel=\"noopener\">Privacy</a></li>\n              <li class=\"mb-2\"><a href=\"https://aka.botech.dev/go/TermsOfUse\" target=\"_blank\" rel=\"noopener\">Terms of Use</a></li>\n              <li class=\"mb-2\"><a href=\"https://aka.botech.dev/go/License\" target=\"_blank\" rel=\"noopener\">License</a></li>\n            </ul>\n          </div>\n        \n        </div>\n      </div>\n    <script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js\" integrity=\"sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz\" crossorigin=\"anonymous\"></script>\n  </body>\n</html>";
            */
            string topContent =
                "<!doctype html>\n<html lang=\"en\" data-bs-theme=\"auto\">\n<head>\n    <meta charset=\"utf-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" />\n    <meta name=\"description\" content=\"\" />\n\n    <title>Please confirm your email | benteler-workplan.api.botech.dev</title>\n\n    <script src=\"../assets/js/color-modes.js\"></script>\n    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB\" crossorigin=\"anonymous\">\n\n    <meta name=\"theme-color\" content=\"#712cf9\" />\n\n</head>\n  <body>\n    <div class=\"container my-5\">\n      <div class=\"p-5 text-center bg-body-tertiary rounded-3\">\n        <img src=\"./Benteler.WorkPlan.Api/wwwroot/assets/logos/BoTechLogoComplete.svg\" width=\"480\"/>\n        <h1 class=\"text-body-emphasis\">Please confirm your email.</h1>\n          <h3>Hi";
            string middleContent =
                ",</h3>\n    \n          <p class=\"col-lg-8 mx-auto fs-5 text-muted\">\n              Please confirm your email address by clicking the button below. If you did not create an account, no further action is required.\n          </p>\n        <div class=\"d-inline-flex gap-2 mb-5\">\n            <a class=\"d-inline-flex align-items-center btn btn-primary btn-lg px-4 rounded-pill\"\n                    type=\"button\"\n                    href=\"";
            string bottomContent =
                "\">\n                Confirm Email\n            </a>\n        </div>\n        <div class=\"col-lg-6 mx-auto text-muted\">\n            <p>\n                By confirming your email, you agree to our <a href=\"https://aka.botech.dev/Benteler.WorkPlan/tou\">Terms of Use</a> and <a href=\"https://aka.botech.dev/Benteler.WorkPlan/pp\">Privacy Policy</a>.\n            </p>\n            <p>\n                Regards,<br />\n                BoTech\n            </p>\n        </div>\n    </div>\n  </body>\n</html>\n";
            string content = topContent + " " + user.UserName + " {" + link + "} " + middleContent + link + bottomContent;
            SendMail(user.Email, "Confirmation Link | benteler-workplan.api.botech.dev", content);
        });
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    private void SendMail(string to, string subject, string body)
    {
        try
        {
            // SMTP server configuration
            string smtpServer = "smtp.ionos.de"; // IONOS SMTP server
            int smtpPort = 587; // Port for STARTTLS
            string emailFrom = "server@botech.dev"; // Your IONOS email address
            string emailPassword = "dfsgkjlbeafwouih4384PAJKLEdsgywrt354!§$"; // Your IONOS email password

            // Create the MailMessage object
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailFrom);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true; // Set to true if the body contains HTML

            // Configure the SMTP client
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.Credentials = new NetworkCredential(emailFrom, emailPassword);
            smtpClient.EnableSsl = true; // Enable SSL for secure connection

            // Send the email
            smtpClient.Send(mail);

            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            // Handle errors
            Console.WriteLine("Error sending email: " + ex.Message);
        }
    }
}
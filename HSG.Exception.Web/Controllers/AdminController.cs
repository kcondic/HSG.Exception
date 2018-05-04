using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Security;
using HSG.Exception.DAL;
using HSG.Exception.DAL.Repositories;

namespace HSG.Exception.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        public AdminController()
        {
            _userRepository = new UserRepository(ConnectionFactory.GetConnection(ConfigurationManager.ConnectionStrings["UserConnection"].ToString()));
        }
        private readonly UserRepository _userRepository;

        [HttpPost]
        [Route("add-user")]
        public IHttpActionResult AddUser(User userToAdd)
        {
            //definirat mail sa kojeg će se slat u web.configu
            var randomPass = Membership.GeneratePassword(8, 0);
            userToAdd.Password = HashHelper.Hash(randomPass);
            var wasUserAdded = _userRepository.AddUser(userToAdd);

            if(!wasUserAdded)
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Forbidden));

            var sendToEmail = userToAdd.Email;
            var mail = new MailMessage();
            var smtpServer = new SmtpClient("smtp.outlook.com");
            mail.From = new MailAddress(ConfigurationManager.AppSettings["SenderMail"]);
            mail.To.Add(sendToEmail);

            mail.Subject = "Dodavanje u sustav HSG Exception Logging";
            mail.Body = "Administrator Vas je dodao u sustav HSG Exception Logging," +
                        $"Vaše korisničko ime je {userToAdd.UserName}, a prvotna lozinka" +
                        $"{randomPass}. Molimo Vas da zbog sigurnosti promijenite lozinku prilikom" +
                        "prve prijave na sustav.";

            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SenderMail"],
                ConfigurationManager.AppSettings["SenderMailPassword"]);
            smtpServer.EnableSsl = true;
            smtpServer.Send(mail);

            return Ok();
        }
    }
}

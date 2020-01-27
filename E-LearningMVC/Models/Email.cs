

namespace E_LearningMVC.Controllers
{
    public class Email
    {
        public Email(InfoInscription info, string user,string pwd) {
            this.To = info.Email;
            this.Subject = "THOT E-Learning Email verification";
            this.Message = "To verify your account, please use these credentials to login: \n" +
                " USERNAME: " + user + "\n PASSWORD: " + pwd;
        }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

    }
}
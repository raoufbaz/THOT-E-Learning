using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using E_LearningMVC.Models;

namespace E_LearningMVC.Controllers
{
    public class StudentController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Inscription()
        {
            niveauEducationDataClassesDataContext NE = new niveauEducationDataClassesDataContext();

            var listeniveauxEducation = (from niveau in NE.niveauEducations
                                         select niveau.nom);
            ViewBag.liste = listeniveauxEducation;

            return View();
        }
        [HttpPost]
        
        public ActionResult AddUser(string Name, string Email, string NE)
        //public ActionResult Inscription(InfoInscription info)
        {
            InfoInscription info = new InfoInscription();
            info.Nom = Name;
            info.Email = Email;
            info.Education = NE;
            var username = GenererUsername(info);
            var pwd = GenererPwd(info);

            Email email = new Email(info, username, pwd);
            bool status = sendMail(email);
            if (status)
            {
                niveauEducationDataClassesDataContext db = new niveauEducationDataClassesDataContext();
                Inscription eleve = new Inscription { nom = Name, email = Email, education = NE };
                db.Inscriptions.InsertOnSubmit(eleve);
                db.SubmitChanges();
                credential cr = new credential { username = username, password = pwd, email = Email, firsttime = 1 };
                db.credentials.InsertOnSubmit(cr);
                db.SubmitChanges();
            }
            return View("inscription");
        }
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(string username, string pwd)
        {
            niveauEducationDataClassesDataContext db = new niveauEducationDataClassesDataContext();
          

            var compteSTD = (from cmpt in db.credentials
                             where cmpt.username == username && cmpt.password == pwd
                             select cmpt);
            int countSTD = compteSTD.Count();

            var comptePROF = (from cmpt in db.profs
                              where cmpt.username == username && cmpt.password == pwd
                              select cmpt);
            int countPROF = comptePROF.Count();


            //trouver le niveau deducation de eleve
            var niveau = (from cmpt in db.Inscriptions
                          where cmpt.email == compteSTD.ToList().ElementAt(0).email 
                          select cmpt);
            if (countSTD == 1)
            {
                int first = 1;
                foreach (var elem in compteSTD)
                {
                    first = elem.firsttime;
                }

                if (first == 1)
                {

                    Session["id"] = compteSTD.ToList().ElementAt(0).Id;
                    Session["username"] = username;
                    Session["password"] = pwd;
                    Session["email"] = compteSTD.ToList().ElementAt(0).email;
                    Session["niveau"] = niveau.ToList().ElementAt(0).education;


                    return View("first");
                }
                else
                {
                    Session["id"] = compteSTD.ToList().ElementAt(0).Id;
                    Session["username"] = username;
                    Session["password"] = pwd;
                    Session["email"] = compteSTD.ToList().ElementAt(0).email;
                    Session["niveau"] = niveau.ToList().ElementAt(0).education;
                    return View("accueil");

                }

            }
            else if (countPROF == 1)
            {
                  Session["id"] = comptePROF.ToList().ElementAt(0).Id;
                Session["username"] = username;
                Session["password"] = pwd;
                return RedirectToAction("accueil", "Teacher");
            }
            else
            {
                ViewBag.msg = "Username or Password incorrect";
                return View("login");
            }

        }

        public ActionResult first()
        {
            return View();
        }

        [HttpPost]
        public ActionResult first(string username, string password)
        {
            niveauEducationDataClassesDataContext db = new niveauEducationDataClassesDataContext();
            var compte = (from cmpt in db.credentials
                             where cmpt.username == Session["username"].ToString() && cmpt.password == Session["password"].ToString()
                          select cmpt);
            foreach (var cmpt in compte)
            {
                cmpt.username = username;
                cmpt.password = password;
                cmpt.firsttime = 0;
                db.SubmitChanges();
            }
            return View("accueil");
        }

        public ActionResult accueil()
        {
            return View();
        }

        public ActionResult AllCourses()
        {
            niveauEducationDataClassesDataContext bd = new niveauEducationDataClassesDataContext();

            var listeCours = (from cours in bd.Cours
                              where cours.niveaueducation == Session["niveau"].ToString()
                              select cours);
            ViewBag.listeCours = listeCours;
            return View();
        }

        [HttpPost]
        public ActionResult Course(string titre)
        {
            niveauEducationDataClassesDataContext bd = new niveauEducationDataClassesDataContext();

            var coursActif = (from cours in bd.Cours
                              where cours.Id == Convert.ToInt32(titre)
                              select cours);
            ViewBag.cours = coursActif;
            CoursContenu c1 = new CoursContenu();
            c1.Id = coursActif.ToList().ElementAt(0).Id;   
            c1.titre = coursActif.ToList().ElementAt(0).titre;
            c1.date = coursActif.ToList().ElementAt(0).date;
            c1.image = coursActif.ToList().ElementAt(0).image;
            c1.niveaueducation = coursActif.ToList().ElementAt(0).niveaueducation;
            c1.chapitres = coursActif.ToList().ElementAt(0).chapitres;
            c1.description = coursActif.ToList().ElementAt(0).description;
            string[] chaps = c1.chapitres.Split(',');
            ViewBag.chapitres = chaps;
            ViewBag.prof = coursActif.ToList().ElementAt(0).prof.username;
            return View(c1);
        }

        public ActionResult MyCourses()
        {
            niveauEducationDataClassesDataContext bd = new niveauEducationDataClassesDataContext();

            var listeCours = (from cours in bd.EleveCours
                              where cours.ideleve == Convert.ToInt32(Session["id"])
                              select cours.Cour);
            ViewBag.listeCours = listeCours;
            return View();
        }
        [HttpPost]
        public ActionResult Course2(string coursid)
        {
            niveauEducationDataClassesDataContext bd = new niveauEducationDataClassesDataContext();
            EleveCour e1 = new EleveCour { ideleve= Convert.ToInt32(Session["id"]), idcours= Convert.ToInt32(coursid) };
            bd.EleveCours.InsertOnSubmit(e1);
            bd.SubmitChanges();
            var listeCours = (from cours in bd.EleveCours
                              where cours.ideleve == Convert.ToInt32(Session["id"])
                              select cours.Cour);
            ViewBag.listeCours = listeCours;
            return View("MyCourses");
        }
        private string GenererUsername(InfoInscription info)
        {
            string user = info.Nom.Substring(0, 3) + info.Email.Substring(0, 3) + info.Education.Substring(0, 3);
            return user;
        }

        private string GenererPwd(InfoInscription info)
        {
            Random rnd = new Random();
            string pwd = info.Nom.Substring(0, 4) + info.Email.Substring(0, 2) + info.Education.Substring(2, 3) + rnd.Next(500);
            return pwd;
        }

        private bool sendMail(Email email)
        {
            try
            {
                string MailSender = System.Configuration.ConfigurationManager.AppSettings["MailSender"].ToString();
                string MailPw = System.Configuration.ConfigurationManager.AppSettings["MailPw"].ToString();

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 100000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(MailSender, MailPw);

                MailMessage mailMessage = new MailMessage(MailSender, email.To, email.Subject, email.Message);
                // mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = System.Text.UTF8Encoding.UTF8;

                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       

       
    }
    
}
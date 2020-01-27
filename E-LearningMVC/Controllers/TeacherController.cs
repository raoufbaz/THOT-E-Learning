using E_LearningMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace E_LearningMVC.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult accueil()
        {
            return View();
        }

        public ActionResult AddCourse()
        {
            niveauEducationDataClassesDataContext NE = new niveauEducationDataClassesDataContext();

            var listeniveauxEducation = (from niveau in NE.niveauEducations
                                         select niveau.nom);
            ViewBag.liste = listeniveauxEducation;
            return View();
        }
        [HttpPost]
        public ActionResult AddCours(string Title, string Education, string Chapters, string Date, string Description,string Image)
        {
            niveauEducationDataClassesDataContext db = new niveauEducationDataClassesDataContext();
            Cour c1 = new Cour { idprof = Convert.ToInt32(Session["id"]), titre=Title, niveaueducation=Education, chapitres= Chapters,
            date= Date,description=Description,image=Image};//fetch IDPROF
            db.Cours.InsertOnSubmit(c1);
            db.SubmitChanges();
           
            return View("AddCourse");
        }

        public ActionResult MyCourses()
        {
            niveauEducationDataClassesDataContext bd = new niveauEducationDataClassesDataContext();
            
            var listeCours = (from cours in bd.Cours where cours.prof.username==Session["username"].ToString()
                                         select cours);
            ViewBag.listeCours = listeCours;
            return View();
        }
        [HttpPost]
        public ActionResult Course(string titre)
        {
            niveauEducationDataClassesDataContext bd = new niveauEducationDataClassesDataContext();
            
            var coursActif= (from cours in bd.Cours
                              where cours.Id == Convert.ToInt32(titre)
                             select cours);
            ViewBag.cours = coursActif;
            CoursContenu c1 = new CoursContenu();
            c1.Id = coursActif.ToList().ElementAt(0).Id;    //.ElementAt(0).Id;
            c1.titre = coursActif.ToList().ElementAt(0).titre;
            c1.date = coursActif.ToList().ElementAt(0).date;
           c1.image = coursActif.ToList().ElementAt(0).image;
           c1.niveaueducation = coursActif.ToList().ElementAt(0).niveaueducation;
           c1.chapitres = coursActif.ToList().ElementAt(0).chapitres;
            c1.description = coursActif.ToList().ElementAt(0).description;
            string[] chaps = c1.chapitres.Split(',');
            ViewBag.chapitres = chaps;
            return View(c1);
        }

        

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearningMVC.Models
{
    public class CoursContenu
    {


        public int Id { get; set; }
        public int idprof { get; set; }
        public string titre { get; set; }
        public string chapitres { get; set; }
        public string date{ get; set; }
        public string niveaueducation{ get; set; }
        public string description{ get; set; }
        public string image{ get; set; }

      
    }
}
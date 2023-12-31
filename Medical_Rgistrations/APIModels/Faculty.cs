﻿using System.ComponentModel.DataAnnotations;

namespace Medical_Rgistrations.ViewModels
{

    public class Faculty
    {
        public Faculty()
        {

        }

        public string? FacultyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }

        public string InstituteName { get; set; }
        public string ProfileName { get; set; }
        public string FacultyType { get; set; }
        public bool Active { get; set; }
        
        public string FacebookLink { get; set; }
      
        public string InstaLink { get; set; }
        
        public string LinkedInLink { get; set; }
       
        public string TwitterLink { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Medical_Rgistrations.ViewModels
{

    public class RegisterViewModels
    {
        public RegisterViewModels()
        {
            this.GenderList = new List<Genders>();
            this.QualificationList = new List<Qualification>();
        }

        public string? RegistrationId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Pincode is required")]
        public string Pincode { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile number is required")]
        [MinLength(10, ErrorMessage = "Invalid mobile number")]
        [MaxLength(10, ErrorMessage = "Invalid mobile number")]
        public string Mobile { get; set; }
        public string? RferenceNbr { get; set; }
        [Required(ErrorMessage = "DOB is required")]
        [DataType(DataType.Date)]
        public DateTime DOB
        {
            get
            {
                return this._dob.HasValue
                   ? this._dob.Value
                   : DateTime.Now.Date;
            }

            set { this._dob = value; }
        }
        private DateTime? _dob = null;

        [Required(ErrorMessage = "Nationality is required")]

        public string Nationality { get; set; }
        [Required(ErrorMessage = "Nationality is required")]
        [Display(Name = "Institute Name")]
        public string InstituteName { get; set; }
        [Required(ErrorMessage = "Year is required")]
        public string Year { get; set; }
        [Required(ErrorMessage = "Please read terms and conditions")]
        public bool ReadTermsCondition { get; set; }
        [Required(ErrorMessage = "Complition date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Completion of part 1 (Month & year)")]
        public DateTime CompletePart
        {
            get
            {
                return this._compPart.HasValue
                   ? this._compPart.Value
                   : DateTime.Now.Date;
            }

            set { this._compPart = value; }
        }
        private DateTime? _compPart = null;

        [Required(ErrorMessage = "RCP code is required")]
        [Display(Name ="RCP Code")]
        public string RCPCode { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string GenderId { get; set; }
        [Required(ErrorMessage = "Qualification is required")]
        public string QualificationId { get; set; }
        
        //public Genders? Genders { get; set; }
        //public Qualification? Qualification { get; set; }

        public List<Genders>? GenderList { get; set; }
        public List<Qualification>? QualificationList { get; set; }
        public IEnumerable<string>? allYears { get; set; }


    }



    public class Genders
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class Qualification
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

}

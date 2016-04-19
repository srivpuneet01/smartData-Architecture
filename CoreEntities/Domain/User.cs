
namespace Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class Users : BaseEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name contains letters only.")]
        [StringLength(50, ErrorMessage = "The First Name must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name contains letters only.")]
        [StringLength(50, ErrorMessage = "The Last Name must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Your email address is not in a valid format. Example of correct format: joe.example@example.org")]
        public string Email { get; set; }
        // public string Password { get; set; }
        public Nullable<int> UserType { get; set; }
        public string AuthFacebookId { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        // [Display(Name = "SuperAdmin")]
        public bool IsSuperAdmin { get; set; }
        //Modify Details
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //End
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        [NotMapped]
       // [Required]
        public string RoleIDs { get; set; }
        [NotMapped]

        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public virtual string Password { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public virtual string ConfirmPassword { get; set; }
        [NotMapped]
        public List<Roles> RolesList { get; set; }

        //[NotMapped]
        //public bool IsExsits { get; set; }

        [NotMapped]
        public string ActiveDisplay
        {
            get
            {
                // If the value given to the side is negative,
                // then set it to 0
                if (Active == false)
                    return "No";
                else
                    return "Yes";
            }
        }
    }
    //For Insert User
    [NotMapped]
    public partial class UserInsert : Users
    {
        [Required]
        public override string Password
        {
            get { return base.Password; }
            set { base.Password = value; }
        }
        [Required]
        public override string ConfirmPassword
        {
            get { return base.Password; }
            set { base.Password = value; }
        }
    }

}

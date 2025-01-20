using CollegeApp.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Dtos
{
    public class StudentDTO
    {
        //[ValidateNever] - This Attribute is will not validate the Id property of the model.
        [ValidateNever]
        public int Id { get; set; }
        //[Required] - Attribute has its own error message however if you want to applu your own message then do as follows
        [Required(ErrorMessage = "Student Name is required")]
        //[StringLength(100)] - Attribute will validate the length of the value as per mentioned.
        [StringLength(100)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string? Sex { get; set; }
        ////[Range(10, 20)] - Attribute will Validate the Field with set of range as per mentioned.
        //[Range(10,20)]
        //public int Age { get; set; }
        //public string Password { get; set; }
        ////[Compare(nameof(Password))] - Attribute will comapre the value of the field with mentioned filed.
        //[Compare(nameof(Password))]
        //public string Confirmpassword { get; set; }

        //[CheckDateAttribute] - Custom validation Attribute created and applied.
        [CheckDateAttribute]
        public DateTime Admissiondate { get; set; }
    }
}

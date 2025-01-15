using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Validators
{
    public class CheckDateAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //converting object type to datetime type
            var date = (DateTime?)value;

            if (date < DateTime.Now)
                return new ValidationResult("The date must be greater than or equals to current date");

            return ValidationResult.Success;
        }
    }
}

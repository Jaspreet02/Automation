using System;
using System.ComponentModel.DataAnnotations;

namespace MobileService
{
    public class ConfirmEmailBindingModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string UserId { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [ComparePassword(nameof(ChangePasswordBindingModel.OldPassword))]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ComparePassword : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} should not be the same as {1}.";

        public string propertyToCompare { get; private set; }
        private string propertyDisplayName = string.Empty;

        public ComparePassword(string property)
          : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentNullException("otherProperty");
            }

            propertyToCompare = property;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, propertyDisplayName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType()
                                   .GetProperty(propertyToCompare);
                var displayAttribute = (otherProperty.GetCustomAttributes(typeof(DisplayAttribute), false));
                if (displayAttribute.Length > 0)
                {
                    propertyDisplayName = (((DisplayAttribute[])displayAttribute)[0]).Name;
                }
                //propertyDisplayName =  otherProperty.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                var otherPropertyValue = otherProperty
                                              .GetValue(validationContext.ObjectInstance, null);
                if (value.Equals(otherPropertyValue))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }
    }
}
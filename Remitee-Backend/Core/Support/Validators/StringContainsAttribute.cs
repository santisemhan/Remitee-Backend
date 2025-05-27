using System.ComponentModel.DataAnnotations;

namespace Remitee_Backend.Core.Support.Validators
{
    public class StringContainsAttribute : ValidationAttribute
    {
        public string[] AllowableValues { get; set; }

        public bool IsCaseSensitive { get; set; } = true;

        public bool Nulleable { get; set; } = false;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                if (value is null && Nulleable)
                {
                    return ValidationResult.Success;
                }

                if (value is null || value.GetType() != typeof(string))
                {
                    throw new ValidationException($"{validationContext.MemberName} is not a string");
                }

                if (!AllowableValues.Contains(value.ToString(), IsCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase))
                {
                    throw new ValidationException($"The value of {validationContext.MemberName} is not allowed.");
                }

                return ValidationResult.Success;
            }
            catch (ValidationException e)
            {
                return new ValidationResult(e.Message);
            }
        }
    }
}

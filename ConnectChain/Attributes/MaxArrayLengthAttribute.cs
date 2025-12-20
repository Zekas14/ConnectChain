using System.ComponentModel.DataAnnotations;

namespace ConnectChain.Attributes
{
    public class MaxArrayLengthAttribute : ValidationAttribute
    {
        private readonly int _maxLength;

        public MaxArrayLengthAttribute(int maxLength)
        {
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Array array && array.Length > _maxLength)
            {
                return new ValidationResult($"Array cannot have more than {_maxLength} elements.");
            }

            return ValidationResult.Success;
        }
    }
}

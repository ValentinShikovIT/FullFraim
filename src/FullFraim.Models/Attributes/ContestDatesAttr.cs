using System;
using System.ComponentModel.DataAnnotations;

namespace FullFraim.Models.Attributes
{
    public class ContestDatesAttr : ValidationAttribute
    {
        public string BiggerThanDependantPropName { get; set; }
        public string DependantPropDisplayName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phase = (DateTime)value;

            var containerType = validationContext.ObjectInstance.GetType();

            if (phase < DateTime.UtcNow)
            {
                return new ValidationResult(ErrorMessage = "Date cannot be in the past");
            }

            if (BiggerThanDependantPropName != null)
            {
                var field = containerType.GetProperty(BiggerThanDependantPropName);
                if (field != null)
                {
                    var phaseII = (DateTime)field.GetValue(validationContext.ObjectInstance, null);
                    if (phaseII > phase)
                    {
                        if (DependantPropDisplayName == null)
                        {
                            throw new Exception();
                        }
                        return new ValidationResult(ErrorMessage = $"Date cannot be before {DependantPropDisplayName}");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}

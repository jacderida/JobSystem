using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.BusinessLogic.Validation.Core
{
    public interface IValidator<T>
    {
        List<ValidationResult> Validate(T obj);
    }
}
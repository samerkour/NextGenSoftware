namespace BuildingBlocks.Validation;

public class ValidationException : Exception
{
    public ValidationException(ValidationResultModel validationResult)
    {
        ValidationResult = validationResult;
    }

    public ValidationResultModel ValidationResult { get; }
}

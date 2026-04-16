using Cortex.Mediator.Queries;
using FluentValidation;
using FluentValidation.Results;
using HngClassify.Application.Features.Genderize.Responses;

namespace HngClassify.Application.Abstractions.Services;

public interface IGenderizeClient
{
    Task<GenderizeResponse?> GetGenderByNameAsync(string name, CancellationToken cancellationToken = default);
}



///// <summary>
///// Configures a pipeline behavior for validation of request queries using <see cref="FluentValidation"/>'s <see cref="IValidator"/>. Where <typeparamref name="TQuery" />
///// is constrained to <see cref="IQuery{TResult}"/> as for this implementation we want to only be running validation behavior for our request Queries pipeline.
///// </summary>
///// <param name="validators">FluentValidator instances for the <typeparamref name="TQuery"/> (<see cref="IQuery{TResult}"/>)</param>
///// <typeparam name="TQuery">The type of query to be handled.</typeparam>
///// <typeparam name="TResponse">The type of response returned by the query handler.</typeparam>
//public class QueryValidationBehavior<TQuery, TResponse>(
//    IEnumerable<IValidator<TQuery>> validators)
//    : IQueryPipelineBehavior<TQuery, TResponse>
//    where TQuery : IQuery<TResponse>
//{
//    public async Task<TResponse> Handle(TQuery query, QueryHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    {
//        var validationFailures = await ValidateAsync(query);

//        // If there are no validation failures, proceed with the next handler.
//        if (validationFailures.Length == default)
//            return await next();

//        if (typeof(TResponse).IsGenericType &&
//            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
//        {
//            var resultType = typeof(TResponse).GetGenericArguments()[0];

//            var failureMethod = typeof(Result<>)
//                .MakeGenericType(resultType)
//                .GetMethods().FirstOrDefault(m => m.Name == nameof(Result<>.ValidationFailure));

//            if (failureMethod is not null)
//            {
//                return (TResponse)failureMethod.Invoke(null, [CreateValidationError(validationFailures)])!;
//            }

//        }

//        if (typeof(TResponse) == typeof(Result<>))
//        {
//            return (TResponse)(object)Result<>.Failure(CreateValidationError(validationFailures));
//        }

//        throw new Exceptions.ValidationException(CreateValidationError(validationFailures));
//    }

//    private async Task<ValidationFailure[]> ValidateAsync(TQuery query)
//    {
//        //if no validator for the incoming query, then just return empty validation failure list
//        if (!validators.Any())
//            return [];

//        //Create a validation context for this request (query instance)
//        var context = new ValidationContext<TQuery>(query);

//        //Execute validators by calling the Validate method over each instance of Validators in the request
//        //Select all error if any and project them into a ValidationError ValueType
//        var validationResults = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context)));

//        var validationFailures = validationResults
//            .Where(result => !result.IsValid)
//            .SelectMany(result => result.Errors)
//            .ToArray();

//        return validationFailures;
//    }

//    /// <summary>
//    /// Creates a new ValidationError instance containing the specified validation failures.
//    /// </summary>
//    /// <param name="validationFailures">An array of ValidationFailure objects representing individual validation errors to include. Cannot be null.</param>
//    /// <returns>A ValidationError object that aggregates the provided validation failures.</returns>
//    private static Fault CreateValidationError(ValidationFailure[] validationFailures) => validationFailures.Select(f => new Fault(f.ErrorCode, f.ErrorMessage)).First();
//}

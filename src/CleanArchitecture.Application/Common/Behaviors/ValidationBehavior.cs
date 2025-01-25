using FluentValidation;
using MediatR;

namespace CleanArchitecture.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
		=> _validators = validators;

	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		var context = new ValidationContext<TRequest>(request);
		var failures = _validators
			.Select(v => v.Validate(context))
			.SelectMany(result => result.Errors)
			.Where(f => f != null)
			.ToList();

		if (failures.Any())
			throw new ValidationException(failures); // Sử dụng exception từ folder Exceptions

		return await next();
	}
}
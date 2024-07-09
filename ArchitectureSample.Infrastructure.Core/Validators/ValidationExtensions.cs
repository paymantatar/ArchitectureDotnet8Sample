using FluentValidation;

namespace ArchitectureSample.Infrastructure.Core.Validators;

public static class ValidationExtensions
{
	public static IRuleBuilderOptions<T, TProperty> EmailValidator<T, TProperty>(
	    this IRuleBuilder<T, TProperty> ruleBuilder) =>
	    ruleBuilder.SetValidator(new EmailValidator<T, TProperty>());

	public static IRuleBuilderOptions<T, TProperty> PhoneNumberValidator<T, TProperty>(
		this IRuleBuilder<T, TProperty> ruleBuilder) =>
		ruleBuilder.SetValidator(new PhoneNumberValidator<T, TProperty>());

	public static IRuleBuilderOptions<T, TProperty> GuidValidator<T, TProperty>(
		this IRuleBuilder<T, TProperty> ruleBuilder) =>
		ruleBuilder.SetValidator(new GuidValidator<T, TProperty>());
}
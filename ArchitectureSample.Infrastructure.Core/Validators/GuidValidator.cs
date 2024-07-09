using FluentValidation;
using FluentValidation.Validators;

namespace ArchitectureSample.Infrastructure.Core.Validators;

public class GuidValidator<T, TProperty> : PropertyValidator<T, TProperty>
{
	public override bool IsValid(ValidationContext<T> context, TProperty value)
	{
		if (value == null)
			return false;

		var guid = value.ToString();

		try
		{
			return Guid.TryParse(guid, out _);
		}
		catch
		{
			return false;
		}
	}

	public override string Name => nameof(GuidValidator<T, TProperty>);

	protected override string GetDefaultMessageTemplate(string errorCode) =>
		"{PropertyName} must be valid GUID.";
}
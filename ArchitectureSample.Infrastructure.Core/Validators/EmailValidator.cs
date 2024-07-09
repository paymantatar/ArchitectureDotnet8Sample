using FluentValidation;
using FluentValidation.Validators;

namespace ArchitectureSample.Infrastructure.Core.Validators;

public class EmailValidator<T, TProperty> : PropertyValidator<T, TProperty>
{
	public override bool IsValid(ValidationContext<T> context, TProperty value)
	{
		if (value == null)
			return false;

		var email = value.ToString();

		try
		{
			var address = new System.Net.Mail.MailAddress(email!);
			return address.Address == email;
		}
		catch
		{
			return false;
		}
	}

	public override string Name => nameof(EmailValidator<T, TProperty>);

	protected override string GetDefaultMessageTemplate(string errorCode) =>
	    "{PropertyName} must be valid email address.";
}
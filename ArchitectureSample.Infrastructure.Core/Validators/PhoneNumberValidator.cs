using FluentValidation;
using FluentValidation.Validators;
using PhoneNumbers;

namespace ArchitectureSample.Infrastructure.Core.Validators;

public class PhoneNumberValidator<T, TProperty> : PropertyValidator<T, TProperty>
{
	public override bool IsValid(ValidationContext<T> context, TProperty value)
	{
		if (value == null)
			return false;

		var phone = value.ToString();

		try
		{
			var phoneNumberUtil = PhoneNumberUtil.GetInstance();
			//var e164PhoneNumber = "+44 117 496 0123";
			//var nationalPhoneNumber = "2024561111";
			//var smsShortNumber = "83835";
			var phoneNumber = phoneNumberUtil.Parse(phone, null);
			return phoneNumberUtil.IsValidNumber(phoneNumber);
		}
		catch
		{
			return false;
		}
	}

	public override string Name => nameof(EmailValidator<T, TProperty>);

	protected override string GetDefaultMessageTemplate(string errorCode) =>
		"{PropertyName} must be valid phone number.";
}
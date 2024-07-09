namespace ArchitectureSample.Tests.Integration.Helpers;

internal static class Constants
{
	internal static Guid SampleId = Guid.Parse("e9abba80-f92b-49ec-871a-ee5c7a30e4b0");

	internal const string ValidSampleFirstName = "Payman";

	internal const string ValidSampleLastName = "Tatar";

	internal const string SampleUpdatedLastName = "***UPDATED***";

	internal static DateTime ValidSampleBirthOfDate = DateTime.Now.AddYears(-40);

	internal const string ValidSamplePhoneNumber = "+31647675034";

	internal const string ValidSampleEmail = "Paymantatar@gmail.com";

	internal const string ValidSampleBankAccount = "1234567890";




	internal const string InvalidSamplePhoneNumber = "123355234";

	internal const string InvalidSampleEmail = "Tatar.com";

	internal static DateTime InvalidSampleBirthOfDate = DateTime.Now.AddYears(-130);
}
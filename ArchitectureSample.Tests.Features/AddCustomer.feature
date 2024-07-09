Feature: AddCustomer
	In order to add customer
	As a user
	I want to be able add customer

@Customers
Scenario Outline: Add new customer
	Given I have entered the customer details
		| FirstName   | LastName   | BirthOfDate   | PhoneNumber   | Email   | BankAccount   |
		| <FirstName> | <LastName> | <BirthOfDate> | <PhoneNumber> | <Email> | <BankAccount> |
	When I save the customer
	Then the customer details should be <Expectation>

Examples:
	| FirstName | LastName  | BirthOfDate | PhoneNumber   | Email                        | BankAccount | Expectation |
	| Payman    | Tatar | 1993/08/03  | +31647675034 | Paymantatar@gmail.com | 1234567890  | Saved       |
	| Payman    | Tatar | 1993/08/03  | +3196860     | Paymantatar@gmail.com | 1234567890  | Not Saved   |
	| Payman    | Tatar | 1993/08/03  | +31647675034 | Payman.Tatar.com         | 1234567890  | Not Saved   |
	| Payman    | Tatar | 1893/08/03  | +31647675034 | Paymantatar@gmail.com | 1234567890  | Not Saved   |
	| Payman    | Tatar | 1993/08/03  | +31647675034 | Paymantatar@gmail.com | 127890      | Not Saved   |

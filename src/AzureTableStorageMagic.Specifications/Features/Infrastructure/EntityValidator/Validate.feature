Feature: EntityValidate.Validate

Background: 
	Given entity is valid

Scenario: when entity is valid
	When Validate(entity) is called
	Then an exception should not be thrown

Scenario Outline: when a property is invalid
	Given FirstName is <firstName>
	When Validate(entity) is called
	Then ValidationException should be thrown for FirstName

	Examples:
	| firstName  |
	| null       |
	| empty      |
	| whitespace |

Scenario Outline: when many properties are invalid
	Given FirstName is <firstName>
	Given LastName is <lastName>
	When Validate(entity) is called
	Then ValidationException should be thrown for 'FirstName, LastName'

	Examples:
	| firstName  | lastName   |
	| null       | null       |
	| empty      | null       |
	| whitespace | null       |
	| null       | empty      |
	| empty      | empty      |
	| whitespace | empty      |
	| null       | whitespace |
	| empty      | whitespace |
	| whitespace | whitespace |

Scenario: when PartitionKey is invalid
	Given PartitionKey is invalid
	When Validate(entity) is called
	Then ValidationException should be thrown for PartitionKey

Scenario: when RowKey is invalid
	Given RowKey is invalid
	When Validate(entity) is called
	Then ValidationException should be thrown for RowKey
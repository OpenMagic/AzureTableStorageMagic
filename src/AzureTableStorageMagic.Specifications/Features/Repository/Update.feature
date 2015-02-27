Feature: Repository.Update

Background: 
	Given Windows Azure Storage Emulator is running
	Given table exists
	Given entity exists
  
Scenario: when entity is valid
	Given entity is valid
	And entity has changed
	When Update(entity) is called
	Then entity should be added to the table

Scenario: when entity has not changed
	Given entity is valid
	When Update(entity) is called
	Then entity should be added to the table

Scenario: when entity does not exist
	Given entity does not exist
	And entity has changed
	When Update(entity) is called
	Then StorageException with 'The remote server returned an error: (404) Not Found.' message should be thrown

Scenario: when entity is not valid
	Given entity is not valid
	When Update(entity) is called
	Then ValidationException should be thrown for entity
	And the table should not be updated

Scenario: when entity is null
	Given entity is null
	When Update(entity) is called
	Then ArgumentNullException should be thrown for entity
	And the table should not be updated

Scenario: when Azure service is not available
	Given Windows Azure Storage Emulator is not running
	And entity is valid
	When Update(entity) is called
	Then StorageException with 'Unable to connect to the remote server' message should be thrown
	And the table should not be updated

Scenario: when table does not exist
	Given the table does not exist
	And entity is valid
	And entity has changed
	When Update(entity) is called
	Then StorageException with 'The remote server returned an error: (404) Not Found.' message should be thrown

Scenario: when HTTP status code is failure
	Given entity is valid
	And entity has changed
	And Update(entity) result.HttpStatusCode is 400 or above
	When Update(entity) is called
	Then WebException with 'Replace operation failed with HttpStatusCode 'NoContent'. Surprised CloudTable didn't throw the error itself.' message should be thrown

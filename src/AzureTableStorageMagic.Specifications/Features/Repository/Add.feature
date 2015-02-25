Feature: Repository.Add

Background: 
	Given Windows Azure Storage Emulator is running
	Given table exists
  
Scenario: when entity is valid
	Given entity is valid
	When Add(entity) is called
	Then entity should be added to the table

Scenario: when entity is not valid
	Given entity is null
	When Add(entity) is called
	Then ArgumentException should be thrown for entity

Scenario: when entity is null
	Given entity is null
	When Add(entity) is called
	Then ArgumentNullException should be thrown for entityScenario: when Azure service cannot be reached

Scenario: when Azure service is not available
	Given Windows Azure Storage Emulator is not running
	And entity is valid
	When Add(entity) is called
	Then IFailedHandler.Add(connectionString, tableName, entity) should be called
	And the entity should be added to the table

Scenario: when table does not exist
	Given the table does not exist
	And entity is valid
	When Add(entity) is called
	Then IFailedHandler.Add(connectionString, tableName, entity) should be called
	And the entity should be added to the table
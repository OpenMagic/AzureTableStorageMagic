Feature: Repository.Add

Background: 
	Given Windows Azure Storage Emulator is running
	Given table exists
  
Scenario: when entity is null
	Given entity is null
	When Add(entity) is called
	Then ArgumentNullException should be thrown for entity

Scenario: when entity is valid
	Given entity is valid
	When Add(entity) is called
	Then entity should be added to the table

Scenario: when PartitionKey is null
	Given entity.PartitionKey is null
	When Add(entity) is called
	Then IFailedHandler.Add(connectionString, tableName, entity) should be called
	And entity should not added to the table

Scenario: when PartitionKey is empty
	Given entity.PartitionKey is empty
	When Add(entity) is called
	Then IFailedHandler.Add(connectionString, tableName, entity) should be called
	And entity should not added to the table

Scenario: when RowKey is null
	Given entity.RowKey is null
	When Add(entity) is called
	Then IFailedHandler.Add(connectionString, tableName, entity) should be called
	And entity should not added to the table

Scenario: when RowKey is empty
	Given entity.RowKey is empty
	When Add(entity) is called
	Then IFailedHandler.Add(connectionString, tableName, entity) should be called
	And entity should not added to the table

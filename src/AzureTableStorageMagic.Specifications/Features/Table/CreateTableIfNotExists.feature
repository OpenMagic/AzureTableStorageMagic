Feature: Table.CreateTableIfNotExists

Background: 
	Given Windows Azure Storage Emulator is running

Scenario: when table does not exist
	Given table does not exist
	When CreateTableIfNotExists(connectionString, tableName) is called
	Then table should be created

Scenario: when table does exist
	Given table does exist
	When CreateTableIfNotExists(connectionString, tableName) is called
	Then nothing should happen

Scenario: when connectionString is null
	Given connectionString is null
	When CreateTableIfNotExists(connectionString, tableName) is called
	Then ArgumentNullException should be thrown for connectionString

Scenario: when connectionString is empty
	Given connectionString is empty
	When CreateTableIfNotExists(connectionString, tableName) is called
	Then ArgumentEmptyStringException should be thrown for connectionString

Scenario: when tableName is null
	Given tableName is null
	When CreateTableIfNotExists(connectionString, tableName) is called
	Then ArgumentNullException should be thrown for tableName

Scenario: when tableName is empty
	Given tableName is empty
	When CreateTableIfNotExists(connectionString, tableName) is called
	Then ArgumentEmptyStringException should be thrown for tableName
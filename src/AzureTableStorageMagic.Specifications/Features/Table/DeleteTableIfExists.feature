Feature: Table.DeleteTableIfExists

Background: 
	Given Windows Azure Storage Emulator is running

Scenario: when table does not exist
	Given table does not exist
	When DeleteTableIfExists() is called
	Then nothing should happen

Scenario: when table does exist
	Given table does exist
	When DeleteTableIfExists() is called
	Then table should be deleted
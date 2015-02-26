﻿Feature: Table.CreateTableIfNotExists

Background: 
	Given Windows Azure Storage Emulator is running

Scenario: when table does not exist
	Given table does not exist
	When CreateTableIfNotExists() is called
	Then table should be created

Scenario: when table does exist
	Given table does exist
	When CreateTableIfNotExists() is called
	Then nothing should happen
Feature: KeyValidator.ValidatePartitionKey

Background: 
    Given PartitionKey is valid
    Given RowKey is valid

Scenario Outline: when PartitionKey is valid
    Given PartitionKey is <PartitionKey>
    When ValidatePartitionKey(partitionKey, validationResults) is called
    Then validationResults count should be 0

    Examples:
        | PartitionKey |
        | example1     |
        | abc          |
        | egf11        |

Scenario Outline: when PartitionKey is null or whitespace
    Given PartitionKey is <PartitionKey>
    When ValidatePartitionKey(partitionKey, validationResults) is called
    Then validationResults count should be 1
    And validationResults should include required validation exception for PartitionKey

    Examples:
        | PartitionKey |
        | null         |
        | empty        |
        | whitespace   |

Scenario Outline: when PartitionKey includes disallowed character
    Given PartitionKey is <PartitionKey>
    When ValidatePartitionKey(partitionKey, validationResults) is called
    Then validationResults count should be 1
    And validationResults should include unacceptable character exception for PartitionKey

    Examples:
        | PartitionKey |
        | /            |
        | \            |
        | #            |
        | ?            |
        | \t           |
        | \r           |

Scenario: when PartitionKey is greater than 1kb
    Given PartitionKey is 1025 characters
    When ValidatePartitionKey(partitionKey, validationResults) is called
    Then validationResults count should be 1
    And validationResults should include to maximum length exception for PartitionKey
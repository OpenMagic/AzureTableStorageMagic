Feature: KeyValidator.ValidateRowKey

Background: 
    Given PartitionKey is valid
    Given RowKey is valid

Scenario Outline: when RowKey is valid
    Given RowKey is <RowKey>
    When ValidateRowKey(RowKey, validationResults) is called
    Then validationResults count should be 0

    Examples:
        | RowKey   |
        | example1 |
        | abc      |
        | egf11    |

Scenario Outline: when RowKey is null or whitespace
    Given RowKey is <RowKey>
    When ValidateRowKey(RowKey, validationResults) is called
    Then validationResults count should be 1
    And validationResults should include required validation exception for RowKey

    Examples:
        | RowKey     |
        | null       |
        | whitespace |

Scenario Outline: when RowKey includes disallowed character
    Given RowKey is <RowKey>
    When ValidateRowKey(RowKey, validationResults) is called
    Then validationResults count should be 1
    And validationResults should include unacceptable character exception for RowKey

    Examples:
        | RowKey |
        | /      |
        | \      |
        | #      |
        | ?      |
        | \t     |
        | \r     |

Scenario: when RowKey is greater than 1kb
    Given RowKey is 1025 characters
    When ValidateRowKey(RowKey, validationResults) is called
    Then validationResults count should be 1
    And validationResults should include to maximum length exception for RowKey
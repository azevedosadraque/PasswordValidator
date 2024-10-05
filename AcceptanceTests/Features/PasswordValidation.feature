Feature: Password Validation

  In order to ensure security
  As a user
  I want my passwords to be validated according to security rules

  Scenario: Valid password
    Given I have the password "AbTp9!fok"
    When I submit the password for validation
    Then the validation result should be "valid"

  Scenario: Password with less than 9 characters
    Given I have the password "aa"
    When I submit the password for validation
    Then the validation result should be "invalid"

  Scenario: Password without special characters
    Given I have the password "AbTp9fok"
    When I submit the password for validation
    Then the validation result should be "invalid"

  Scenario: Password with repeated characters
    Given I have the password "AbTp9!foo"
    When I submit the password for validation
    Then the validation result should be "invalid"

  Scenario: Password with spaces
    Given I have the password "AbTp9 fok"
    When I submit the password for validation
    Then the validation result should be "invalid"

  Scenario: Password with special characters
    Given I have the password "AbTp9@fok"
    When I submit the password for validation
    Then the validation result should be "valid"

  Scenario: Empty password
    Given I have the password ""
    When I submit the password for validation
    Then the validation result should be "invalid"
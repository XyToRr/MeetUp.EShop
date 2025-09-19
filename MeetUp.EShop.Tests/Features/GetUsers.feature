Feature: GetUsers

A short summary of the feature

@requiresUsers @fullCleanUp
Scenario: GetAllUsers
	When try to get all users
	Then all users should be returned

@emptyDB
Scenario: GetAllUsersEmpty
	When try to get all users
	Then no users should be returned

@requresExistingUser @apiChangingDB
Scenario: GetUserByExistingId
	When try to get user by existing id
	Then user should be returned

Scenario: GetUserByNonExistingId
	When try to get user by non-existing id
	Then user should not be returned

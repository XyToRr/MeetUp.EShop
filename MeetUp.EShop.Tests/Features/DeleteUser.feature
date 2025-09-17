Feature: DeleteUser

A short summary of the feature

@apiChangingDB @requresExistingUser @requiresLogin
Scenario: Successfully delete existing user
	When try to delete existing user
	Then user should be deleted successfully

@requresExistingUser @requiresLogin @apiChangingDB
Scenario: Try to delete non-existing user
	When try to delete non-existing user
	Then user should not be found
Feature: UpdateUser

A short summary of the feature

@requresExistingUser @apiChangingDB @requiresLogin
Scenario: UpdateUserSuccessfully
	When updating existing user
	Then the user shoul be updated successfulll

@requresExistingUser @apiChangingDB @requiresLogin
Scenario: UpdateUserWithNonExistingId
	When updating non-existing user
	Then the user should not be updated
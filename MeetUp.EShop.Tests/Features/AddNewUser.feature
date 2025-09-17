Feature: AddNewUser

A short summary of the feature

@apiChangingDB
Scenario: Register valid user
	Given user tries tu register with data:
	
	|   FirstName | LastName   | Email   | Login   | Password   |
    | <FirstName> | <LastName> | <Email> | <Login> | <Password> |

	When user tries to register with given data
	Then user should be registered successfully

	Examples:
  | FirstName | LastName   | Email                             | Login        | Password           |
  | Oleksiy   | Bondarenko | oleksiy.bondarenko@example.com    | oleksiy_b    | M#9vTq2zR8pL       |
  | Kateryna  | Shevchenko | kateryna.shevchenko@example.com   | kat.shev     | kA7!r4PzUx2q       |
  | Ivan      | Moroz      | ivan.moroz@example.com            | ivanmoroz    | !Iv4n*MOroz21      |
  | Maria     | Kovalenko  | maria.kovalenko@example.com       | mariak       | M@r1aKov2025       |
  | Dmytro    | Petrenko   | dmytro.petrenko@example.org       | d_petrenko   | Dp8#t9!xL2wQ       |
  | Olena     | Hrytsenko  | olena.hrytsenko@example.org       | elenah       | O1e!naHry2024      |
  | Andriy    | Kravchuk   | andriy.kravchuk@example.net       | andriy.k     | A#ndriyK99!z       |
  | Natalia   | Lisova     | natalia.lisova@example.net        | nat_lisova   | N@t!l\s0va2023     |
  | Serhii    | Tarasenko  | serhii.tarasenko@example.com      | serh_t       | S3r#TarasX7p       |
  | Viktoriya | Melnyk     | viktoriya.melnyk@example.com      | vika.m       | V!k0r1yaMH45       |

#@apiChangingDB
#Scenario: Register user with invalid data
#	Given user tries tu register with data:
#		|   FirstName | LastName   | Email    | Login     | Password   |
#		| <FirstName> | <LastName> | <Email>  | <Login>   | <Password> |
#	When user tries to register with given data
#	Then user should not be registered
#
#	Examples:
#| FirstName | LastName   | Email                            | Login        | Password           |
#| Oleksiy   | Bondarenko | oleksiy.bondarenko@.com          | oleksiy_b    | M#9vTq2zR8pL       |
#| Kateryna  | Shevchenko | kateryna.shevchenko@com          | kat.shev     | kA7!r4PzUx2q       |
#| Ivan      | Moroz      | ivan.moroz@example               | ivanmoroz    | !Iv4n*MOroz21      |
#| Maria     | Kovalenko  | maria.kovalenko@.example.com     | mariak       | M@r1aKov2025       |
#| Dmytro    | Petrenko   | dmytro.petrenko@example..org     | d_petrenko   | Dp8#t9!xL2wQ       |
#| Olena     | Hrytsenko  | olena.hrytsenko@exam_ple.org     | elenah       | O1e!naHry2024      |
#| Andriy    | Kravchuk   | andriy.kravchuk@example,net      | andriy.k     | A#ndriyK99!z       |
#| Natalia   | Lisova     | natalia.lisova@ example.net      | nat_lisova   | N@t!l\s0va2023     |
#| Serhii    | Tarasenko  | serhii.tarasenko@example..com    | serh_t       | S3r#TarasX7p       |
#| Viktoriya | Melnyk     | viktoriya.melnyk@.example.com    | vika.m       | V!k0r1yaMH45       |



@apiChangingDB
Scenario: Register existing user
	Given user exists:
	    |   FirstName | LastName   | Email    | Login     | Password   |
		| <FirstName> | <LastName> | <Email>  | <Login>   | <Password> |
	And user tries tu register with data:
		|   FirstName | LastName   | Email    | Login     | Password   |
		| <FirstName> | <LastName> | <Email>  | <Login>   | <Password> |

	When user tries to register with given data
	Then user should not be registered

	Examples:
  | FirstName | LastName   | Email                             | Login        | Password           |
  | Oleksiy   | Bondarenko | oleksiy.bondarenko@example.com    | oleksiy_b    | M#9vTq2zR8pL       |
  | Kateryna  | Shevchenko | kateryna.shevchenko@example.com   | kat.shev     | kA7!r4PzUx2q       |
  | Ivan      | Moroz      | ivan.moroz@example.com            | ivanmoroz    | !Iv4n*MOroz21      |
  | Maria     | Kovalenko  | maria.kovalenko@example.com       | mariak       | M@r1aKov2025       |
  | Dmytro    | Petrenko   | dmytro.petrenko@example.org       | d_petrenko   | Dp8#t9!xL2wQ       |
  | Olena     | Hrytsenko  | olena.hrytsenko@example.org       | elenah       | O1e!naHry2024      |
  | Andriy    | Kravchuk   | andriy.kravchuk@example.net       | andriy.k     | A#ndriyK99!z       |
  | Natalia   | Lisova     | natalia.lisova@example.net        | nat_lisova   | N@t!l\s0va2023     |
  | Serhii    | Tarasenko  | serhii.tarasenko@example.com      | serh_t       | S3r#TarasX7p       |
  | Viktoriya | Melnyk     | viktoriya.melnyk@example.com      | vika.m       | V!k0r1yaMH45       |



@apiChangingDB
Scenario: Register user with missing mandatory fields 
	Given user tries tu register with data:
		|   FirstName | LastName   | Email    | Login     | Password   |
		| <FirstName> | <LastName> | <Email>  | <Login>   | <Password> |
	When user tries to register with given data
	Then user should not be registered

Examples:
| FirstName | LastName   | Email                           | Login        | Password           |
| Oleksiy   | Bondarenko | oleksiy.bondarenko@example.com  |              | M#9vTq2zR8pL       |  # ❌ Відсутній логін
| Kateryna  | Shevchenko | kateryna.shevchenko@example.com | kat.shev     |                    |  # ❌ Відсутній пароль
| Ivan      | Moroz      | ivan.moroz@example.com          |              |                    |  # ❌ Відсутній логін і пароль
| Maria     | Kovalenko  | maria.kovalenko@example.com     |              |                    |  # ❌ Відсутній логін і пароль
| Dmytro    | Petrenko   | dmytro.petrenko@example.org     | d_petrenko   |                    |  # ❌ Відсутній пароль
| Olena     | Hrytsenko  | olena.hrytsenko@example.org     |              | O1e!naHry2024      |  # ❌ Відсутній логін




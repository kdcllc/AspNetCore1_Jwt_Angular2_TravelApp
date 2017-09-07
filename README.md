# Travel App
AspNet Core 1.x WebApi with Jwt Token Authentication and Angular2/4 Client App Sample

## Installation
Navigate to src/King.David.Consulting.Travel and run the following commands in the commandline:
```
npm install

npm run build:dev

dotnet restore

dotnet run 
```
## To run Api tester go to:

```
http://localhost:5151/swagger/

```
Simple UI framework was laid out:
```
http://localhost:5151/

````
- Web application is self-hosting with Kesterl with simple SSL certificate. (no need for IIS dependencies)

## Technology Stack

1. Server Side Stack is based on Microsoft AspNetCore 1.x C# code
	- Models Mapping with [AutoMapper](http://automapper.org)
	- Build In Swagger Api tools[Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
	- FluentValidation is used for Models' validation [FluentValidation.AspNetCore](https://github.com/JeremySkinner/fluentvalidation)
	- CQRS build with [MediatR](https://github.com/jbogard/MediatR)
    - Custom User Account management with JWT authentication using [ASP.NET Core JWT Bearer Authentication]
	- SQLite for database

2. Angular2
	- 
	- Webpack

## Active Users
User.cvs was used to populate the folloing users, with passords P@ssword(n) is the user id:
| UserId |	Email				   | Username  |
|--------|:-----------------------:|----------:|
| 1		 | henry.harrison@web.com  | user1	   |
| 2		 | john.smith@web.com	   | user2     |
| 3		 | jim.smith@web.com	   | user3     |
| 4		 | jose.gonzales@web.com   | user4     |
| 5		 | andrew.watson@web.com   | user5     |
| 6		 | bill.brown@web.com	   | user6     |
| 7		 | michael.jackson@web.com | user7     |
| 8		 | eliscia.smith@web.com   | user8     |
| 9		 | chris.jones@web.com	   | user9     |

### Implementation was done based on the Domain Driven Design Pattern
The the following features are implemented:
1. Cities Feature:
 - `GET /cities/` - return Cities in a paged amount (default 20) or all if limit = -1
 - `GET /cities/{user}` - return Cities for the user in a paged amount (default 20) or all if limit = -1
 - `GET /state/{state}/cities` - return Cities in a a given State in a paged amount (default 20) or all if limit = -1. **Required endpoints**
	 
2. States Feature:
 - `GET /user/{user}/visits/states` - return States user has visited in a paged amount (default 20) or all if limit = -1. **Required endpoints**
 - `GET /states/` - return States in a paged amount (default 20) or all if limit = -1

3. Visits Feature:
	- `GET /user/{user}/visits` - return Cities the user has visited in a paged amount (default 20) or all if limit = -1. **Required endpoints**
	- `POST /user/{user}/visits` - allow to create rows of data to indicate they have visited a particular city. If City doesn't exist in the Cities table the Rest Error is thrown. **Required endpoints**

	```
	{
		"city": "Chicago",
		"state": "IL"
	}
	```
	- `DEL /user/{user}/visit/{visit}` - allow a user to remove an improperly pinned visit. **Required endpoints**
    - `POST /visits` - allow authenticated user to create a row of data indicating the visit.
	- `DEL visits/{visit}` - allow authenticated user to remove an improperly pinned visit.
	- `GET /visits` - return Cities in a paged amount (default 20) or all if limit = -1.
	- `GET /visits/user/{user}` return Cities for specified user in a paged amount (default 20) or all if limit = -1.

4. Users Feature:
	- `POST /users` - create a new user
	- `POST /users/login` - login an existing user
	- `GET /user` - must be authenticated user, return will be a current username
	- `PUT /user` - must be authenticated user, update user information.

## Unit Testing
 - Simple set of unit tests for the controllers
 - An attributes and routes validation



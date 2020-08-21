# Etica Coding Challenge

The Tech stack of the API:

*	Asp Net Core 3.1 Web Api
*	Entity Framework Core 3.1.7
*	Entity Framework Core Sqlite 3.1.7
*	Automapper 10.0.0
*	xUnit
*	Swashbuckle.AspNetCore 5.5.1

## The Repository layer

*	EF Core Code First.

The Context and Respository are shown below:

![Repository](https://github.com/VeritasSoftware/EticaCodingChallenge/blob/master/Repository.jpeg)

The Context persists data in a **Sqlite** database.

## The Business layer

*	Transforms the data entity to response model using Automapper.
*	Calls into the Repository layer.

The Business layer is shown below:

![Business](https://github.com/VeritasSoftware/EticaCodingChallenge/blob/master/Business.jpeg)

## The API

*	Calls into the Business layer.
*	Returns data to the client.

![API](https://github.com/VeritasSoftware/EticaCodingChallenge/blob/master/Api.jpeg)
# GamesTracker
**Check out the live site on [videogamestracker.herokuapp.com](https://videogamestracker.herokuapp.com/)**

This is a website that helps keeping track of the video games you are playing, plan to play or have played in the past. You can also add a personal rating to games and show your library to other users by sharing them the URL.

## Powered by
- Angular 12
- Bootstrap 4
- ASP.NET core web api (.NET 5)
- Entity Framework Core
- MySQL

*Note: The metadata for games is queried from the [IGDB](https://www.igdb.com/) api.*

## How to host your own instance on Heroku
1. Fork this repository
2. Get a MySQL database, for example you can get a free database with 100 MB of space on [remotemysql.com](https://remotemysql.com/)
3. Register an account on [twitch.com](https://twitch.com), create a new app and write down the app id and app secret. This is needed to authenticate to the igdb api in order to query the metadata for games.
4. Register an account on [sendinblue.com](https://sendinblue.com) and write down the api key. This is needed to send emails to users (e.g. for password recovery codes).
5. Register an account on [heroku.com](https://heroku.com) and create a new app
6. In the settings tab, configure the following environment variables

```
ASPNETCORE_ENVIRONMENT = Production
JWT_ISSUER_KEY = [put a long, random secret key here]
MYSQL_CONNECTION = [connection string to your mysql instance]
SENDINBLUE_API_KEY = [sendinblue.com api key]
TWITCH_APP_ID = [twitch.com app id]
TWITCH_APP_SECRET = [twitch.com app secret]
```
The connection string for MySQL is formatted like this
```
Server=myServerAddress;Port=1234;Database=myDataBase;Uid=myUsername;Pwd=myPassword;
```

7. In the settings tab, configure heroku to use the following buildpack

```
https://github.com/jincod/dotnetcore-buildpack.git
```

8. In the deploy tab, connect your github and tell heroku to deploy the app from your fork

## Unit tests
There are some unit tests to make sure that the 3rd party services work. To run the unit tests you will need to create a file called `appkeys.json` in the `API/` folder. The file should have the following content
```json
{
	"twitch_app_id": "[twitch.com app id]",
	"twitch_app_secret": "[twitch.com app secret]",
	"sendinblue_api_key": "[sendinblue.com api key]"
}
```

## Future improvements
- Add tests for controllers as well by mocking the repositories with Moq

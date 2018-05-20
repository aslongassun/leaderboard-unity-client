# Leaderboard Client Unity

Unity leaderboard project use UnityWebRequest to send request to server.
The server is Nodejs server you can see [here](https://github.com/aslongassun/nodejs-leaderboard)

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. You can buil to Android or IOS application.

### Prerequisites

- [Unity](https://unity3d.com/)<br />

### Installing

1) Get source from Github: $ git clone https://github.com/aslongassun/leaderboard-unity-client.git <br />
2) Open project and configure Server Url and Port in RequestManager.cs file <br />

- If you run server on local host: <br />
  private static string _Url = "http://localhost"; <br />
  private static string _Port = ":" + "3000"; <br />
- If you run server on a clound such as heroku:
  private static string _Url = "https://[appname].herokuapp.com";
  private static string _Port = "";
  
3) That'is enough for you, run application on Destop or Mobile.<br />

### How To Test
- Currently app send request to server with the url: <br/>
  https://vinhua-nodejs.herokuapp.com <br/>
- The server conneted to online Mongodb on [mlab](https://mlab.com/)<br/>
- You can login app with two role Admin or User<br/>
- User Admin was created with login info username/password: admin/admin<br/>
- You can create user with Register button then Login to application to test<br/>


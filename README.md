# Leaderboard Client Unity

Unity leaderboard project use UnityWebRequest to send request to server.
The server is Nodejs server you can see <br />
[Nodejs-Redis](https://github.com/aslongassun/nodejs-redis)
[Nodejs-MongoDB](https://github.com/aslongassun/nodejs-mongodb)

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. You can build to Android or IOS application.

### Prerequisites

- [Unity install](https://unity3d.com/)<br />

### Installing

1) Get source from Github<br />
```
$ git clone https://github.com/aslongassun/leaderboard-unity-client.git
```
2) Open project and configure Server _Url and _Port in /Asset/Scripts/RequestManager.cs file <br />
```
// Connect to local host
private static string _Url = "http://localhost";
private static string _Port = ":" + "3000";

// Connect to cloud
//private static string _Url = "https://[appname].herokuapp.com";
//private static string _Port = "";

Please also change the Socket Url Of SocketIO Prefab in Unity application

Example for local host:
ws://127.0.0.1:3000/socket.io/?EIO=4&transport=websocket

Example for cloud:
ws://[appname].herokuapp.com/socket.io/?EIO=4&transport=websocket

```

3) Create user with Role Admin to manage user on scoreboard<br />
If you use NodeJs - Redis server: [Nodejs-Redis](https://github.com/aslongassun/nodejs-redis)<br />
Edit /User/UserController.js file<br />

From:<br />
```
var userInfo = JSON.stringify({
'_id': user_id,
'name': req.body.name,
'passwork': req.body.password,
'score': 0,
'updatecounter': 0,
'role':'user',
'updatedAt': currentTime,
'timemilisecond': currentTime.getTime()
});

// login info set
var login_info = req.body.name + "_" + req.body.password;
client.set(login_info, user_id, function(err, reply) {
// add data for leaderboard
client.zadd("users", 0, userInfo);
// add data for admnin leaderboard for order by update time
client.zadd("admin_users", currentTime.getTime(), userInfo);
// inscrease user_id key
client.incr("user:_id");
// add data to set uiser-id info
client.set(user_id, userInfo);
// add set userid logininfo
client.set(user_id+":login", login_info);
// request success
res.status(200).send(userInfo);
console.log(userInfo);
});
```
To:<br />
```
var userInfo = JSON.stringify({
'_id': user_id,
'name': req.body.name,
'passwork': req.body.password,
'score': 0,
'updatecounter': 0,
'role':'admin', //=========EDIT AT THIS LINE========//
'updatedAt': currentTime,
'timemilisecond': currentTime.getTime()
});

// login info set
var login_info = req.body.name + "_" + req.body.password;
client.set(login_info, user_id, function(err, reply) {
// add data for leaderboard
client.zadd("admin", 0, userInfo); //=========EDIT AT THIS LINE========//
// add data for admnin leaderboard for order by update time
//client.zadd("admin_users", currentTime.getTime(), userInfo); //==EDIT AT THIS LINE: remove===//
// inscrease user_id key
client.incr("user:_id");
// add data to set uiser-id info
client.set(user_id, userInfo);
// add set userid logininfo
client.set(user_id+":login", login_info);
// request success
res.status(200).send(userInfo);
console.log(userInfo);
});
```
Then run the server and register user -> now this user when registed by this app will have Admin Role<br />

If you use NodeJs - MongoDB server: [Nodejs-MongoDB](https://github.com/aslongassun/nodejs-mongodb)
Edit /User/UserController.js file<br />

From:<br />
```
// create user
router.post('/', function (req, res) {
    User.create({
        name : req.body.name,
        password : req.body.password,
        score : 0,
        updatecounter: 0,
        lastmodified: new Date(),
        role: 'user'
    }, 
    function (err, user) {
        if (err) return res.status(500).send("Problem when adding user to the database." + err);
        res.status(200).send(user);
        console.log("Create Called");
    });
});
```
To:<br />
```
// create user
router.post('/', function (req, res) {
    User.create({
        name : req.body.name,
        password : req.body.password,
        score : 0,
        updatecounter: 0,
        lastmodified: new Date(),
        role: 'admin' //=========EDIT AT THIS LINE========//
    }, 
    function (err, user) {
        if (err) return res.status(500).send("Problem when adding user to the database." + err);
        res.status(200).send(user);
        console.log("Create Called");
    });
});
```
Then run the server and register user -> now this user when registed by this app will have Admin role<br />

4) Reverse the source and re-run server to create user with Role User<br />

5) Now it's the time to test and learn. 





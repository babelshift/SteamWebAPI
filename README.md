Steam Web API library
========

### Description ###

This is a .NET library that makes it easy to use the Steam Web API. It conveniently wraps around all of the JSON data and ugly API details with clean methods, structures and classes.

The previous versions of this API are obsolete (in this fork), so the old documentation no longer applies.

### Project Structure ###

Three project files are in this GitHub fork. SteamWebAPI.Old contains the code from the base of the fork. SteamWebAPI.WinRT includes my complete overhaul of the code adapted for the Windows Store. SteamWebModel contains model classes that contain data retrieved from the Steam Web Methods to be passed back to whatever calling application requires the data.

### Examples ###

SteamWebSession contains all methods required to retrieve data from the Steam Web API. Using your API key that is obtained from the Steam Web API developer page on Valve's website, pass the developer key into the constructor of the SteamWebSession as shown below.

```C#
string developerKey = "8D06823A74AB641C684EBD95AB5F2E49"; // dummy key, don't use this in your code
var session = new SteamWebSession(developerKey);
```

After instantiating a new web session, you can call methods to retrieve data from the Steam servers as shown below.

##### Get Friend List for Steam ID #####

Button click event to retrieve all friends belonging to a certain SteamID.

```C#
private async void ButtonClick1(object sender, RoutedEventArgs e)
{
  long steamId = 76563197961361044; // dummy steam id, don't use this in your code
  List<Friend> friends = await steamSession.GetFriendListAsync(steamId);
  foreach(var friend in friends)
  {
    long friendSteamID = friend.SteamID;
    string relationship = friend.Relationship;
    DateTime friendSinceDate = friend.FriendSinceDate;
  }
}
```

It's really as easy as that. 
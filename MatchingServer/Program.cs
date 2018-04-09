using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingServer
{
    class Program
    {
        public static QueueList myQueue = new QueueList();
        public static Match matcher = new Match();
        static void Main(string[] args)
        {
            new Program().Run().Wait();
            matcher.printPotentialGroups();
            //myQueue.PrintQueue();
            //Console.ReadLine();
        }

        public async Task Run()
        {
            /*
            User user2 = new User();
            user2.AddPreference("icecream");
            user2.AddPreference("noticream");
            user2.AddPreference("bitch");
            user2.AddPreference("fucker");
            user2.AddPreference("porn");
            user2.MaxRange = 10001;
            user2.Lat = 110;
            user2.Lon = 100;
            user2.UserID = "user2aksdlakdasldk32qdsadasldas";

            User user3 = new User();
            user3.AddPreference("icecream");
            user3.AddPreference("noticream");
            user3.AddPreference("bitch");
            user3.AddPreference("fucker");
            user3.AddPreference("porn");
            user3.MaxRange = 10001;
            user3.Lat = 110;
            user3.Lon = 100;
            user3.UserID = "user3aksdlakdasldkaslds3qwe23adasda123sas";

            User user4 = new User();
            user4.AddPreference("rofl");
            user4.AddPreference("doubt");
            user4.AddPreference("function");
            user4.AddPreference("salut");
            user4.AddPreference("beegees");
            user4.MaxRange = 10001;
            user4.Lat = 110;
            user4.Lon = 100;
            user4.UserID = "user4aksdlakdasldkasl312321dasddsadasda123sas";

            User user5 = new User();
            user5.AddPreference("rofl");
            user5.AddPreference("doubt");
            user5.AddPreference("function");
            user5.AddPreference("salut");
            user5.AddPreference("beegees");
            user5.MaxRange = 10001;
            user5.Lat = 110;
            user5.Lon = 100;
            user5.UserID = "user5aksdlakdasldk23qfsadasldsadasda123sas";
            User user6 = new User();
            user6.AddPreference("rofl");
            user6.AddPreference("doubt");
            user6.AddPreference("function");
            user6.AddPreference("salut");
            user6.AddPreference("porn");
            user6.MaxRange = 10001;
            user6.Lat = 110;
            user6.Lon = 100;
            user6.UserID = "user6aksdlakdasldka3213sdfasdsldsadasda123sas";
            User user7 = new User();
            user7.AddPreference("icecream");
            user7.AddPreference("noticream");
            user7.AddPreference("bitch");
            user7.AddPreference("fucker");
            user7.AddPreference("porn");
            user7.MaxRange = 10001;
            user7.Lat = 110;
            user7.Lon = 100;
            user7.UserID = "user7aksdlakdasldkaslsadasdadsadasda123sas";

            User user8 = new User();
            user8.AddPreference("icecreamasd");
            user8.AddPreference("noticream");
            user8.AddPreference("bitch");
            user8.AddPreference("fucker");
            user8.AddPreference("porn");
            user8.MaxRange = 10001;
            user8.Lat = 110;
            user8.Lon = 100;
            user8.UserID = "user8aksdlakdasldkaslsadasdadsadasda123sas";

            User user9 = new User();
            user9.AddPreference("son");
            user9.AddPreference("off");
            user9.AddPreference("alex");
            user9.AddPreference("dulla");
            user9.AddPreference("steven");
            user9.MaxRange = 10001;
            user9.Lat = 110;
            user9.Lon = 100;
            user9.UserID = "XTyFsywYM7csODRYKNFZPh4Wahf2";


            User user10 = new User();
            user10.AddPreference("not");
            user10.AddPreference("you");
            user10.AddPreference("alex");
            user10.AddPreference("dulla");
            user10.AddPreference("steven");
            user10.MaxRange = 10001;
            user10.Lat = 110;
            user10.Lon = 100;
            user10.UserID = "user8aksdlakdasldkaslsadasdadsadasda123sas";

            myQueue.AddUserToQueue(user2);

            matcher.MatchQueue(myQueue);

            myQueue.AddUserToQueue(user3);
            matcher.MatchQueue(myQueue);

            myQueue.AddUserToQueue(user4);
            matcher.MatchQueue(myQueue);
            myQueue.AddUserToQueue(user5);
            matcher.MatchQueue(myQueue);
            myQueue.AddUserToQueue(user6);
            matcher.MatchQueue(myQueue);
            myQueue.AddUserToQueue(user7);
            matcher.MatchQueue(myQueue);
            myQueue.AddUserToQueue(user8);
            matcher.MatchQueue(myQueue);
            myQueue.AddUserToQueue(user9);
            myQueue.AddUserToQueue(user10);
            matcher.MatchQueue(myQueue);
            matcher.MatchQueue(myQueue);
            
            User user1 = new User();
            user1.AddPreference("Movies");
            user1.AddPreference("Food");
            user1.AddPreference("Sports");
            user1.AddPreference("lov,e");
            user1.AddPreference("clothes");
            user1.MaxRange = 100;
            user1.Lat = 37.421998333333335;
            user1.Lon = -124.08400000000002;
            user1.UserID = "230qasfksalfaskdlsadkasld";
            myQueue.AddUserToQueue(user1);
            */
            //test loop to push to firebase
            while (true)
            {
                IFirebaseConfig config = new FirebaseConfig
                {
                    AuthSecret = "kxpHhB3nvJ3qsBFnNDWphHO7wlXCmwfzyKD3Weh8",
                    BasePath = "https://chatrdk-458bf.firebaseio.com/"
                };
                IFirebaseClient client = new FirebaseClient(config);
                FirebaseResponse response = await client.GetAsync("chatQueue");
                try { 
                    var myJson = response.Body;
                    JObject jObject = JObject.Parse(myJson);
                    foreach (var item in jObject)
                    {
                        JToken currentUser = jObject[item.Key];
    #pragma warning disable IDE0017 // Simplify object initialization
                        User newUser = new User();
    #pragma warning restore IDE0017 // Simplify object initialization

                        newUser.UserID = item.Key;
                        newUser.Lat = Convert.ToDouble(currentUser["Lat"]);
                        newUser.Lon = Convert.ToDouble(currentUser["Lon"]);
                        newUser.MaxRange = Convert.ToDouble(currentUser["MaxRange"]);
                        foreach (var prefer in currentUser["Preferences"])
                        {
                            newUser.AddPreference(prefer.ToString());
                        }
                        myQueue.AddUserToQueue(newUser);
                        FirebaseResponse deleteResponse = await client.DeleteAsync("chatQueue/" + newUser.UserID); //Deletes
                        Console.WriteLine(deleteResponse.StatusCode);

                        //PushResponse tester = await client.PushAsync("chatQueue", newUser);
                        //Console.WriteLine(matcher.GetDistanceFromLatLonInKm(newUser, (User)myQueue.UserList[0]));

                        matcher.MatchQueue(myQueue);
                        matcher.printPotentialGroups();
                    }
                } catch (Exception e)
                {
                    Console.WriteLine(" no javascript object nintendo");
                }

                List<Group> readyGroups = matcher.PrepareReadyGroups();
                for (int i = 0; i < readyGroups.Count; i++)
                {
                    Console.WriteLine("ready group");
                    Console.WriteLine(readyGroups[i]);
                }
                for(int i =0; i < readyGroups.Count; i++)
                {
                    Group currentGroup = readyGroups[i];
                    FirebaseGroup currentFireGroup = currentGroup.FirebaseGroupGenerate();
                    //PushResponse tester = await client.PushAsync("group", currentFireGroup);
                    SetResponse tester = await client.SetAsync($"group/{currentFireGroup.getGroupname()}", currentFireGroup);

                    //travel through each user profile and add the group to their list.
                    for(int j =0; j < currentGroup.Users.Count; j++)
                    {
                        User currentUser = (User)currentGroup.Users[j];
                        String currentuserID = currentUser.UserID;
                        SetResponse userTravelerFirebase = await client.SetAsync($"user/{currentuserID}/group/{currentFireGroup.getGroupname()}", currentFireGroup.getGroupname());
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }


    }
}

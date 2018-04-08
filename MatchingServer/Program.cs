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

            //test case
            User user1 = new User();
            user1.AddPreference("icecream");
            user1.AddPreference("noticream");
            user1.AddPreference("bitch");
            user1.AddPreference("fucker");
            user1.AddPreference("porn");
            user1.MaxRange = 10000;
            user1.Lat = 100;
            user1.Lon = 100;
            user1.UserID = "aksdlakdasldkasldaasfasdsads";

            User user2 = new User();
            user2.AddPreference("icecream");
            user2.AddPreference("noticream");
            user2.AddPreference("bitch");
            user2.AddPreference("fucker");
            user2.AddPreference("porn");
            user2.MaxRange = 10000;
            user2.Lat = 100;
            user2.Lon = 100;
            user2.UserID = "aksdlakdasldkasldas";

            myQueue.AddUserToQueue(user1);
            myQueue.AddUserToQueue(user2);

            matcher.MatchQueue(myQueue);

            //new Program().Run().Wait();

            //myQueue.PrintQueue();
            Console.ReadLine();
        }

        public async Task Run()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "kxpHhB3nvJ3qsBFnNDWphHO7wlXCmwfzyKD3Weh8",
                BasePath = "https://chatrdk-458bf.firebaseio.com/"
            };
            IFirebaseClient client = new FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("chatReq");
            var myJson = response.Body;
            JObject jObject = JObject.Parse(myJson);
            foreach(var item in jObject) {
                JToken currentUser = jObject[item.Key];
#pragma warning disable IDE0017 // Simplify object initialization
                User newUser = new User();
#pragma warning restore IDE0017 // Simplify object initialization

                newUser.UserID = item.Key;
                newUser.Lat = Convert.ToDouble(currentUser["lat"]);
                newUser.Lon = Convert.ToDouble(currentUser["lon"]);
                newUser.MaxRange = Convert.ToDouble(currentUser["range"]);
                foreach (var prefer in currentUser["preferences"])
                {
                    newUser.AddPreference(prefer.ToString());
                }
                myQueue.AddUserToQueue(newUser);
                FirebaseResponse deleteResponse = await client.DeleteAsync("chatReq/" + "8eNnCRIYKTNnA6c8DWoU8Vm9jMA2"); //Deletes todos collection
                Console.WriteLine(deleteResponse.StatusCode);

                PushResponse tester = await client.PushAsync("chatQueue", newUser);
                Console.WriteLine(matcher.GetDistanceFromLatLonInKm(newUser, (User)myQueue.UserList[0]));
            }

            
        }


    }
}

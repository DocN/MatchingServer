using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingServer
{
    class Match
    {
        private const int MAX_PREF_MATCH = 5;
        private const int MIN_PREF_MATCH = 1;

        private const int STARTGROUPLEAD = 0;
        public Match()
        {

        }
        public void MatchQueue(QueueList masterQueueList)
        {
            if (masterQueueList.UserList == null && masterQueueList.UserList.Count < 0)
            {
                Console.WriteLine("no users in queue list");
                return;
            }
            int maxMatchDeincrement = 0;
            Group tempGroup = new Group();
            User grouplead = (User)masterQueueList.UserList[STARTGROUPLEAD];
            tempGroup.AddUser(grouplead);
            masterQueueList.UserList.RemoveAt(STARTGROUPLEAD);

            for(int i =0; i< masterQueueList.UserList.Count; i++)
            {
                User currentUser = (User)masterQueueList.UserList[i];
                double firstUserLimitRange = grouplead.MaxRange;
                double secondUserLimitRange = grouplead.MaxRange;
                double distanceAway = this.GetDistanceFromLatLonInKm(grouplead, currentUser);

                //if they're within their required distance ranges then check their preferences
                if(distanceAway <= firstUserLimitRange && distanceAway <= secondUserLimitRange)
                {
                    Console.WriteLine("Distance match");
                    ArrayList currentMatches = GetMatches(grouplead, currentUser);
                    if (currentMatches.Count >= (MAX_PREF_MATCH - maxMatchDeincrement))
                    {
                        Console.WriteLine("Preference match & distance match");
                        tempGroup.AddUser(currentUser);
                        masterQueueList.UserList.RemoveAt(i);
                    }
                }
            }
        }

        public double GetDistanceFromLatLonInKm(User user1, User user2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = Deg2rad(user2.Lat - user1.Lat);  // deg2rad below
            var dLon = Deg2rad(user2.Lat - user1.Lat);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +  Math.Cos(Deg2rad(user1.Lat)) * Math.Cos(Deg2rad(user2.Lat)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }
        
        public ArrayList GetMatches(User user1, User user2)
        {
            ArrayList firstUserPref = user1.Preferences;
            ArrayList secUserPref = user2.Preferences;
            ArrayList matchedPrefs = new ArrayList();

            for(int i =0;i < firstUserPref.Count; i++)
            {
                for(int j =0; j < secUserPref.Count; j++)
                {
                    if(firstUserPref[i].Equals(secUserPref[j]))
                    {
                        matchedPrefs.Add(firstUserPref[i]);
                    }
                }
            }
            return matchedPrefs;
        }

        public double Deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
 
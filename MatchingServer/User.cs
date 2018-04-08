using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingServer
{
    class User
    {
        private string userID;
        private double maxRange;
        private double lat;
        private double lon;
        private ArrayList preferences;
        private int timeInQueue;
        private const int DEFAULT_TIME = 0;
        public User()
        {
            Preferences = new ArrayList();
            timeInQueue = DEFAULT_TIME;
        }

        public string UserID { get => userID; set => userID = value; }
        public double MaxRange { get => maxRange; set => maxRange = value; }
        public double Lat { get => lat; set => lat = value; }
        public double Lon { get => lon; set => lon = value; }
        public int TimeInQueue { get => timeInQueue; set => timeInQueue = value; }
        public ArrayList Preferences { get => preferences; set => preferences = value; }

        public void AddPreference(String currentPref)
        {
            Preferences.Add(currentPref);
        }

        public void PrintPreferences()
        {
            for(int i=0; i< Preferences.Count; i++)
            {
                Console.WriteLine(Preferences[i]);
            }
        }

        public override string ToString() {
            return "UserID = " + userID + " Distance: " + maxRange + " Lat: " + Lat + " Lon: " + Lon; 
        }
    }
}

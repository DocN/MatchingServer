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
        private const int MAX_PREF_MATCH = 1;
        private const int MIN_PREF_MATCH = 1;

        private const int STARTGROUPLEAD = 0;

        private List<Group> potentialGroups;

        public Match()
        {
            potentialGroups = new List<Group>();
        }

        public void MatchQueue(QueueList masterQueueList)
        {
            //check precondition
            if (masterQueueList.UserList == null && masterQueueList.UserList.Count < 0)
            {
                Console.WriteLine("no users in queue list");
                return;
            }

            bool matched = matchExistingGroup(masterQueueList);
            //try to match a new group if there's no group that works
            if(matched == false)
            {
                matchNewGroup(masterQueueList);
            }
            
        }

        private bool matchExistingGroup(QueueList masterQueueList)
        {
            if(potentialGroups.Count <= 0)
            {
                return false;
            }
            bool matched = false;
            List<User> matchedUsers = new List<User>();
            ArrayList usersInQueue = masterQueueList.UserList;

            for (int i =0; i < potentialGroups.Count; i++)
            {
                for(int j = 0; j< usersInQueue.Count; j++)
                {
                    Group currentGroup = potentialGroups[i];
                    //check if current group is already at max capacity if so skip it
                    if(currentGroup.memberLimit())
                    {
                        continue;
                    }
                    User grouplead  = currentGroup.GetGroupLead();
                    User currentUser = (User)usersInQueue[j];
                    double firstUserLimitRange = grouplead.MaxRange;
                    double secondUserLimitRange = currentUser.MaxRange;
                    double distanceAway = this.GetDistanceFromLatLonInKm(grouplead, currentUser);

                    if (distanceAway <= firstUserLimitRange && distanceAway <= secondUserLimitRange) { 
                        ArrayList matches = GetMatchesAgainstGroup(currentGroup, currentUser);
                        int matchesRequired = currentGroup.groupFormPatience();
                        Console.WriteLine("matches Required " + matchesRequired);
                        //found someone to join group
                        if(matches.Count >= matchesRequired)
                        {
                            Console.WriteLine("matched user to existing group" + currentUser.ToString());
                            currentGroup.AddUser(currentUser);
                            matchedUsers.Add(currentUser);
                            matched = true;
                        }
                    }
                }
                //remove users that have been matched
                for(int j =0; j< matchedUsers.Count; j++)
                {
                    User currentUser = matchedUsers[j];
                    int currentUserIndex = masterQueueList.UserList.IndexOf(currentUser);
                    if(currentUserIndex >= 0)
                    {
                        masterQueueList.UserList.RemoveAt(currentUserIndex);
                    }
                }
            }

            return matched;
        }
        private bool matchNewGroup(QueueList masterQueueList)
        {
            if (masterQueueList.UserList.Count <= 0)
            {
                return false;
            }
            Console.WriteLine("making new group");
            int maxMatchDeincrement = 0;
            Group tempGroup = new Group();
            User grouplead = (User)masterQueueList.UserList[STARTGROUPLEAD];
            tempGroup.AddUser(grouplead);
            masterQueueList.UserList.RemoveAt(STARTGROUPLEAD);
            Boolean createdGroup = false;
            for (int i = 0; i < masterQueueList.UserList.Count; i++)
            {
                User currentUser = (User)masterQueueList.UserList[i];
                Console.WriteLine("current user " + currentUser);
                double firstUserLimitRange = grouplead.MaxRange;
                double secondUserLimitRange = currentUser.MaxRange;
                double distanceAway = this.GetDistanceFromLatLonInKm(grouplead, currentUser);

                //if they're within their required distance ranges then check their preferences
                Console.WriteLine(distanceAway);
                if (distanceAway <= firstUserLimitRange && distanceAway <= secondUserLimitRange)
                {
                    Console.WriteLine("Distance match");
                    ArrayList currentMatches = GetMatches(grouplead, currentUser);
                    if (currentMatches.Count >= (MAX_PREF_MATCH - maxMatchDeincrement))
                    {
                        Console.WriteLine("Preference match & distance match");
                        tempGroup.AddUser(currentUser);
                        tempGroup.MatchedPreferences = currentMatches;
                        createdGroup = true;
                    }
                }
            }
            if (createdGroup)
            {
                potentialGroups.Add(tempGroup);
                removeGroupedUsers(tempGroup, masterQueueList);
            }
            else
            {
                masterQueueList.UserList.Add(grouplead);
            }
            return createdGroup;
        }
        public void removeGroupedUsers(Group newGroup, QueueList masterQueueList) 
        {
            for(int i =0; i < newGroup.Users.Count; i++)
            {
                int currentIndex = masterQueueList.UserList.IndexOf(newGroup.Users[i]);
                //don't remove anything if it's not there
                if(currentIndex != -1)
                {
                    masterQueueList.UserList.RemoveAt(currentIndex);
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

        public ArrayList GetMatchesAgainstGroup(Group group1, User user1)
        {
            ArrayList preferences = group1.MatchedPreferences;
            ArrayList userPreferences = user1.Preferences;
            ArrayList matches = new ArrayList();

            for(int i = 0; i< preferences.Count; i++)
            {
                for(int j =0; j < userPreferences.Count; j++)
                {
                    String currentGroupPreference = (String)preferences[i];
                    String currentUserPreference = (String)userPreferences[j];

                    //Console.WriteLine("matching " + currentGroupPreference);
                    //Console.WriteLine("matching " + currentUserPreference);
                    if (currentGroupPreference.Equals(currentUserPreference))
                    {
                        matches.Add(currentGroupPreference);
                    }
                }
            }
            return matches;
        }

        public double Deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        public void printPotentialGroups()
        {
            for(int i =0; i< potentialGroups.Count; i++)
            {
                Group currentGroup = (Group)potentialGroups[i];
                Console.WriteLine(currentGroup);
            }
        }

        public List<Group> PrepareReadyGroups()
        {
            List<Group> readyGroups = new List<Group>();
            for(int i =0; i < potentialGroups.Count; i++)
            {
                Group currentGroup = potentialGroups[i];
                bool currentGroupStatus = currentGroup.CheckTimeLimit();
                //ready to be sent off 
                if(currentGroupStatus)
                {
                    readyGroups.Add(currentGroup);
                }
            }
            RemoveReadyGroups(readyGroups);
            return readyGroups;
        }

        private void RemoveReadyGroups(List <Group> readyGroups)
        {
            for(int i =0; i < readyGroups.Count; i++)
            {
                //find the element and remove it from the group of rooms waiting to be sent to ready list.
                int readyGroupIndex = potentialGroups.IndexOf(readyGroups[i]);
                potentialGroups.RemoveAt(readyGroupIndex);
            }
        }
    }
}
 
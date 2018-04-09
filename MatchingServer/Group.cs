using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingServer
{

    class Group
    {
        private String groupName;
        private ArrayList users;
        private ArrayList matchedPreferences;
        private long groupCreationTime;
        private const int LEADER_INDEX = 0;
        private const int GROUPMAXPATIENCE = 2;
        private const int MINGROUPMATCH = 1;
        private const int SECONDS_PER_PATIENCE = 10;
        private const int MAX_WAIT_TIME_CLOSE_GROUP = 15;
        private const int MAX_MEMBERS = 5;
        private static Random random = new Random();
        private int groupFormPatienceCount;
        
       
        public Group()
        {
            Users = new ArrayList();
            MatchedPreferences = new ArrayList();
            groupCreationTime = UnixTimeNow();
            groupFormPatienceCount = GROUPMAXPATIENCE;
            groupName = this.randomID();
        }

        public ArrayList Users { get => users; set => users = value; }
        public ArrayList MatchedPreferences { get => matchedPreferences; set => matchedPreferences = value; }

        public void AddUser(User currentUser)
        {
            Users.Add(currentUser);
        }

        public int groupFormPatience()
        {
            if(groupFormPatienceCount == 1)
            {
                return groupFormPatienceCount;
            }
            long timePassed = this.UnixTimeNow() - this.groupCreationTime;
            groupFormPatienceCount = groupFormPatienceCount - unchecked((int)(timePassed / SECONDS_PER_PATIENCE));
            if(groupFormPatienceCount < 1)
            {
                groupFormPatienceCount = MINGROUPMATCH;
            }
            return groupFormPatienceCount;
        }

        public long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        public Boolean memberLimit()
        {
            if(users.Count >= MAX_MEMBERS)
            {
                return true;
            }
            return false;
        }

        private String randomID()
        {
            string a = GetLetter() + DateTime.Now.Month.ToString() +
            DateTime.Now.Day.ToString() +
            DateTime.Now.Year.ToString() +
            DateTime.Now.Hour.ToString() +
            DateTime.Now.Minute.ToString() +
            DateTime.Now.Second.ToString() +
            DateTime.Now.Millisecond.ToString() + GetLetter();
            return a;
        }

         
        public static char GetLetter()
        {
            
            // This method returns a random lowercase letter
            // ... Between 'a' and 'z' inclusize.
            int num = random.Next(0, 26); // Zero to 25
            char let = (char)('a' + num);
            return let;
        }



        public override string ToString()
        {
            String output = "------------------Group: " + this.groupName + "--------------------------\n";
            for(int i =0; i< users.Count; i++)
            {
                output = output + (User)users[i] + "\n";
            }
            output = output + "\n" + "----------------------Matched Preferences---------------------- \n";
            for (int i =0; i< MatchedPreferences.Count; i++)
            {
                output = output + (String)matchedPreferences[i] + "\n";
            }
            return output;
        }
        public User GetGroupLead()
        {
            User theUser = null;
            if (Users.Count > 0)
            {
                theUser = (User)Users[LEADER_INDEX];
            }
            return theUser;
        }
        
        public bool CheckTimeLimit()
        {
            long currentTime = this.UnixTimeNow();
            long timePassedInSeconds = currentTime - groupCreationTime;
            //check if time elapsed for closing group
            if(timePassedInSeconds >= MAX_WAIT_TIME_CLOSE_GROUP)
            {
                return true;
            }
            return false;
        }

        /*used for converting to a firebase acceptable data format */
        public FirebaseGroup FirebaseGroupGenerate()
        {
            FirebaseGroup fireGroup = new FirebaseGroup();
            //copy all users into member
            for(int i =0; i < users.Count; i++)
            {
                fireGroup.addMember((User)users[i]);
            }

            //copy preferences
            for(int i =0; i < MatchedPreferences.Count; i++)
            {
                fireGroup.addPreference((String)MatchedPreferences[i]);
            }

            fireGroup.setGroupName(groupName);
            return fireGroup;
        }
    }
}

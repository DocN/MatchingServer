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
        private const int MAX_MEMBERS = 5;

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
            string a = DateTime.Now.Month.ToString() +
            DateTime.Now.Day.ToString() +
            DateTime.Now.Year.ToString() +
            DateTime.Now.Hour.ToString() +
            DateTime.Now.Minute.ToString() +
            DateTime.Now.Second.ToString() +
            DateTime.Now.Millisecond.ToString();
            return a;
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
    }
}

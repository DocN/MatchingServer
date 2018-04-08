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
        private ArrayList users;
        private ArrayList matchedPreferences;
        private int numberOfUsers;
        private long groupCreationTime; 
       
        public Group()
        {

            Users = new ArrayList();
            MatchedPreferences = new ArrayList();
            NumberOfUsers = 0;
            groupCreationTime = UnixTimeNow();
        }

        public ArrayList Users { get => users; set => users = value; }
        public ArrayList MatchedPreferences { get => matchedPreferences; set => matchedPreferences = value; }
        public int NumberOfUsers { get => numberOfUsers; set => numberOfUsers = value; }

        public void AddUser(User currentUser)
        {
            Users.Add(currentUser);
        }
        public long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

    }
}

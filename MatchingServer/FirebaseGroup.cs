using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingServer
{
    

    class FirebaseGroup
    {

        private FirebaseGroupInfo groupinfoData;
        private ArrayList memberData;
        private ArrayList matchedPreferencesData;

        public FirebaseGroupInfo groupInfo { get => groupinfoData; set => groupinfoData = value; }
        public ArrayList member { get => memberData; set => memberData = value; }
        public ArrayList MatchedPreferences { get => matchedPreferencesData; set => matchedPreferencesData = value; }

        public FirebaseGroup()
        {
            groupinfoData = new FirebaseGroupInfo();
            matchedPreferencesData = new ArrayList();
            member = new ArrayList();
        }

        public void setAdmin(String newAdmin)
        {
            groupInfo.admin = newAdmin;
        }
        public String getAdmin()
        {
            return groupInfo.admin;
        }

        public void setGroupName(String newGroupName)
        {
            groupInfo.name = newGroupName;
        }

        public String getGroupname()
        {
            return groupInfo.name;
        }

        public void addMember(User currentUser)
        {
            member.Add(currentUser.UserID);
        }
        public void addPreference(String pref)
        {
            MatchedPreferences.Add(pref);
        }
    }
}

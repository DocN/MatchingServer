using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingServer
{
    class QueueList
    {
        ArrayList userList;
        public QueueList()
        {
            UserList = new ArrayList();
        }

        public ArrayList UserList { get => userList; set => userList = value; }

        public void AddUserToQueue(User newUser)
        {
            UserList.Add(newUser);   
        }

        public void PrintQueue()
        {
            for(int i =0; i < UserList.Count; i++)
            {
                Console.WriteLine(UserList[i]);
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingServer
{

    class FirebaseGroupInfo
    {
        private String adminData;
        private String nameData;

        public FirebaseGroupInfo()
        {
            //defaults
            this.admin = "XTyFsywYM7csODRYKNFZPh4Wahf2";
            this.name = "";
        }
        public string admin { get => adminData; set => adminData = value; }
        public string name { get => nameData; set => nameData = value; }
    }
}

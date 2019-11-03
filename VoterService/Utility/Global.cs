using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoterService.Utility
{
    public class Global
    {
        public static string DefaultConnection = "";
        public static int VOTED = 3;
        public static int NOT_VOTED = 2;
        public static int ENTERED = 1;

//        public const int SEARCH_USERID = 1;
//        public const int SEARCH_ID = 2;

        // Generate DB connection string according to the district id.
        public static string GetDbConnectionString(int nIdx)
        {
            string connectionString = DefaultConnection + nIdx;

            return connectionString;
        }
    }
}

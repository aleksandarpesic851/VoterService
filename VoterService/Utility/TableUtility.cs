using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace VoterService.Utility
{
    public class TableUtility
    {
        //Check responsable table is existing. If not, create table voter_idx
        public static bool CheckCreateTable()
        {
            SqlConnection SC = new SqlConnection(Global.DefaultConnection);
            try
            {
                string cmdText = @"IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
                       WHERE TABLE_NAME='voter') SELECT 1 ELSE SELECT 0";
                SC.Open();
                SqlCommand cmdCheck = new SqlCommand(cmdText, SC);
                int x = Convert.ToInt32(cmdCheck.ExecuteScalar());

                //If already exist a table, return
                if (x == 1)
                    return true;
            
                // The following code uses an SqlCommand based on the SqlConnection.
                using (SqlCommand command = new SqlCommand("CREATE TABLE voter (id int IDENTITY(1,1) PRIMARY KEY, userid varchar(256), username varchar(256), nic varchar(256), address varchar(256), district int, province int, vote_state int);", SC))
                {
                    command.ExecuteNonQuery();
                }
                SC.Close();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static List<int> GetTableNames()
        {
            List<int> arrTableIdx = new List<int>();
            SqlConnection SC = new SqlConnection(Global.DefaultConnection);
            
            if (SC.State == ConnectionState.Open)
            {
                List<string> arrTableName = SC.GetSchema("Tables").AsEnumerable().Select(s => s[2].ToString()).ToList();
                foreach(string table in arrTableName)
                {
                    string[] arrTmp = table.Split("_");
                    arrTableIdx.Add(Convert.ToInt32(arrTmp[1]));
                }
                return arrTableIdx;
            }
            SC.Close();
            //Add some error-handling instead !
            return arrTableIdx;
        }
    }
}

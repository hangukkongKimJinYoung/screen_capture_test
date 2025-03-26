using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections;

namespace BitmapImageSaveTest.Services
{
    public class MysqlDataService
    {
        private string _remoteNickname;
        private IPEndPoint _remoteEP;
        private string _databaseName;
        private string _databaseId;
        private string _databasePwd;
        private string _connectionStr;


        internal MysqlDataService(string remoteNickname, IPEndPoint remoteEP, string databaseName, string id, string pwd)
        {
            this._remoteNickname = remoteNickname;
            this._remoteEP = remoteEP;
            this._databaseName = databaseName;
            this._databaseId = id;
            this._databasePwd = pwd;

            MakeConnectionStr(this._remoteEP, this._databaseName, this._databaseId, this._databasePwd);
        }


        /// <summary>
        /// 커넥션 할 때 암호 연결 그런것도 있어서 나중에 설정 정보 같은 것을(다른 클래스이고 필드로 가지겠다~) 참조해서 커넥션 문자열을 만들거나
        /// 쿼리 하나 할 때마다 문자열 만드는 것은 비효율적이니까 설정이 바뀌었을 때만 새로 만들어서 필드로 저장하면 좋을듯
        /// </summary>
        private void MakeConnectionStr(IPEndPoint remoteEP, string dbName, string id, string pwd)
        {
            this._connectionStr = $"Server={remoteEP.Address.ToString()};Database={dbName};Uid={id};Pwd={pwd}";
        }

        public void ExecuteQuery(string query)
        {
            using (MySqlConnection conn = GetConnection())
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.ExecuteNonQuery();
                }
            }
        }

        public void ExecuteQuery(MySqlCommand comm)
        {
            using (MySqlConnection conn = GetConnection())
            {
                using (comm)
                {
                    comm.Connection = conn;
                    comm.ExecuteNonQuery();
                }
            }
        }

        public DataTable SelectQuery(string query)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = GetConnection())
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                {
                    adapter.Fill(dt);
                }
            }

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            return null;
        }

        private MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(this._connectionStr);
            conn.Open();

            return conn;
        }
    }
}

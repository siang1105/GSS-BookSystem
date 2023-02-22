using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace bookSystem.Dao
{
    public class codeDao : IcodeDao
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return bookSystem.Common.configTool.GetDBConnectionString("DBConn");
        }

        /// <summary>
        /// 取得書本狀態Table資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookStatusTable(string bookStatus)
        {
            DataTable dt = new DataTable();
            List<SelectListItem> result = new List<SelectListItem>();
            string sql = @" SELECT CODE_ID, CODE_NAME FROM BOOK_CODE WHERE CODE_TYPE = @bookStatus ";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookStatus", bookStatus));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            return DealWithSelectListData(dt, "CODE_NAME", "CODE_ID");
        }

        /// <summary>
        /// 取得書本類別Table資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookClassTable()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT BOOK_CLASS_ID, BOOK_CLASS_NAME FROM BOOK_CLASS";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            return DealWithSelectListData(dt, "BOOK_CLASS_NAME", "BOOK_CLASS_ID");
        }

        /// <summary>
        /// 取得借閱人Table資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetUserNameTable(string type)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT USER_ID, USER_ENAME, USER_CNAME, USER_ENAME + '-' + USER_CNAME AS N'UserECName' FROM MEMBER_M";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            if (type == "Chinese")
            {
                return DealWithSelectListData(dt, "UserECName", "USER_ID");
            }
            else
            {
                return DealWithSelectListData(dt, "USER_ENAME", "USER_ID");
            }

        }

        /// <summary>
        /// 將Table資料轉換成select list 
        /// </summary>
        private List<SelectListItem> DealWithSelectListData(DataTable dt, string textColumn, string valueColumn)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row[textColumn]?.ToString(),
                    Value = row[valueColumn]?.ToString()
                });
            }
            return result;
        }


    }
}

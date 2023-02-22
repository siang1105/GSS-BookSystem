using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookSystem.Dao
{
    public class bookDao : IbookDao
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
        /// 新增書本
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public void InsertBook(bookSystem.Model.bookInsert bookInsertData)
        {
            string sql = @" INSERT INTO BOOK_DATA 
                                (BOOK_NAME, BOOK_CLASS_ID, BOOK_AUTHOR, BOOK_BOUGHT_DATE
                                , BOOK_PUBLISHER, BOOK_NOTE, BOOK_STATUS, BOOK_KEEPER
                                , BOOK_AMOUNT, CREATE_DATE, CREATE_USER, MODIFY_DATE, MODIFY_USER)
	                        VALUES 
                                (@bookName, @bookClassID, @bookAuthor, CAST(@bookBoughtDate AS SMALLDATETIME)
                                , @bookPublisher, @bookNote, 'A', '', '',  GETDATE(),'admin', GETDATE(), 'admin')";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookName", bookInsertData.bookName));
                cmd.Parameters.Add(new SqlParameter("@bookAuthor", bookInsertData.bookAuthor));
                cmd.Parameters.Add(new SqlParameter("@bookBoughtDate", bookInsertData.bookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@bookPublisher", bookInsertData.bookPublisher));
                cmd.Parameters.Add(new SqlParameter("@bookClassID", bookInsertData.bookClassID));
                cmd.Parameters.Add(new SqlParameter("@bookNote", bookInsertData.bookNote));
                //bookId = Convert.ToInt32(cmd.ExecuteScalar());//執行查詢，並傳回查詢所傳回之結果集中第一個資料列的第一個資料行。 忽略其他資料行或資料列
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// 刪除書本
        /// </summary>
        public void DeleteBookById(int bookId)
        {
            try
            {
                string sql = "Delete FROM BOOK_DATA Where BOOK_ID = @bookId";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@bookId", bookId));
                    cmd.ExecuteNonQuery();//不會傳回任何資料列，但對應至參數的任何輸出參數或傳回值會填入資料。對 UPDATE、INSERT 和 DELETE 陳述式而言，傳回值是受命令影響的資料列數目
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 依照條件取得書本資料
        /// </summary>
        /// <returns></returns>
        public List<bookSystem.Model.book> GetBookByCondtioin(bookSystem.Model.bookSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT D.BOOK_ID AS N'書本ID', C.BOOK_CLASS_NAME AS N'圖書類別', C.BOOK_CLASS_ID AS N'圖書類別代號', 
                                  D.BOOK_NAME AS N'書名', CONVERT(VARCHAR, D.BOOK_BOUGHT_DATE, 111) AS N'購書日期',
                                  CODE.CODE_NAME AS N'借閱狀態', CODE.CODE_ID AS N'借閱代號',ISNULL(M.USER_ENAME, ' ') AS N'借閱人', 
                                  ISNULL(M.USER_ID, ' ') AS N'借閱人ID', D.BOOK_AUTHOR AS N'作者', D.BOOK_NOTE AS N'書籍簡介', 
                                  D.BOOK_PUBLISHER AS N'出版商', ISNULL(M.USER_CNAME, ' ') AS N'借閱人中文姓名'
                           FROM BOOK_DATA AS D
                           INNER JOIN BOOK_CLASS AS C
	                            ON D.BOOK_CLASS_ID = C.BOOK_CLASS_ID
                           INNER JOIN BOOK_CODE AS CODE
	                            ON D.BOOK_STATUS = CODE.CODE_ID AND CODE.CODE_TYPE_DESC = '書籍狀態'
                           LEFT JOIN MEMBER_M AS M
	                            ON D.BOOK_KEEPER = M.USER_ID
                           WHERE (C.BOOK_CLASS_ID = @bookClassID OR @bookClassID = '') AND 
                                 (D.BOOK_NAME LIKE '%' + @bookName + '%' OR @bookName = '') AND
                                 (CODE.CODE_ID = @bookStatusCode OR @bookStatusCode = '') AND 
                                 (M.USER_ID = @userID OR @userID = '') AND (D.BOOK_ID = @bookId OR @bookId = '')
                           ORDER BY D.BOOK_BOUGHT_DATE DESC";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookClassID", arg.bookClassID ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@bookName", arg.bookName ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@bookStatusCode", arg.bookStatusCode ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@userID", arg.userId ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@bookId", arg.bookId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            return this.MapBookDataToList(dt);
        }

        public bookSystem.Model.book GetBookById(int bookId)
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT C.BOOK_CLASS_ID AS N'圖書類別代號', D.BOOK_NAME AS N'書名', CONVERT(char(10), D.BOOK_BOUGHT_DATE, 126) AS N'購書日期',
                                  CODE.CODE_ID AS N'借閱代號', ISNULL(D.BOOK_KEEPER, ' ') AS N'借閱人ID', D.BOOK_AUTHOR AS N'作者', D.BOOK_NOTE AS N'書籍簡介', 
                                  D.BOOK_PUBLISHER AS N'出版商', CODE.CODE_NAME AS N'借閱狀態'
                           FROM BOOK_DATA AS D
                           INNER JOIN BOOK_CLASS AS C
	                            ON D.BOOK_CLASS_ID = C.BOOK_CLASS_ID
                           INNER JOIN BOOK_CODE AS CODE
	                            ON D.BOOK_STATUS = CODE.CODE_ID AND CODE.CODE_TYPE_DESC = '書籍狀態'
                           WHERE (D.BOOK_ID = @bookId)";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookId", bookId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return MapBookDataByIdToList(dt);
            }

        }



        /// <summary>
        /// 編輯書本
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int UpdateBook(bookSystem.Model.book book)
        {
            string sql = @" UPDATE BOOK_DATA
                            SET BOOK_NAME = @bookName
                               ,BOOK_AUTHOR = @bookAuthor
                               ,BOOK_PUBLISHER = @bookPublisher
                               ,BOOK_NOTE = @bookNote
                               ,BOOK_BOUGHT_DATE = @bookBoughtDate
                               ,BOOK_CLASS_ID = @bookClassID
                               ,BOOK_STATUS = @bookStatusCode
                               ,BOOK_KEEPER = @userID
                            FROM BOOK_DATA
                            WHERE BOOK_ID = @bookId ";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookName", book.bookName));
                cmd.Parameters.Add(new SqlParameter("@bookAuthor", book.bookAuthor));
                cmd.Parameters.Add(new SqlParameter("@bookPublisher", book.bookPublisher));
                cmd.Parameters.Add(new SqlParameter("@bookNote", book.bookNote));
                cmd.Parameters.Add(new SqlParameter("@bookBoughtDate", book.bookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@userID", book.userId ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@bookClassID", book.bookClassID));
                cmd.Parameters.Add(new SqlParameter("@bookStatusCode", book.bookStatusCode));
                cmd.Parameters.Add(new SqlParameter("@bookId", book.bookId));
                cmd.ExecuteScalar();
                conn.Close();
            }
            return 0;
        }



        /// <summary>
        /// 依照條件取得借閱紀錄
        /// </summary>
        /// <returns></returns>
        public List<bookSystem.Model.bookLendRecord> GetBookLendRecord(int bookId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT
	                            CONVERT(VARCHAR, L.LEND_DATE, 111) AS N'借閱日期'
                                ,L.KEEPER_ID AS N'借閱人員編號'
                                ,M.USER_ENAME AS N'英文姓名'
                                ,M.USER_CNAME AS N'中文姓名'
                            FROM BOOK_LEND_RECORD AS L
                            INNER JOIN MEMBER_M AS M
	                            ON L.KEEPER_ID = M.USER_ID
                            WHERE L.BOOK_ID = @bookId";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookId", bookId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            return this.MapBookLendRecordToList(dt);

        }

        /// <summary>
        /// 新增借閱紀錄
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public void InsertBookLendRecord(int bookId, string userId)
        {
            string sql = @" INSERT INTO BOOK_LEND_RECORD 
                            (BOOK_ID, KEEPER_ID, LEND_DATE, CRE_DATE, CRE_USR, MOD_DATE, MOD_USR)
	                        VALUES
                            (@bookId, @userId, GETDATE(), GETDATE(), 'admin', GETDATE(), 'admin')";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@bookId", bookId));
                cmd.Parameters.Add(new SqlParameter("@userId", userId));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }


        /// <summary>
        /// Map資料進List
        /// </summary>
        /// <param name="bookData"></param>
        /// <returns></returns>
        private List<bookSystem.Model.book> MapBookDataToList(DataTable bookData)
        {
            List<bookSystem.Model.book> result = new List<bookSystem.Model.book>();
            foreach (DataRow row in bookData.Rows)
            {
                result.Add(new bookSystem.Model.book()
                {
                    bookId = (int)row["書本ID"],
                    bookClassName = row["圖書類別"]?.ToString(),
                    bookClassID = row["圖書類別代號"]?.ToString(),
                    bookName = row["書名"]?.ToString(),
                    bookBoughtDate = row["購書日期"]?.ToString(),
                    bookStatus = row["借閱狀態"]?.ToString(),
                    bookStatusCode = row["借閱代號"]?.ToString(),
                    userName = row["借閱人"]?.ToString(),
                    userCName = row["借閱人中文姓名"]?.ToString(),
                    userId = row["借閱人ID"]?.ToString(),
                    bookAuthor = row["作者"]?.ToString(),
                    bookNote = row["書籍簡介"]?.ToString(),
                    bookPublisher = row["出版商"]?.ToString(),
                });
            }
            return result;

        }

        /// <summary>
        /// Map資料進List
        /// </summary>
        /// <param name="bookLendData"></param>
        /// <returns></returns>
        private List<bookSystem.Model.bookLendRecord> MapBookLendRecordToList(DataTable bookLendData)
        {
            List<bookSystem.Model.bookLendRecord> result = new List<bookSystem.Model.bookLendRecord>();
            foreach (DataRow row in bookLendData.Rows)
            {

                result.Add(new bookSystem.Model.bookLendRecord()
                {
                    bookLendDate = row["借閱日期"]?.ToString(),
                    bookKeeperID = row["借閱人員編號"]?.ToString(),
                    userEName = row["英文姓名"]?.ToString(),
                    userCName = row["中文姓名"]?.ToString()
                });
            }
            return result;
        }

        private bookSystem.Model.book MapBookDataByIdToList(DataTable bookData)
        {

            bookSystem.Model.book result = new bookSystem.Model.book()
            {
                bookClassID = bookData.Rows[0]["圖書類別代號"]?.ToString(),
                bookName = bookData.Rows[0]["書名"]?.ToString(),
                bookBoughtDate = bookData.Rows[0]["購書日期"]?.ToString(),
                bookStatusCode = bookData.Rows[0]["借閱代號"]?.ToString(),
                bookStatus = bookData.Rows[0]["借閱狀態"]?.ToString(),
                userId = bookData.Rows[0]["借閱人ID"]?.ToString(),
                bookAuthor = bookData.Rows[0]["作者"]?.ToString(),
                bookNote = bookData.Rows[0]["書籍簡介"]?.ToString(),
                bookPublisher = bookData.Rows[0]["出版商"]?.ToString()
            };
            return result;
        }
    }
}

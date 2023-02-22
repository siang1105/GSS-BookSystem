using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookSystem.Service
{
    public class bookService : IbookService
    {
        private bookSystem.Dao.IbookDao bookDao { get; set; }

        public void InsertBook(bookSystem.Model.bookInsert bookInsertData)
        {
            bookSystem.Dao.bookDao bookDao = new bookSystem.Dao.bookDao();
            bookDao.InsertBook(bookInsertData);
        }

        public void DeleteBookById(int bookId)
        {
            bookSystem.Dao.bookDao bookDao = new bookSystem.Dao.bookDao();
            bookDao.DeleteBookById(bookId);
        }

        public List<bookSystem.Model.book> GetBookByCondtioin(bookSystem.Model.bookSearchArg arg)
        {
            return bookDao.GetBookByCondtioin(arg);
        }

        public bookSystem.Model.book GetBookById(int bookId)
        {
            bookSystem.Dao.bookDao bookDao = new bookSystem.Dao.bookDao();
            return bookDao.GetBookById(bookId);
        }

        public int UpdateBook(bookSystem.Model.book book)
        {
            return bookDao.UpdateBook(book);
        }

        public List<bookSystem.Model.bookLendRecord> GetBookLendRecord(int bookId)
        {
            return bookDao.GetBookLendRecord(bookId);
        }
        public void InsertBookLendRecord(int bookId, string userId)
        {
            bookDao.InsertBookLendRecord(bookId, userId);
        }
    }
}

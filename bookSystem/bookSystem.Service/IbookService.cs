using bookSystem.Model;
using System.Collections.Generic;

namespace bookSystem.Service
{
    public interface IbookService
    {
        void DeleteBookById(int bookId);
        List<book> GetBookByCondtioin(bookSearchArg arg);
        book GetBookById(int bookId);
        List<bookLendRecord> GetBookLendRecord(int bookId);
        void InsertBook(bookInsert bookInsertData);
        void InsertBookLendRecord(int bookId, string userId);
        int UpdateBook(book book);
    }
}
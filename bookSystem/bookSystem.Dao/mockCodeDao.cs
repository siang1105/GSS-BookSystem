using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace bookSystem.Dao
{
    public class MockCodeDao : IcodeDao
    {
        private string rootCodeDataFilePath = @"C:\Users\carol_SL_chang\Desktop\carol_chang\course6\bookSystem\bookSystem\test\";

        List<SelectListItem> IcodeDao.GetBookClassTable()
        {
            string bookClassFilePath = rootCodeDataFilePath + "BOOK_CLASS.txt";

            var lines = File.ReadAllLines(bookClassFilePath);
            List<SelectListItem> result = new List<SelectListItem>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                result.Add(new SelectListItem()
                {
                    Text = item.Split(splitChar.ToCharArray())[1],
                    Value = item.Split(splitChar.ToCharArray())[0]
                });
            }
            return result;
        }

        List<SelectListItem> IcodeDao.GetBookStatusTable(string bookStatus)
        {
            string bookCodeFilePath = rootCodeDataFilePath + "BOOK_CODE.txt";

            var lines = File.ReadAllLines(bookCodeFilePath);
            List<SelectListItem> result = new List<SelectListItem>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                if (item.Split(splitChar.ToCharArray())[0] == bookStatus)
                {
                    result.Add(new SelectListItem()
                    {
                        Text = item.Split(splitChar.ToCharArray())[3],
                        Value = item.Split(splitChar.ToCharArray())[1]
                    });
                }

            }
            return result;
        }

        List<SelectListItem> IcodeDao.GetUserNameTable(string type)
        {
            string memborFilePath = rootCodeDataFilePath + "MEMBER_M.txt";

            var lines = File.ReadAllLines(memborFilePath);
            List<SelectListItem> result = new List<SelectListItem>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                result.Add(new SelectListItem()
                {
                    Text = item.Split(splitChar.ToCharArray())[1],
                    Value = item.Split(splitChar.ToCharArray())[0]
                });
            }
            return result;
        }
    }
}

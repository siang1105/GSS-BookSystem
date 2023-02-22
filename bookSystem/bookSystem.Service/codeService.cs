using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace bookSystem.Service
{
    public class codeService : IcodeService
    {
        private bookSystem.Dao.IcodeDao codeDao { get; set; }
        public List<SelectListItem> GetBookStatusTable(string bookStatus)
        {
            return codeDao.GetBookStatusTable(bookStatus);
        }

        public List<SelectListItem> GetBookClassTable()
        {
            return codeDao.GetBookClassTable();
        }

        public List<SelectListItem> GetUserNameTable(string type)
        {
            return codeDao.GetUserNameTable(type);
        }
    }
}

using System.Collections.Generic;
using System.Web.Mvc;

namespace bookSystem.Dao
{
    public interface IcodeDao
    {
        List<SelectListItem> GetBookClassTable();
        List<SelectListItem> GetBookStatusTable(string bookStatus);
        List<SelectListItem> GetUserNameTable(string type);
    }
}
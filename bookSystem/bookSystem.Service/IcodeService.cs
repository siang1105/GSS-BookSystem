using System.Collections.Generic;
using System.Web.Mvc;

namespace bookSystem.Service
{
    public interface IcodeService
    {
        List<SelectListItem> GetBookClassTable();
        List<SelectListItem> GetBookStatusTable(string bookStatus);
        List<SelectListItem> GetUserNameTable(string type);
    }
}
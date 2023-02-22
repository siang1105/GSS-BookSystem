using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace bookSystem.Model
{
    public class bookSearchArg
    {

        [DisplayName("書本ID")]
        public int bookId { get; set; }

        [DisplayName("書籍類別")]
        public string bookClassID { get; set; }

        [AllowHtml]
        [DisplayName("書名")]
        [StringLength(40, ErrorMessage = "{0}不可超過{1}個字")]
        public string bookName { get; set; }

        [DisplayName("購書日期")]
        public string bookBoughtDate { get; set; }

        [DisplayName("借閱狀態")]
        public string bookStatusCode { get; set; }

        [DisplayName("借閱人")]
        public string userId { get; set; }
    }
}

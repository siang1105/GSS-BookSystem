using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookSystem.Model
{
    public class bookLendRecord
    {
        [DisplayName("借閱日期")]
        public string bookLendDate { get; set; }
        [DisplayName("借閱人員編號")]
        public string bookKeeperID { get; set; }

        [DisplayName("英文姓名")]
        public string userEName { get; set; }

        [DisplayName("中文姓名")]
        public string userCName { get; set; }
    }
}

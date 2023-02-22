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
    public class bookInsert
    {

        [DisplayName("書籍類別")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookClassID { get; set; }

        [AllowHtml]
        [DisplayName("書名")]
        [StringLength(40, ErrorMessage = "{0}不可超過{1}個字")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookName { get; set; }

        [DisplayName("購書日期")]
        [MyValidateDateRange()]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookBoughtDate { get; set; }

        [AllowHtml]
        [DisplayName("作者")]
        [StringLength(40, ErrorMessage = "{0}不可超過{1}個字")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookAuthor { get; set; }

        [AllowHtml]
        [DisplayName("出版商")]
        [StringLength(40, ErrorMessage = "{0}不可超過{1}個字")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookPublisher { get; set; }

        [AllowHtml]
        [DisplayName("內容簡介")]
        [StringLength(300, ErrorMessage = "{0}不可超過{1}個字")]
        [Required(ErrorMessage = "此欄位必填")]
        public string bookNote { get; set; }
    }


    public class MyValidateDateRangeAttribute : ValidationAttribute
    {
        // Methods
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                // invalid
                var errorMsg = string.Format("此欄位必填");
                return new ValidationResult(errorMsg);
            }
            int stringLen = ((string)value).Length;
            if (stringLen > 10)
            {
                // invalid
                var errorMsg = string.Format("日期格式錯誤");
                return new ValidationResult(errorMsg);
            }

            DateTime bookBoughtDate = Convert.ToDateTime(value);
            if (bookBoughtDate > DateTime.Now)
            {
                // invalid
                var errorMsg = string.Format("購書日期不可大於當前日期");
                return new ValidationResult(errorMsg);
            }
            else if (bookBoughtDate < Convert.ToDateTime("1911 / 10 / 10"))
            {
                // invalid
                var errorMsg = string.Format("非有效日期");
                return new ValidationResult(errorMsg);
            }
            else
            {
                // valid
                return ValidationResult.Success;
            }
        }
    }
}

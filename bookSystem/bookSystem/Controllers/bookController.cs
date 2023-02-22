using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bookSystem.Controllers
{
    public class requestResult
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
    public class bookController : Controller
    {
        bookSystem.Service.IcodeService codeService = new bookSystem.Service.codeService();
        bookSystem.Service.IbookService bookService = new bookSystem.Service.bookService();
        // GET: book
        [HttpGet()]
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return View("Error");
            }
            
        }

        /// <summary>
        /// 書本資料查詢(查詢)
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult Index(bookSystem.Model.bookSearchArg arg)
        {
            try
            {
                return Json(bookService.GetBookByCondtioin(arg));
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }
        }

        /// <summary>
        /// 新增書本畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult InsertBook()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return View("Error");
            }
        }

        /// <summary>
        /// 新增書本
        /// </summary>
        /// <param name="bookInsertData"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult InsertBook(bookSystem.Model.bookInsert bookInsertData)
        {
            try
            {
                string message = "";
                bool success;
                if (ModelState.IsValid)
                {
                    bookService.InsertBook(bookInsertData);
                    message = "新增成功";
                    success = true;
                }
                else
                {
                    message = "新增失敗";
                    success = false;
                }
                return Json(new requestResult { message = message, success = success });
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }

            
        }

        /// <summary>
        /// 修改書本畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult UpdateBook(int bookId)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return View("Error");
            }
        }

        /// <summary>
        /// 修改書本
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult UpdateBook(bookSystem.Model.book book)
        {
            try
            {
                string message = "";
                bool success;

                if (ModelState.IsValid)
                {
                    bookService.UpdateBook(book);
                    if (book.bookStatusCode == "B" || book.bookStatusCode == "C")
                    {
                        bookService.InsertBookLendRecord(book.bookId, book.userId);
                    }
                    message = "修改成功";
                    success = true;
                }
                else
                {
                    message = "修改失敗";
                    success = false;
                }
                return Json(new requestResult { message = message, success = success });
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }
            
        }

        [HttpPost()]
        public JsonResult GetBookDataById(int bookId)
        {
            try
            {
                return Json(bookService.GetBookById(bookId));
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }
            
        }
        /// <summary>
        /// 明細書本畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult BookDetail()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return View("Error");
            }
        }

        /// <summary>
        /// 明細書本資料
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult BookDetail(int bookId)
        {
            try
            {
                bookSystem.Model.bookSearchArg bookSearchArg = new bookSystem.Model.bookSearchArg()
                {
                    bookId = bookId
                };

                var SearchResult = bookService.GetBookByCondtioin(bookSearchArg);

                return Json(SearchResult.FirstOrDefault());//查FirstOrDefault()
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }
            
        }

        /// <summary>
        /// 刪除書本
        /// </summary>
        /// <param name="bookID"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteBook(int bookId)
        {
            try
            {
                var SearchResult = bookService.GetBookById(bookId);
                string message = "";
                bool success;
                if (SearchResult.bookStatusCode == "B" || SearchResult.bookStatusCode == "C")
                {
                    message = "刪除失敗";
                    success = false;
                }
                else
                {
                    bookService.DeleteBookById(bookId);
                    message = "刪除成功";
                    success = true;
                }
                return Json(new requestResult { message = message, success = success });
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }

            
        }

        /// <summary>
        /// 借閱紀錄畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult BookLendRecord()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return View("Error");
            }
        }

        [HttpPost()]
        public JsonResult BookLendRecord(int bookId)
        {
            try
            {
                return Json(bookService.GetBookLendRecord(bookId));
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }
            
        }

        [HttpPost()]
        public JsonResult GetBookClassData()
        {
            try
            {
                return Json(codeService.GetBookClassTable());
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }
            
        }

        [HttpPost()]
        public JsonResult GetUserNameData()
        {
            try
            {
                return Json(codeService.GetUserNameTable("English"));
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }
            
        }

        [HttpPost()]
        public JsonResult GetUserECNameData()
        {
            try
            {
                return Json(codeService.GetUserNameTable("Chinese"));
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }
            
        }

        [HttpPost()]
        public JsonResult GetBookStatusData()
        {
            try
            {
                return Json(codeService.GetBookStatusTable("BOOK_STATUS"));
            }
            catch (Exception ex)
            {
                bookSystem.Common.Logger.Write(bookSystem.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return Json("Error");
            }
            
        }


    }
}
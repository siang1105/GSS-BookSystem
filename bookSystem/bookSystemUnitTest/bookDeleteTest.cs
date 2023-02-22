using bookSystem.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Web.Mvc;

namespace bookSystem.Test
{
    [TestClass]
    public class bookDeleteTest
    {

        [TestMethod]
        public void deleteTest1()
        {
            bookController bookController = new bookController();
            JsonResult result = bookController.DeleteBook(1);
            var det = result.Data.GetType().GetProperty("message", BindingFlags.Instance | BindingFlags.Public);
            var dataVal = det.GetValue(result.Data, null);

            Assert.AreEqual("刪除失敗", dataVal);
        }

        [TestMethod]
        public void deleteTest2()
        {
            bookController bookController = new bookController();
            bookSystem.Model.bookInsert bookInsert = new bookSystem.Model.bookInsert()
            {
                bookClassID = "BK",
                bookName = "test",
                bookBoughtDate = "2023/2/21",
                bookAuthor = "testAuthor",
                bookPublisher = "testPublisher",
                bookNote = "testNote"
            };
            bookController.InsertBook(bookInsert);

            JsonResult result = bookController.DeleteBook(2341);
            var data = result.Data.GetType().GetProperty("message", BindingFlags.Instance | BindingFlags.Public);
            var dataVal = data.GetValue(result.Data, null);

            Assert.AreEqual("刪除成功", dataVal);
        }
    }
}

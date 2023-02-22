//取得書本Id
var urlParams = new URLSearchParams(location.search);
var bookId = urlParams.get('bookId');

//取得書本明細資料
$.ajax({
    type: "POST",
    url: "/book/BookDetail",
    data: "bookId=" + bookId,
    dataType: "json",
    success: function (response) {
        $("#bookName").html(response.bookName);
        $("#bookAuthor").html(response.bookAuthor);
        $("#bookPublisher").html(response.bookPublisher);
        $("#bookNote").html(response.bookNote);
        $("#bookBoughtDate").html(response.bookBoughtDate);
        $("#bookClassName").html(response.bookClassName);
    }, error: function (error) {
        alert("系統發生錯誤");
    }
});
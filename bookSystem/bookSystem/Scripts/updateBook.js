//自訂驗證 kendo validator
$("#updateBookData").kendoValidator({
    rules: {
        validDate: function (input) {
            if (input.is("[id=bookBoughtDate]") && $("#bookBoughtDate").data("kendoDatePicker").value() > new Date()) {
                return false;
            }
            else {
                return true
            }
        },
        maxLength: function (input) {
            if ((input.is("[id=bookName]") || input.is("[id=bookAuthor]") || input.is("[id=bookPublisher]")) &&
                (input.val().length > 40)) {
                return false;
            }
            else {
                return true
            }
        },
        maxLengthForTextArea: function (input) {
            if ((input.is("[id=bookNote]")) && (input.val().length > 300)) {
                return false;
            }
            else {
                return true
            }
        }
    },
    messages: {
        required: "此欄位必填",
        validDate: "購書日期不可大於當前日期",
        maxLength: "輸入長度不可超過40",
        maxLengthForTextArea: "輸入長度不可超過300"
    }
});

//kendo datePicker
$("#bookBoughtDate").kendoDatePicker({
    format: "yyyy/MM/dd",
    dateInput: true
});

//書本狀態為已借出或已借出未領將借閱人設成必填 否則設成唯讀
function bookStatusChange() {
    if ($("#statusDropDownList").data("kendoDropDownList").value() == "A" || $("#statusDropDownList").data("kendoDropDownList").value() == "U") {
        $("#userDropDownList").data("kendoDropDownList").select(0);
        $("#userDropDownList").data("kendoDropDownList").enable(false);
        $("#userDropDownList").prop('required', false);
    }
    else {
        $("#userDropDownList").data("kendoDropDownList").enable(true);
        $("#userDropDownList").prop('required', true);

    }
}

//取得書本Id
var urlParams = new URLSearchParams(location.search);
var bookId = urlParams.get('bookId');

//取得書本原始資料
$.ajax({
    type: "POST",
    url: "/book/GetBookDataById",
    data: "bookId=" + bookId,
    dataType: "json",
    success: function (response) {
        $("#bookName").val(response.bookName);
        $("#bookAuthor").val(response.bookAuthor);
        $("#bookPublisher").val(response.bookPublisher);
        $("#bookNote").val(response.bookNote);
        $("#bookBoughtDate").val(response.bookBoughtDate);
        $("#categoryDropDownList").data("kendoDropDownList").value(response.bookClassID);
        $("#userDropDownList").data("kendoDropDownList").value(response.userId);
        $("#statusDropDownList").data("kendoDropDownList").value(response.bookStatusCode);
        if (response.bookStatusCode == "A" || response.bookStatusCode == "U") {
            $("#userDropDownList").data("kendoDropDownList").enable(false);
        }
    }, error: function (error) {
        alert("系統發生錯誤");
    }
});

//類別下拉選單
$("#categoryDropDownList").kendoDropDownList({
    dataTextField: "Text",
    dataValueField: "Value",
    dataSource: {
        transport: {
            read: {
                url: "/book/GetBookClassData",
                dataType: "json",
                type: "post"
            }
        }
    },
    optionLabel: "請選擇"
});

//借閱人下拉選單
$("#userDropDownList").kendoDropDownList({
    dataTextField: "Text",
    dataValueField: "Value",
    dataSource: {
        transport: {
            read: {
                url: "/book/GetUserECNameData",
                dataType: "json",
                type: "post"
            }
        }
    },
    change: bookStatusChange,
    optionLabel: "請選擇"
});

//借閱狀態下拉選單
$("#statusDropDownList").kendoDropDownList({
    dataTextField: "Text",
    dataValueField: "Value",
    dataSource: {
        transport: {
            read: {
                url: "/book/GetBookStatusData",
                dataType: "json",
                type: "post"
            }
        }
    },
    change: bookStatusChange,
    optionLabel: "請選擇"
});


//更新書本資料
$("#updateBookData").submit(function (e) {
    e.preventDefault();
    var bookData = {
        bookName: $("#bookName").val(),
        bookAuthor: $("#bookAuthor").val(),
        bookPublisher: $("#bookPublisher").val(),
        bookNote: $("#bookNote").val(),
        bookBoughtDate: $("#bookBoughtDate").val(),
        bookClassID: $("#categoryDropDownList").data("kendoDropDownList").value(),
        bookStatusCode: $("#statusDropDownList").data("kendoDropDownList").value(),
        userId: $("#userDropDownList").data("kendoDropDownList").value(),
        bookId: bookId
    }
    var validator = $("#updateBookData").kendoValidator().data("kendoValidator");
    if (validator.validate()) {
        $.ajax({
            type: "POST",
            url: "/book/UpdateBook",
            data: bookData,
            dataType: "json",
            success: function (response) {
                alert(response.message);
                if (response.success) {
                    location.href = "/book/Index";
                }
            }, error: function (error) {
                alert("系統發生錯誤");
            }
        });
    }

})

//確認刪除視窗
function confirmDelete(e) {
    e.preventDefault();
    $.ajax({
        type: "POST",
        url: "/book/GetBookDataById",
        data: "bookId=" + bookId,
        dataType: "json",
        success: function (response) {
            if (response.bookStatusCode == "B" || response.bookStatusCode == "C") {
                $("#dialog").kendoDialog({
                    title: "書本借出中",
                    content: "書本已被借出，無法刪除。書本狀態 : " + response.bookStatus,
                    actions: [{
                        text: "OK",
                    }]
                });
                $('#dialog').data("kendoDialog").open();
            }
            else {
                $('#dialog').kendoDialog({
                    width: "450px",
                    title: "確認刪除",
                    closable: false,
                    content: "<p>是否確認要刪除這本書 ?<p>",
                    actions: [
                        { text: 'No' },
                        {
                            text: 'Yes', primary: true, action: function () {
                                deleteBook(bookId);
                            }
                        }
                    ],
                });
                $('#dialog').data("kendoDialog").open();
            }
        }, error: function (error) {
            alert("系統發生錯誤");
        }
    });
}

//刪除書本
function deleteBook(bookid) {
    $.ajax({
        type: "post",
        url: "/book/deletebook",
        data: "bookid=" + bookid,
        datatype: "json",
        success: function (response) {
            alert(response.message);
            if (response.success) {
                location.href = "/book/Index";
            }
        }, error: function (error) {
            alert("系統發生錯誤");
        }
    });
}
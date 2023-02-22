
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

//使用者下拉選單
$("#userDropDownList").kendoDropDownList({
    dataTextField: "Text",
    dataValueField: "Value",
    dataSource: {
        transport: {
            read: {
                url: "/book/GetUserNameData",
                dataType: "json",
                type: "post"
            }
        }
    },
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
    optionLabel: "請選擇"
});

//搜尋書本
$("form#searchBookData").submit(function (e) {
    e.preventDefault();
    $("#bookGrid").data("kendoGrid").dataSource.read();
})

//書本 kendo grid
$("#bookGrid").kendoGrid({
    dataSource: {
        transport: {
            read: {
                url: "/book/Index",
                dataType: "json",
                type: "post",
                data: function () {
                    return {
                        bookName: $("#bookName").val(),
                        bookClassID: $("#categoryDropDownList").data("kendoDropDownList").value(),
                        userId: $("#userDropDownList").data("kendoDropDownList").value(),
                        bookStatusCode: $("#statusDropDownList").data("kendoDropDownList").value()
                    }
                }
            }
        },
        pageSize: 20,
    },
    height: 550,
    sortable: true,
    pageable: {
        input: true,
        numeric: false
    },
    selectable: true,
    columns: [
        { field: "bookClassName", title: "書籍類別", width: "20%" },
        { field: "bookName", title: "書名", template: '<a href="/book/BookDetail?bookId=#= bookId #">#= bookName #</a>', width: "30%" },
        { field: "bookBoughtDate", title: "購書日期", width: "15%" },
        { field: "bookStatus", title: "借閱狀態", width: "15%" },
        { field: "userName", title: "借閱人", width: "15%" },
        {
            command: {
                text: "借閱紀錄", click: function (e) {
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    window.location.href = "/book/BookLendRecord?bookId=" + dataItem.bookId
                }
            }, title: " ", width: "100px"
        },
        {
            command: {
                text: "修改", click: function (e) {
                    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    window.location.href = "/book/UpdateBook?bookId=" + dataItem.bookId
                }
            }, title: " ", width: "75px"
        },
        { command: { text: "刪除", click: confirmDelete }, title: " ", width: "80px" }
    ]
});

//書名auto complete
$("#bookName").kendoAutoComplete({
    placeholder: "請輸入書名",
    dataTextField: "bookName",
    filter: "contains",
    dataSource: {
        transport: {
            read: {
                url: "/book/Index",
                dataType: "json",
                type: "post"
            }
        }
    }
});

//kendo dialog 確認刪除視窗
function confirmDelete(e) {
    e.preventDefault();
    var grid = $("#bookGrid").data("kendoGrid")
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);
    if (data.bookStatusCode == "B" || data.bookStatusCode == "C") {
        $("#dialog").kendoDialog({
            title: "書本借出中",
            content: "書本已被借出，無法刪除。書本狀態 : " + data.bookStatus,
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
                        deleteBook(e, data.bookId);
                    }
                }
            ],
        });
        $('#dialog').data("kendoDialog").open();
    }
}

//刪除書本
function deleteBook(e, bookId) {
    e.preventDefault();
    $.ajax({
        type: "POST",
        url: "/book/DeleteBook",
        data: "bookId=" + bookId,
        dataType: "json",
        success: function (response) {
            alert(response.message);
            if (response.success) {
                $("#bookGrid").data("kendoGrid").dataSource.read();
            }   
        }, error: function (error) {
            alert("系統發生錯誤");
        }
    });
}
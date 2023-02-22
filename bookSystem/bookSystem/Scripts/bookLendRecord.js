//取得書本Id
var urlParams = new URLSearchParams(location.search);
var bookId = urlParams.get('bookId');

//書本借閱紀錄kendo grid
$("#bookLendRecordGrid").kendoGrid({
    dataSource: {
        transport: {
            read: {
                url: "/book/BookLendRecord",
                dataType: "json",
                type: "post",
                data: function () {
                    return {
                        bookId: bookId
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
        { field: "bookLendDate", title: "借閱日期", width: "25%" },
        { field: "bookKeeperID", title: "借閱人員編號", width: "25%" },
        { field: "userEName", title: "英文姓名", width: "25%" },
        { field: "userCName", title: "中文姓名", width: "25%" }
    ]
});
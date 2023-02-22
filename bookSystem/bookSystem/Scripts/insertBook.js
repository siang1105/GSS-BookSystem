//自訂驗證 kendo validator
$("#insertBookData").kendoValidator({
    rules: {
        validDate: function (input) {//日期不可大於當前日期
            if (input.is("[id=bookBoughtDate]") && $("#bookBoughtDate").data("kendoDatePicker").value() > new Date()) {

                return false;
            }
            else {
                return true
            }
        },
        maxLength: function (input) {//一般輸入框長度不可超過40
            if ((input.is("[id=bookName]") || input.is("[id=bookAuthor]") || input.is("[id=bookPublisher]")) &&
                (input.val().length > 40)) {
                return false;
            }
            else {
                return true
            }
        },
        maxLengthForTextArea: function (input) {//書籍簡介輸入框長度不可超過300
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

//kendo DatePicker
$("#bookBoughtDate").kendoDatePicker({
    format: "yyyy/MM/dd",
    dateInput: true
});

//日期預設為今天
$("#bookBoughtDate").data("kendoDatePicker").value(new Date());

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

//新增書本
$("#insertBookData").submit(function (e) {
    e.preventDefault();
    var bookData = {
        bookName: $("#bookName").val(),
        bookAuthor: $("#bookAuthor").val(),
        bookPublisher: $("#bookPublisher").val(),
        bookNote: $("#bookNote").val(),
        bookBoughtDate: kendo.toString($("#bookBoughtDate").data("kendoDatePicker").value(), 'yyyy-MM-dd'),
        bookClassID: $("#categoryDropDownList").data("kendoDropDownList").value()
    }
    var validator = $("#insertBookData").kendoValidator().data("kendoValidator");
    if (validator.validate()) {
        $.ajax({
            type: "POST",
            url: "/book/InsertBook",
            data: bookData,
            dataType: "json",
            success: function (response) {
                console.log(response)
                alert(response.message)
                if (response.success) {
                    location.href = "/book/Index";
                }
            }, error: function (error) {
                alert("系統發生錯誤");
            }
        });
    }

})
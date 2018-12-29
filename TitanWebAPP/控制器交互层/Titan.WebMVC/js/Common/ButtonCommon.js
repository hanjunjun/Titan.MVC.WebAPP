//禁用提交按钮..这个事件会在submit按钮绑定的方法执行完成之后执行。
$(document).delegate("form", "submit", function () {
    
    $(this).find("button[type='submit']").attr("disabled", "disabled");
    $(this).css("cursor", "Wait"); //设置光标为等待样式
    //$(this).css("cursor", "url(../../img/046.gif),auto"); //未来替换为cur格式的图片展示
});

//还原提交按钮
var realCallback;
//注册ajax成功回调
function formSuccessCallback(data) {
    
    window[realCallback](data);
    if (data != "success") {
        $(this).find("button[type='submit']").removeAttr("disabled");
        $(this).css("cursor", "Default"); //还原光标样式
    }
}
//注册ajax失败回调
function formFailureCallback() {
    
    $(this).find("button[type='submit']").removeAttr("disabled");
    $(this).css("cursor", "Default"); //还原光标样式
}
//给form绑定鼠标按下事件
$(document).delegate("form", "keypress", function () {
    
    if ($(this).attr("data-ajax-success") != "formSuccessCallback") {
        realCallback = $(this).attr("data-ajax-success");
    }
    $(this).attr("data-ajax-success", "formSuccessCallback");
    $(this).attr("data-ajax-failure", "formFailureCallback");
});
//给form下的submit 绑定鼠标穿过事件
$(document).delegate("form button[type='submit']", "mouseenter", function () {
    
    if ($(this).parents("form").attr("data-ajax-success") != "formSuccessCallback") {
        realCallback = $(this).parents("form").attr("data-ajax-success");
    }
    $(this).parents("form").attr("data-ajax-success", "formSuccessCallback");
    $(this).parents("form").attr("data-ajax-failures", "formFailureCallback");
});
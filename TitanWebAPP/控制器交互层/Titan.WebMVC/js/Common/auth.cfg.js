var respPreHandler = function (resp) {
    //todo 添加交互UI
    alert("操作失败，" + resp.data.Message);
}

var respErrorHandler = function (errorResp) {
    alert("侦测到错误(" + errorResp.status + ")。错误信息：" + errorResp.data.Message);
}

//封装某些上传插件 不调用axios的时候，的header问题。
function getHeader(){
    if (getTicket() === "") {
        alert('message');
        
    }else{
        alert("00000message");
    }
}
var getHeader = {
    'auth': getTicket()
}

//创建axios 普通请求的公共对象
var AxiosObj = axios.create({
    baseURL: _baseUrl,
    headers: {
        'auth': getTicket()
    }
});
//请求发送时的拦截器功能
AxiosObj.interceptors.request.use(function (config) {
    // Do something before request is sent
    config.headers["auth"] = getTicket();
    return config;
}, function (error) {
    // Do something with request error
    return Promise.reject(error);
});

//服务器响应的拦截器功能
AxiosObj.interceptors.response.use(
    function (response) {
        if (response.status == 404) {
            alert(response.statusText)
        }
        //身份认证信息有效期自动延长
        let storage = getStorageObj();
        if (storage && response && response.headers && response.headers.auth) {
            storage.setItem(_param, response.headers.auth);
            AxiosObj.defaults.headers.common['auth'] = response.headers.auth;
        }
        //todo 选择有必要的,需要提前处理的信息在此处处理,如果不需要在此处处理,请在then或error中处理.
        //请求失败的信息,请尽可能抽象并提取到功能函数统一处理。

        //ResultType的代码与信息对应关系:
        //0=操作没有引发任何变化，提交取消。
        //1=操作成功。
        //2=操作引发错误。
        //3=指定参数的数据不存在。
        //4=输入信息验证失败。
        //5=登录失效。
        //6=身份认证信息错误。
        //7=未登录。
        let resultData = response.data;
        if (resultData.Successed && resultData.Successed == true) {

        } else {
            respPreHandler(response);
            return;
        }
        return response;
    },
    function (error) {
        console.log('----------axios respone error------------->');
        console.log(error);
        if (!error) {
            return Promise.reject("Network unknown error");
        }

        if (error.response) {
            if (error.response.status == 401) {
                console.log(error.response.data);
                //ResultType的代码与信息对应关系:
                //5=登录失效。
                //6=身份认证信息错误。
                //7=未登录。
                let ResultData = error.response.data;
                if (!ResultData.Successed) {
                    let msg = ResultData.Message + "-" + ResultData.ResultType;
                    alert(msg); //todo 提示的交互UI需要修改,由于页面可能同时并行多个请求,所以提示功能需要限制仅显示一个实例.
                    window.location.href = "login.html"; //todo 提供更友好的转向提示,并且能转向到登录页,例如提示框包含一个锁屏和重新登录的按钮.
                }
            } else if (error.response.status == 404) {
                alert("Api Not Found(404)"); //todo 提示的交互UI需要修改,尽量不阻塞UI进程.
            } else {
                console.log('----------axios respone error------------->');
                console.log(error.response);
                respErrorHandler(error.response);
            }
        } else if (error.request) {
            //请求已创建,但无服务器响应
            //1:在浏览器时,'error.request'对象是XMLHttpRequest实例
            //2:在Node.js时,'error.request'对象是http.ClientRequest实例
            console.log('----------axios no respone error------------->');
            console.log(error.request);
            alert("Api Not Found(404)");
            //window.location.href = '../../404.html';
        } else {
            // Something happened in setting up the request that triggered an Error
            console.log('Error', error.message);
        }
        return Promise.reject(error);
    }
);
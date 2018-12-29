/**
 * 获取token
 */
function getToken() {
    var token = localStorage["Token"];
    if (token == undefined || token == null)
        token = "";
    return token;
}

/**
 * 写入token
 * @param {string} token
 */
function setToken(token) {
    localStorage["Token"] = token;
}

/**
 * 清除token
 */
function clearToken() {
    localStorage.removeItem("Token");
}

/**
 * 写入userinfo
 * @param {any} info
 */
function setUserInfo(info) {
    var infoStr = JSON.stringify(info);
    localStorage["UserInfo"] = infoStr;
}

/**
 * 获取保存的userinfo
 * */
function getUserInfo() {
    var infoStr = localStorage["UserInfo"];

    var info = null;
    if (infoStr != undefined) {
        info = JSON.parse(infoStr);
    }

    return info;
}

/**
 * 清除userinfo
 */
function clearUserInfo() {
    localStorage.removeItem("UserInfo");
}

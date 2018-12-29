const _baseUrl = "http://localhost:8508/api/";
const _param = "auth";
/*
http://192.168.1.233:8085/
http://localhost:2180/
 */
/**
 * 获取存储对象
 * 
 * @returns 
 */
var getStorageObj = function () {
    // localStorage
    if (window.localStorage) {
        return window.localStorage;
    }
    return null;
}
/**
 * 获取票据
 * 
 * @returns 
 */
var getTicket = function () {
    let storage = getStorageObj();
    if (storage == null) {
        return null;
    }
    return storage.getItem(_param);
}
/**
 * 登出
 *  
 * @returns 
 */
var signOut = function () {
    let storage = getStorageObj();
    if (storage == null) {
        return null;
    }
    return storage.removeItem(_param);
    window.location = "login.html";
}
// ==UserScript==
// @name         保存圖片
// @namespace    http://tampermonkey.net/
// @version      1
// @description  try to take over the world!
// @author       You
// @match        https://twitter.com/*/status/*
// @icon         https://www.google.com/s2/favicons?sz=64&domain=twitter.com
// @grant        GM_xmlhttpRequest
// @run-at       context-menu
// ==/UserScript==

(function() {
    'use strict';
    var imgSrc = document.querySelector('img[alt="圖片"]').getAttribute("src");
    var time = document.querySelector("time").getAttribute("datetime");
    var tags = prompt("自定義標籤使用,隔開").split(",");
    if(tags!=null){
        GM_xmlhttpRequest({
            method: "post",
            url: "http://localhost:1263/api/photo",
            headers: {
                "Content-Type": "application/json"
            },
            data: JSON.stringify({
                "Address": location.href.split('?')[0],
                "Source": imgSrc.split('?')[0],
                "Date_8601": time,
                "Tag": tags
            }),
            onload: function(response){
                if(response.status === 200){
                    alert("Successed")
                }else{
                    alert("Failed")
                    console.log(response)
                }
            }
        });
    }
})();
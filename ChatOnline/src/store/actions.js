import * as types from './mutation-type'
/* 在这里引用api访问后端数据 */

export default {
    [types.SET_NOTICE_MAIN] ( commit ){
        return new Promise((resolve,reject) => {
            (function(flag){
                if(flag){ return flag }
                else { return !flag }
            })(true).then(res => {
                if(res){
                    commit(types.SET_NOTICE_MAIN,{ MainNoticeSwitch:true,MainNoticeContent:'今天天气不错' })
                }
            }).catch(()=>{
                reject('服务器请求失败');
            });
        });
    }
}
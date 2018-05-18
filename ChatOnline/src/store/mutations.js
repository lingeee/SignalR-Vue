import * as types from './mutation-type'

export default {
    [types.SET_NOTICE_MAIN](states,payload){
        states.MainNoticeSwitch = payload.MainNoticeSwitch;
        states.MainNoticeContent = payload.MainNoticeContent;
    }
}

'user strict'

var Base = {
    connectUrl : 'ws://101.132.147.117:7181/',
    copyRight : '',
    GetGuid : function () {  
        function S4() {  
           return (((1+Math.random())*0x10000)|0).toString(16).substring(1);  
        }  
        return (S4()+S4()+S4()+S4());  
    } 
}

export default Base;

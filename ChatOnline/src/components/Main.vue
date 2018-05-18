<template>
    <div>
        signalr connect
        <div>
            <div>{{showmsg}}</div>
            <input v-model="value" placeholder="请输入..." />
            <Button type="info" @click="sendMsg">信息按钮</Button>
        </div>
    </div>
    </template>
    <script>
    import $ from 'jquery'
    import signalR from '../assets/js/jquery.signalR-2.2.3'
    // import Hubs from '../signalr/hubs'
    export default {
    name: "Signalr",
    data() {
        return {
            value: "",
            showmsg: "222",
            proxy: {}
        }
    },
    mounted() {
        var $this = this;
        $this.connectServer();
    },
    methods: {
        connectServer() {
            var $this = this;
            var conn = $.hubConnection("http://localhost:12053", { name: "clientId=1232222"  })
            $this.proxy = conn.createHubProxy("chatRoomHub");
            $this.getMsg();
            conn.start().done((data) => {
                $this.sendMsg();
            }).fail((data) => {
                console.log('data=',data);
            });
        },
        sendMsg() {
            var $this = this;
            console.log('$this.value=',$this.value);
            
            $this.proxy.invoke("send", $this.value).done((msg) => {
                
            });
        },
        getMsg() {
            var $this = this;
            $this.proxy.on("clientMethod", (data) => {
                $this.showmsg = data;
            });
        }
    }
    }
    </script>

    <style>

    </style>


<template>
    <div>
        <h1>home</h1>
        <p>{{msg}}</p>
        <button @click="clickme">clickme</button>
    </div>
</template>
<script>
    export default {
        data () {
            return {
                msg: "我是home 组件"
            }
        },
        methods :{
            clickme (){
                
            }
        }
    }
</script>
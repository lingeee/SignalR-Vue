<template>
  <div id="app">
    <router-view></router-view> 
    <about></about>
    <home></home>
  </div>
</template>

<script>
import home from './components/home';
import about from './components/about';

export default {
  name: 'app',
  components:{
    home,
    about
  }
}
</script>


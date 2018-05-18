import Vue from 'vue';
import main from '@/components/Main';
import home from '@/components/home';
import about from '@/components/about';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

const  routes = [{
  path: '/main',
  component: main,
},{
  path: '/home',
  component: home,
},{
  path: '/about',
  component: about,
}, {
  path: '/',
  component: main,
}, {
  path: '',
  component: main,
}];

var router = new VueRouter({
  routes
})

export default router;

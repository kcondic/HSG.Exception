// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue';
import axios from 'axios';
import App from './App';
import router from './router';
import {API_URL} from './constants';

Vue.use(axios);
axios.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('token');
axios.interceptors.response.use(response => {
    return response;
  }, error => {
    let originalRequest = error.config;
    if(error.response && error.response.status === 401 && !originalRequest._retry && localStorage.getItem('token'))
    {
      originalRequest._retry = true;
      axios.post(API_URL + 'login/refresh', 
      {
        token: localStorage.getItem('token')
      }).then(response => {
        console.log(response);
        localStorage.setItem('token', response.data);
        axios(originalRequest);
      });
    }
    else if(error.response && error.response.status === 401)
    {
      localStorage.removeItem('token');
      window.location.href = '/';
    }
});


Vue.config.productionTip = false;
/* eslint-disable no-new */
new Vue({
  el: '#app', 
  router, 
  template: '<App/>',
  components: { App }
});
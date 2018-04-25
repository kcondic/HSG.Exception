// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue';
import axios from 'axios';
import App from './App';
import router from './router';
import {API_URL} from './constants';

Vue.use(axios);

axios.interceptors.request.use(config => {
    config.headers.authorization = `Bearer ${localStorage.getItem('token')}`;
    return config;
  },
  error => {
    Promise.reject(error);
});

axios.interceptors.response.use(response => {
    return response;
  }, error => {
    let originalRequest = error.config;
    if(error.response && error.response.status === 401 && !originalRequest._retry && localStorage.getItem('token'))
    {
      originalRequest._retry = true;
      return axios.post(API_URL + 'login/refresh', 
      {
        token: localStorage.getItem('token')
      }).then(response => {
        localStorage.setItem('token', response.data);
        return axios(originalRequest);
      });
    }
    else if(error.response && error.response.status === 403)
    {
      localStorage.removeItem('token');
      window.location.href = '/';
      return;
    }
    Promise.reject(error);
});
 

Vue.config.productionTip = false;
/* eslint-disable no-new */
new Vue({
  el: '#app', 
  router, 
  template: '<App/>',
  components: { App }
});
<template>
  <div>
    <form v-on:submit.prevent="handleLogin">
        <input type="text" v-model="userName" placeholder="Korisničko ime" />
        <input type="password" placeholder="Lozinka" v-model="password" />
        <button type="submit">Prijava</button>
    </form>
  </div>
</template>

<script>
import axios from 'axios';
import {API_URL} from '../constants'

export default {
  name: 'Home',
  data () {
    return {
      userName: '',
      password: ''
    }
  },
  methods: {
    handleLogin: function() {       
        axios.post(API_URL + 'login',
            {
                userName: this.userName,
                password: this.password
            })
            .then(response => {
              localStorage.setItem("token", response.data);
              this.$router.push('home');
            })
            .catch(error => {
                alert('Neispravni korisnički podaci.');
            });
    }
  }  
}
</script>
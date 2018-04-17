<template>
  <div>
    <form v-on:submit.prevent="handleSubmit">
      <button type="submit">Daj mi 5!</button>
    </form>
    <div>
      Ovo je broj: {{number}}
    </div>
  </div>
</template>

<script>
import axios from 'axios';
const API_URL = 'http://localhost:62517/api/';

export default {
  name: 'Home',
  data () {
    return {
      number: 1
    }
  },
  methods: {
    handleSubmit() {            
      axios.get(API_URL + 'test', {crossDomain: true})
            .then(response => {
              this.number = response.data;
            });
    }
  }  
}
</script>


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
import {API_URL} from '../constants';

export default {
  name: 'Home',
  data () {
    return {
      number: 1
    }
  },
  methods: {
    handleSubmit: function() {            
      axios.get(API_URL + 'test')
        .then(response => {
            this.number = response.data;
        });
    }
  }  
}
</script>


<template>
  <div class="login-container">
    <h2>Login</h2>
    <div v-if="errorMessage" class="error-message">
      {{ errorMessage }}
    </div>
    <form @submit.prevent="login">
      <div>
        <label for="username">Username:</label>
        <input type="text" id="username" v-model="username" required />
      </div>
      <div>
        <label for="password">Password:</label>
        <input
            type="password"
            id="password"
            v-model="password"
            required
            autocomplete="current-password"
        />
      </div>
      <button type="submit">Submit</button>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import apiClient from '@/axios';
import { useRouter } from 'vue-router';

const router = useRouter();

const username = ref('');
const password = ref('');
const errorMessage = ref('');

const login = async () => {
  try {
    const response = await apiClient.post('/api/Auth/login', {
      login: username.value,
      password: password.value,
    });
    localStorage.setItem('token', response.data.token);
    localStorage.setItem('sessionId', response.data.sessionId);
    localStorage.setItem('username', username.value);

    router.push('/welcome');
  } catch (error) {
    errorMessage.value =
        'We could not log you in. Please check your username/password and try again.';

    password.value = '';
  }
};
</script>

<style scoped>
.login-container {
  min-width: 400px;
  margin: 0 auto;
}
.error-message {
  color: red;
  margin-bottom: 10px;
}
</style>

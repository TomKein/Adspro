<template>
  <div class="welcome-container">
    <h2>Welcome, {{ username }}! This page is for authenticated users only.</h2>
    <p>Your session: {{sessionId}}</p>
    <button @click="logout">Logout</button>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import apiClient from "@/axios.js";

const router = useRouter();
const username = ref('');
const token = ref('');
const sessionId = ref('');

onMounted(async () => {
  try {
    const res = await apiClient.get('/api/privateData');
    username.value = localStorage.getItem('username') || '';
    sessionId.value = localStorage.getItem('sessionId') || '';
  }
  catch (error) {
    router.push('/login');
  }
});

const logout = async () => {
  await apiClient.post('/api/Auth/logout', {
    username: username.value,
    sessions: [sessionId.value],
  });
  localStorage.removeItem('token');
  localStorage.removeItem('sessionId');
  localStorage.removeItem('username');
  router.push('/login');
};
</script>

<style scoped>
.welcome-container {
  min-width: 1024px;
  text-align: center;
  margin-top: 50px;
}
</style>

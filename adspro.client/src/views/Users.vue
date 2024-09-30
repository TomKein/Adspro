<template>
  <div class="users-container">
    <h1>User Administration</h1>
    <table>
      <thead>
      <tr>
        <th>Username</th>
        <th>Active</th>
      </tr>
      </thead>
      <tbody>
      <tr v-for="user in users" :key="user.id">
        <td @click="toggleUserSessions(user)">
          {{ user.login }}
        </td>
        <td>{{ user.sessions.length > 0 ? 'Yes' : 'No' }}</td>
      </tr>
      </tbody>
    </table>

    <!-- User Sessions Panel -->
    <div v-if="selectedUser" class="sessions-panel">
      <div v-if="selectedUser.sessions.length > 0">
        <h2>Sessions for {{ selectedUser.login }}</h2>
        <div>
          <label v-for="session in selectedUser.sessions" :key="session.id">
            <input
                type="checkbox"
                :value="session.id"
                v-model="selectedSessions"
            />
            Session ID: {{ session.id }}, Expires At: {{ session.expiresAt }}
          </label>
        </div>
        <button @click="logout(selectedUser.login, selectedSessions)">Logout Selected</button>
        <button @click="logout(selectedUser.login, null)">Logout on All Devices</button>
      </div>
      <div v-else>
        <h2>User: {{selectedUser.login}} not logined</h2>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import apiClient from '@/axios';

const users = ref([]);
const selectedUser = ref(null);
const selectedSessions = ref([]);
const curUser = localStorage.getItem('username');
const curSession = localStorage.getItem('sessionId');

const fetchUsers = async () => {
  const response = await apiClient.get('/api/users');
  users.value = response.data;
};

const toggleUserSessions = (user) => {
  if (selectedUser.value && selectedUser.value.id === user.id) {
    selectedUser.value = null;
    selectedSessions.value = [];
  } else {
    selectedUser.value = user;
    selectedSessions.value = [];
  }
};

const logout = async (username, sessions) => {
  await apiClient.post('/api/Auth/logout', {
    username: username,
    sessions: sessions,
  });
  await fetchUsers();
  selectedUser.value = null;
  
  if (curUser === username && !sessions || sessions.includes(curSession) ) {
    localStorage.removeItem('token');
    localStorage.removeItem('sessionId');
    localStorage.removeItem('username');
  }
};

fetchUsers();
</script>

<style scoped>
.users-container {
  min-width: 1024px;
  margin: 0 auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead th {
  text-align: left;
  border-bottom: 2px solid #ccc;
}

tbody tr:hover {
  background-color: #a5a5a5;
  cursor: pointer;
}

.sessions-panel {
  margin-top: 20px;
}

.sessions-panel h3 {
  margin-bottom: 10px;
}

.sessions-panel label {
  display: block;
  margin-bottom: 5px;
}
</style>

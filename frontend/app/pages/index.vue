<template>
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <div class="w-full max-w-md p-8 bg-white rounded-xl shadow-md">
      <h2 class="text-2xl font-bold text-center mb-6">Log In</h2>
      <form @submit.prevent="login">

        <div class="mb-4">
          <label class="block text-gray-700 mb-1" for="username">Username</label>
          <input
            v-model="form.username"
            id="username"
            type="text"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 mb-1" for="password">Password</label>
          <input
            v-model="form.password"
            id="password"
            type="password"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        <button
          type="submit"
          class="w-full bg-yellow-500 text-white font-bold text-2xl py-2 px-4 rounded-md hover:bg-yellow-600 transition"
        >
          Log In
        </button>
        </form>
        <div class="border-gray-300 border m-5"></div>
        <button
            type="submit"
            class="w-full bg-purple-500 text-white font-bold text-2xl py-2 px-4 rounded-md hover:bg-purple-600 transition"
        >
            <NuxtLink to="/register">
                Create an Account
            </NuxtLink>
        </button>
    </div>
  </div>
</template>

<script setup>
import { reactive } from 'vue';
import { NuxtLink } from '#components';

const config = useRuntimeConfig();

const form = reactive({
  username: '',
  password: ''
});

async function login() {
  try {
    const response = await $fetch(`${config.public.apiBase}/auth/login`, {
      method: 'POST',
      body: {
        username: form.username,
        password: form.password
      }
    });

    // Store token
    localStorage.setItem('token', response.token);

    alert('Login successful!');
    navigateTo('/noteslist'); // Adjust route as needed
  } catch (err) {
    alert('Login failed.');
    console.error(err);
  }
}
</script>


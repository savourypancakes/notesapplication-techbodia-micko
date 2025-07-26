<template>
  <div class="flex flex-col items-center justify-start min-h-screen bg-gray-100 py-10">
    <!-- FORM -->
    <div class="w-full max-w-md p-8 bg-white rounded-xl shadow-md mb-10">
      <h2 class="text-2xl font-bold text-center mb-6">Post Note</h2>
      <form @submit.prevent="post">
        <div class="mb-4">
          <label class="block text-gray-700 mb-1" for="title">Title</label>
          <input
            v-model="form.title"
            id="title"
            type="text"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 mb-1" for="content">Content</label>
          <input
            v-model="form.content"
            id="content"
            type="text"
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        <button
          type="submit"
          class="w-full bg-yellow-500 text-white font-bold text-2xl py-2 px-4 rounded-md hover:bg-yellow-600 transition"
        >
          Post
        </button>
      </form>
    </div>

    <!-- NOTE LIST -->
    <div class="w-full max-w-2xl bg-white p-6 rounded-xl shadow-md">
      <h2 class="text-xl font-bold mb-4">Your Notes</h2>
      <ul v-if="notes.length">
        <li v-for="note in notes" :key="note.id" class="mb-4 p-4 border border-gray-200 rounded-md">
          <h3 class="text-lg font-semibold">{{ note.title }}</h3>
          <p class="text-xs text-gray-400 mt-1">Created at: {{ formatDate(note.createdOn) }}</p>
        </li>
      </ul>
      <p v-else class="text-gray-500">No notes found.</p>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue';
const config = useRuntimeConfig();

const form = ref({
  title: '',
  content: ''
});

const notes = ref([]);

// Dynamically read the token (important after refresh)
function getToken() {
  return localStorage.getItem('token');
}

// Format date for display
function formatDate(dateString) {
  return new Date(dateString).toLocaleString();
}

// Fetch notes from API
async function fetchNotes() {
  const token = getToken();
  if (!token) return alert('Please log in first.');

  try {
    const data = await $fetch(`${config.public.apiBase}/Note`, {
      method: 'GET',
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
    notes.value = data;
  } catch (err) {
    console.error('Failed to fetch notes:', err);
  }
}

// Post new note to API
async function post() {
  const token = getToken();
  if (!token) return alert('Please log in first.');

  try {
    await $fetch(`${config.public.apiBase}/Note`, {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${token}`
      },
      body: {
        noteTitle: form.value.title,
        noteContent: form.value.content
      }
    });

    alert('Note posted!');
    form.value.title = '';
    form.value.content = '';
    await fetchNotes(); // Refresh notes after post
  } catch (err) {
    alert('Failed to post note.');
    console.error(err);
  }
}

// Load notes on page mount
onMounted(fetchNotes);
</script>

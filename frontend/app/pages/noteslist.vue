<template>
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <div class="w-full max-w-md p-8 bg-white rounded-xl shadow-md">
      <h2 class="text-2xl font-bold text-center mb-6">Post Note</h2>
      <form @submit.prevent="post">

        <div class="mb-4">
          <label class="block text-gray-700 mb-1" for="username">Title</label>
          <input
            v-model="form.title"
            id="title"
            type="text"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        <div class="mb-4">
          <label class="block text-gray-700 mb-1" for="password">Content</label>
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
  </div>
</template>

<script setup>

const config = useRuntimeConfig();

const form = ref({
  noteTitle: '',
  noteContent: ''
});

async function post() {
  const token = localStorage.getItem('token'); // or use useCookie() if stored as cookie

  try {
    const response = await $fetch(`${config.public.apiBase}/Note`, {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${token}`
      },
      body: {
        title: form.value.title,
        content: form.value.content
      }
    });

    alert('Note posted successfully!');
    form.value.title = '';
    form.value.content = '';
  } catch (err) {
    alert('Note post failed.');
    console.error(err);
  }
}

</script>

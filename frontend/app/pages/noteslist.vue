<template>
  <div class="min-h-screen flex flex-col">
    <header class = "w-full shadow-lg mb-1 p-4 flex justify-end rounded-md border-b-5 border-purple-600">
      <button
        class=" flex gap-4 border-1 border-purple-300 text-purple-500 px-4 py-2 rounded-md hover:border-purple-600 hover:text-purple-900 hover:bg-purple-100"
        @click="logout"
        
      >
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
          <path stroke-linecap="round" stroke-linejoin="round" d="M8.25 9V5.25A2.25 2.25 0 0 1 10.5 3h6a2.25 2.25 0 0 1 2.25 2.25v13.5A2.25 2.25 0 0 1 16.5 21h-6a2.25 2.25 0 0 1-2.25-2.25V15M12 9l3 3m0 0-3 3m3-3H2.25" />
        </svg>
        Logout
      </button>
    </header>
    <main class="flex-1 flex items-start justify-center bg-gray-100 gap-10 py-10">
        <div class="w-full max-w-md p-8 bg-white rounded-xl shadow-md mb-10">
          <div class="text-2xl font-bold text-center mb-6 bg-purple-600 text-white p-4 rounded-t-md">Post Note</div>
          <form @submit.prevent="post">
            <div class="mb-4">
              <label class="block text-gray-700 mb-1" for="title">Title</label>
              <input
                v-model="form.title"
                id="title"
                type="text"
                required
                class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-purple-300"
              />
            </div>

            <div class="mb-4">
              <label class="block text-gray-700 mb-1" for="content">Content</label>
              <input
                v-model="form.content"
                id="content"
                type="text"
                class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-purple-300"
              />
            </div>

            <button
              type="submit"
              class="w-full bg-purple-400 text-white font-bold text-2xl py-2 px-4 rounded-md hover:bg-purple-600 transition"
            >
              Post
            </button>
          </form>
        </div>

        <div class="w-full max-w-2xl bg-white p-6 rounded-xl shadow-md">
          <h2 class="text-xl font-bold mb-4 bg-purple-600 text-white p-4 rounded-t-md">Your Notes</h2>
          <div class = "flex">
            <input
              v-model="searchQuery"
              type="text"
              placeholder="Search by title"
              class="w-full p-2 border rounded mb-4 border-neutral-300"
            />
            <select v-model="sortOption" class="p-2 border rounded mb-4 ml-2 border-neutral-300 text-neutral-500">
              <option value="newest">Newest First</option>
              <option value="oldest">Oldest First</option>
              <option value="a-z">Title A-Z</option>
              <option value="z-a">Title Z-A</option>
            </select>
          </div>
          <ul v-if="filteredNotes.length">
            <li
              v-for="note in filteredNotes"
              :key="note.noteID"
              class="mb-4 p-4 border border-gray-200 rounded-md"
            >
              <div v-if="editingNoteId === note.noteID">
                <input
                  v-model="editForm.title"
                  class="w-full mb-2 px-3 py-2 border rounded"
                  placeholder="Title"
                />
                <textarea
                  v-model="editForm.content"
                  class="w-full mb-2 px-3 py-2 border rounded"
                  placeholder="Content"
                ></textarea>

                <div class="flex gap-2">
                  <button @click="saveEdit(note.noteID)" class="bg-green-500 text-white px-4 py-1 rounded hover:bg-green-600">
                    Save
                  </button>
                  <button @click="cancelEdit" class="bg-gray-300 px-4 py-1 rounded hover:bg-gray-400">
                    Cancel
                  </button>
                </div>
              </div>

              <div v-else @click="note.content && toggleNote(note.noteID)" class="cursor-pointer">
                <div class="flex justify-between items-center">
                  <div class="flex text-lg items-center gap-1 font-semibold text-purple-600">
                    {{ note.title }}
                    <span v-if="note.content" class="hover:bg-purple-100 p-1 rounded-full">
                      <svg v-if = "!isExpanded(note.noteID)" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4">
                        <path stroke-linecap="round" stroke-linejoin="round" d="m8.25 4.5 7.5 7.5-7.5 7.5" />
                      </svg>
                      <svg v-else xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4">
                        <path stroke-linecap="round" stroke-linejoin="round" d="m19.5 8.25-7.5 7.5-7.5-7.5" />
                      </svg>

                    </span>
                  </div>
                  <div class="flex">
                    <button
                      @click.stop="startEditing(note)"
                      class="px-3 py-3 rounded-xl text-yellow-600 hover:bg-yellow-100"
                    >
                      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4">
                        <path stroke-linecap="round" stroke-linejoin="round" d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10" />
                      </svg>
                    </button>
                    <button
                      @click="deleteNote(note.noteID)"
                      class="px-3 py-3 rounded-xl text-red-600 hover:bg-red-100"
                    >
                      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4">
                        <path stroke-linecap="round" stroke-linejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
                      </svg>
                    </button>
                  </div>
                </div>

                <p v-if="note.content && isExpanded(note.noteID)" class="text-gray-600 mt-2">
                  {{ note.content }}
                </p>
              </div>

              <p class="text-xs text-gray-400 mt-1">
                Created: {{ formatDate(note.createdOn) }}
              </p>
              <p v-if="note.updatedOn" class="text-xs text-gray-400">
                Updated: {{ formatDate(note.updatedOn) }}
              </p>
            </li>
          </ul>
          <p v-else class="text-gray-500">No notes found.</p>
        </div>
    </main>
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
const expandedNotes = ref(new Set());
const editingNoteId = ref(null);
const editForm = ref({
  noteTitle: '',
  noteContent: ''
});

const searchQuery = ref('');
const sortOption = ref('newest');

const filteredNotes = computed(() => {
  let result = [...notes.value];

  // 1. Search
  if (searchQuery.value.trim()) {
    const q = searchQuery.value.trim().toLowerCase();
    result = result.filter(note =>
      note.title.toLowerCase().includes(q)
    );
  }

  // 2. Sort
  switch (sortOption.value) {
    case 'newest':
      result.sort((a, b) => new Date(b.createdOn) - new Date(a.createdOn));
      break;
    case 'oldest':
      result.sort((a, b) => new Date(a.createdOn) - new Date(b.createdOn));
      break;
    case 'a-z':
      result.sort((a, b) => a.title.localeCompare(b.title));
      break;
    case 'z-a':
      result.sort((a, b) => b.title.localeCompare(a.title));
      break;
  }


  return result;
});

const logout = () => {
  localStorage.removeItem('token');
  window.location.href = '/';
};


function startEditing(note) {
  editingNoteId.value = note.noteID;
  editForm.value.title = note.title;
  editForm.value.content = note.content;
}

function cancelEdit() {
  editingNoteId.value = null;
}

function toggleNote(id) {
  if (expandedNotes.value.has(id)) {
    expandedNotes.value.delete(id);
  } else {
    expandedNotes.value.add(id);
  }
}

function isExpanded(id) {
  return expandedNotes.value.has(id);
}

function getToken() {
  return localStorage.getItem('token');
}


function formatDate(dateString) {
  return new Date(dateString).toLocaleString();
}



async function deleteNote(id) {
  const token = getToken();
  if (!confirm('Are you sure you want to delete this note?')) return;

  try {
    await $fetch(`${config.public.apiBase}/Note/${id}`, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${token}`
      }
    });

    // Refresh the notes list
    await fetchNotes();
  } catch (err) {
    console.error('Failed to delete note:', err);
  }
}



async function saveEdit(noteID) {
  const token = getToken();
  try {
    await $fetch(`${config.public.apiBase}/Note`, {
      method: 'PUT',
      headers: {
        Authorization: `Bearer ${token}`
      },
      body: {
        noteID: noteID,
        noteTitle: editForm.value.title,
        noteContent: editForm.value.content
      }
    });

    editingNoteId.value = null;
    await fetchNotes(); // Refresh list
  } catch (err) {
    console.error(err);
  }
}



// Fetch notes from API
async function fetchNotes() {
  const token = getToken();

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

    form.value.title = '';
    form.value.content = '';
    await fetchNotes(); // Refresh notes after post
  } catch (err) {
    console.error(err);
  }
}

// Load notes on page mount
onMounted(fetchNotes);
</script>

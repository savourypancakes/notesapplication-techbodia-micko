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

    <div class="w-full max-w-2xl bg-white p-6 rounded-xl shadow-md">
      <h2 class="text-xl font-bold mb-4">Your Notes</h2>
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Search by title"
        class="w-full p-2 border rounded mb-4"
      />
      <select v-model="sortOption" class="p-2 border rounded mb-4 ml-2">
        <option value="newest">Newest First</option>
        <option value="oldest">Oldest First</option>
        <option value="a-z">Title A-Z</option>
        <option value="z-a">Title Z-A</option>
      </select>
      <ul v-if="filteredNotes.length">
        <li
          v-for="note in filteredNotes"
          :key="note.noteID"
          class="mb-4 p-4 border border-gray-200 rounded-md"
        >
          <!-- IF EDITING THIS NOTE -->
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
              <h3 class="text-lg font-semibold text-blue-600">
                {{ note.title }}
                <span v-if="note.content" class="ml-2 text-sm">
                  {{ isExpanded(note.noteID) ? '▼' : '►' }}
                </span>
              </h3>
              <button
                @click.stop="startEditing(note)"
                class="text-sm text-yellow-600 hover:underline"
              >
                Edit
              </button>
            </div>

            <p v-if="note.content && isExpanded(note.noteID)" class="text-gray-600 mt-2">
              {{ note.content }}
            </p>
          </div>

          <p class="text-xs text-gray-400 mt-1">
            Created: {{ formatDate(note.createdOn) }}
          </p>
          <p class="text-xs text-gray-400">
            Updated: {{ formatDate(note.updatedOn) }}
          </p>
          <div class="flex justify-end mt-2">
            <button
              @click="deleteNote(note.noteID)"
              class="text-red-500 hover:underline text-sm"
            >
              Delete
            </button>
          </div>
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


// Dynamically read the token (important after refresh)
function getToken() {
  return localStorage.getItem('token');
}

// Format date for display
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
    alert('Failed to delete the note.');
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
    alert('Note updated!');
  } catch (err) {
    console.error(err);
    alert('Failed to update note.');
  }
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

import { ref, computed, mergeProps, unref, useSSRContext } from 'vue';
import { ssrRenderAttrs, ssrRenderAttr, ssrIncludeBooleanAttr, ssrLooseContain, ssrLooseEqual, ssrRenderList, ssrInterpolate } from 'vue/server-renderer';

const _sfc_main = {
  __name: "noteslist",
  __ssrInlineRender: true,
  setup(__props) {
    const form = ref({
      title: "",
      content: ""
    });
    const notes = ref([]);
    const expandedNotes = ref(/* @__PURE__ */ new Set());
    const editingNoteId = ref(null);
    const editForm = ref({
      noteTitle: "",
      noteContent: ""
    });
    const searchQuery = ref("");
    const sortOption = ref("newest");
    const filteredNotes = computed(() => {
      let result = [...notes.value];
      if (searchQuery.value.trim()) {
        const q = searchQuery.value.trim().toLowerCase();
        result = result.filter(
          (note) => note.title.toLowerCase().includes(q)
        );
      }
      switch (sortOption.value) {
        case "newest":
          result.sort((a, b) => new Date(b.createdOn) - new Date(a.createdOn));
          break;
        case "oldest":
          result.sort((a, b) => new Date(a.createdOn) - new Date(b.createdOn));
          break;
        case "a-z":
          result.sort((a, b) => a.title.localeCompare(b.title));
          break;
        case "z-a":
          result.sort((a, b) => b.title.localeCompare(a.title));
          break;
      }
      return result;
    });
    function isExpanded(id) {
      return expandedNotes.value.has(id);
    }
    function formatDate(dateString) {
      return new Date(dateString).toLocaleString();
    }
    return (_ctx, _push, _parent, _attrs) => {
      _push(`<div${ssrRenderAttrs(mergeProps({ class: "min-h-screen flex flex-col" }, _attrs))}><header class="w-full shadow-lg mb-1 p-4 flex justify-end rounded-md border-b-5 border-purple-600"><button class="flex gap-4 border-1 border-purple-300 text-purple-500 px-4 py-2 rounded-md hover:border-purple-600 hover:text-purple-900 hover:bg-purple-100"><svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6"><path stroke-linecap="round" stroke-linejoin="round" d="M8.25 9V5.25A2.25 2.25 0 0 1 10.5 3h6a2.25 2.25 0 0 1 2.25 2.25v13.5A2.25 2.25 0 0 1 16.5 21h-6a2.25 2.25 0 0 1-2.25-2.25V15M12 9l3 3m0 0-3 3m3-3H2.25"></path></svg> Logout </button></header><main class="flex-1 flex items-start justify-center bg-gray-100 gap-10 py-10"><div class="w-full max-w-md p-8 bg-white rounded-xl shadow-md mb-10"><div class="text-2xl font-bold text-center mb-6 bg-purple-600 text-white p-4 rounded-t-md">Post Note</div><form><div class="mb-4"><label class="block text-gray-700 mb-1" for="title">Title</label><input${ssrRenderAttr("value", form.value.title)} id="title" type="text" required class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-purple-300"></div><div class="mb-4"><label class="block text-gray-700 mb-1" for="content">Content</label><input${ssrRenderAttr("value", form.value.content)} id="content" type="text" class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-purple-300"></div><button type="submit" class="w-full bg-purple-400 text-white font-bold text-2xl py-2 px-4 rounded-md hover:bg-purple-600 transition"> Post </button></form></div><div class="w-full max-w-2xl bg-white p-6 rounded-xl shadow-md"><h2 class="text-xl font-bold mb-4 bg-purple-600 text-white p-4 rounded-t-md">Your Notes</h2><div class="flex"><input${ssrRenderAttr("value", searchQuery.value)} type="text" placeholder="Search by title" class="w-full p-2 border rounded mb-4 border-neutral-300"><select class="p-2 border rounded mb-4 ml-2 border-neutral-300 text-neutral-500"><option value="newest"${ssrIncludeBooleanAttr(Array.isArray(sortOption.value) ? ssrLooseContain(sortOption.value, "newest") : ssrLooseEqual(sortOption.value, "newest")) ? " selected" : ""}>Newest First</option><option value="oldest"${ssrIncludeBooleanAttr(Array.isArray(sortOption.value) ? ssrLooseContain(sortOption.value, "oldest") : ssrLooseEqual(sortOption.value, "oldest")) ? " selected" : ""}>Oldest First</option><option value="a-z"${ssrIncludeBooleanAttr(Array.isArray(sortOption.value) ? ssrLooseContain(sortOption.value, "a-z") : ssrLooseEqual(sortOption.value, "a-z")) ? " selected" : ""}>Title A-Z</option><option value="z-a"${ssrIncludeBooleanAttr(Array.isArray(sortOption.value) ? ssrLooseContain(sortOption.value, "z-a") : ssrLooseEqual(sortOption.value, "z-a")) ? " selected" : ""}>Title Z-A</option></select></div>`);
      if (unref(filteredNotes).length) {
        _push(`<ul><!--[-->`);
        ssrRenderList(unref(filteredNotes), (note) => {
          _push(`<li class="mb-4 p-4 border border-gray-200 rounded-md">`);
          if (editingNoteId.value === note.noteID) {
            _push(`<div><input${ssrRenderAttr("value", editForm.value.title)} class="w-full mb-2 px-3 py-2 border rounded" placeholder="Title"><textarea class="w-full mb-2 px-3 py-2 border rounded" placeholder="Content">${ssrInterpolate(editForm.value.content)}</textarea><div class="flex gap-2"><button class="bg-green-500 text-white px-4 py-1 rounded hover:bg-green-600"> Save </button><button class="bg-gray-300 px-4 py-1 rounded hover:bg-gray-400"> Cancel </button></div></div>`);
          } else {
            _push(`<div class="cursor-pointer"><div class="flex justify-between items-center"><div class="flex text-lg items-center gap-1 font-semibold text-purple-600">${ssrInterpolate(note.title)} `);
            if (note.content) {
              _push(`<span class="hover:bg-purple-100 p-1 rounded-full">`);
              if (!isExpanded(note.noteID)) {
                _push(`<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4"><path stroke-linecap="round" stroke-linejoin="round" d="m8.25 4.5 7.5 7.5-7.5 7.5"></path></svg>`);
              } else {
                _push(`<svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4"><path stroke-linecap="round" stroke-linejoin="round" d="m19.5 8.25-7.5 7.5-7.5-7.5"></path></svg>`);
              }
              _push(`</span>`);
            } else {
              _push(`<!---->`);
            }
            _push(`</div><div class="flex"><button class="px-3 py-3 rounded-xl text-yellow-600 hover:bg-yellow-100"><svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4"><path stroke-linecap="round" stroke-linejoin="round" d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10"></path></svg></button><button class="px-3 py-3 rounded-xl text-red-600 hover:bg-red-100"><svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4"><path stroke-linecap="round" stroke-linejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0"></path></svg></button></div></div>`);
            if (note.content && isExpanded(note.noteID)) {
              _push(`<p class="text-gray-600 mt-2">${ssrInterpolate(note.content)}</p>`);
            } else {
              _push(`<!---->`);
            }
            _push(`</div>`);
          }
          _push(`<p class="text-xs text-gray-400 mt-1"> Created: ${ssrInterpolate(formatDate(note.createdOn))}</p>`);
          if (note.updatedOn) {
            _push(`<p class="text-xs text-gray-400"> Updated: ${ssrInterpolate(formatDate(note.updatedOn))}</p>`);
          } else {
            _push(`<!---->`);
          }
          _push(`</li>`);
        });
        _push(`<!--]--></ul>`);
      } else {
        _push(`<p class="text-gray-500">No notes found.</p>`);
      }
      _push(`</div></main></div>`);
    };
  }
};
const _sfc_setup = _sfc_main.setup;
_sfc_main.setup = (props, ctx) => {
  const ssrContext = useSSRContext();
  (ssrContext.modules || (ssrContext.modules = /* @__PURE__ */ new Set())).add("pages/noteslist.vue");
  return _sfc_setup ? _sfc_setup(props, ctx) : void 0;
};

export { _sfc_main as default };
//# sourceMappingURL=noteslist-DMQiCaPU.mjs.map

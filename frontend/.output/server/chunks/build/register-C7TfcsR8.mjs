import { ref, mergeProps, unref, withCtx, createTextVNode, useSSRContext } from 'vue';
import { ssrRenderAttrs, ssrRenderAttr, ssrRenderComponent } from 'vue/server-renderer';
import { _ as __nuxt_component_0 } from './nuxt-link-DG8WII_W.mjs';
import '../_/nitro.mjs';
import 'node:http';
import 'node:https';
import 'node:events';
import 'node:buffer';
import 'node:fs';
import 'node:url';
import '@iconify/utils';
import 'node:crypto';
import 'consola';
import 'node:path';
import 'better-sqlite3';
import 'ipx';
import './server.mjs';
import 'vue-router';
import 'tailwindcss/colors';
import '@iconify/vue';
import '../routes/renderer.mjs';
import 'vue-bundle-renderer/runtime';
import 'unhead/server';
import 'devalue';
import 'unhead/utils';

const _sfc_main = {
  __name: "register",
  __ssrInlineRender: true,
  setup(__props) {
    const form = ref({
      username: "",
      password: ""
    });
    return (_ctx, _push, _parent, _attrs) => {
      _push(`<div${ssrRenderAttrs(mergeProps({ class: "flex items-center justify-center min-h-screen bg-gray-100" }, _attrs))}><div class="w-full max-w-md p-8 bg-white rounded-xl shadow-md"><h2 class="text-2xl font-bold text-center mb-6">Register</h2><form><div class="mb-4"><label class="block text-gray-700 mb-1" for="username">Username</label><input${ssrRenderAttr("value", unref(form).username)} id="username" type="text" required class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"></div><div class="mb-4"><label class="block text-gray-700 mb-1" for="password">Password</label><input${ssrRenderAttr("value", unref(form).password)} id="password" type="password" required class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"></div><button type="submit" class="w-full bg-yellow-500 text-white font-bold text-2xl py-2 px-4 rounded-md hover:bg-yellow-600 transition"> Register </button></form><div class="border-gray-300 border m-5"></div><button type="submit" class="w-full bg-purple-500 text-white font-bold text-2xl py-2 px-4 rounded-md hover:bg-purple-600 transition">`);
      _push(ssrRenderComponent(unref(__nuxt_component_0), { to: "/" }, {
        default: withCtx((_, _push2, _parent2, _scopeId) => {
          if (_push2) {
            _push2(` Already have an account? `);
          } else {
            return [
              createTextVNode(" Already have an account? ")
            ];
          }
        }),
        _: 1
      }, _parent));
      _push(`</button></div></div>`);
    };
  }
};
const _sfc_setup = _sfc_main.setup;
_sfc_main.setup = (props, ctx) => {
  const ssrContext = useSSRContext();
  (ssrContext.modules || (ssrContext.modules = /* @__PURE__ */ new Set())).add("pages/register.vue");
  return _sfc_setup ? _sfc_setup(props, ctx) : void 0;
};

export { _sfc_main as default };
//# sourceMappingURL=register-C7TfcsR8.mjs.map

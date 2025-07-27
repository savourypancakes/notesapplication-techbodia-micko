import tailwindcss from "@tailwindcss/vite";

export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  css: ['~/assets/css/main.css'],
  app: {
    baseURL: process.env.NODE_ENV === 'production' ? '/notesapplication-techbodia-micko/' : '/',
  },
  vite: {
    base: process.env.NODE_ENV === 'production' ? '/notesapplication-techbodia-micko/' : '/',
    plugins: [
      tailwindcss(),
    ],
  },
  runtimeConfig: {
    public: {
      apiBase: 'https://notesapplication-techbodia-micko.onrender.com/api'  // remove double slash
    }
  },
  modules: [
    '@nuxt/content',
    '@nuxt/eslint',
    '@nuxt/image',
    '@nuxt/scripts',
    '@nuxt/test-utils',
    '@nuxt/ui'
  ]
})

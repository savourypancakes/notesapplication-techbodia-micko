import _RemarkEmoji from 'remark-emoji'
import _Highlight from 'C:/Users/kokmi/Documents/GitHub/notesapplication-techbodia-micko/frontend/node_modules/@nuxtjs/mdc/dist/runtime/highlighter/rehype-nuxt.js'

export const remarkPlugins = {
  'remark-emoji': { instance: _RemarkEmoji },
}

export const rehypePlugins = {
  'highlight': { instance: _Highlight, options: {} },
}

export const highlight = {"theme":{"default":"github-light","dark":"github-dark"}}
import { defineConfig } from 'vitepress'

// https://vitepress.dev/reference/site-config
export default defineConfig({
  title: "BlazeFusion",
  description: "Create .NET apps with SPA feeling without JS",
  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Getting Started', link: '/introduction/getting-started'}
    ],

    sidebar: [
      {
        text: 'Introduction',
        items: [
          { text: 'Overview', link: '/introduction/overview' },
          { text: 'Motivation', link: '/introduction/motivation' },
          { text: 'Getting Started', link: '/introduction/getting-started' }
        ]
      },
      {
        text: 'Features',
        items: [
          { text: 'Components', link: '/features/components' },
          { text: 'Parameters', link: '/features/parameters' },
          { text: 'Binding', link: '/features/binding' },
          { text: 'Actions', link: '/features/actions' },
          { text: 'Events', link: '/features/events' },
          { text: 'Navigation', link: '/features/navigation' },
          { text: 'Authorization', link: '/features/authorization' },
          { text: 'Form Validation', link: '/features/form-validation' },
          { text: 'Cookies', link: '/features/cookies' },
          { text: 'Long Polling', link: '/features/long-polling' },
          { text: 'Errors Handling', link: '/features/errors-handling' },
          { text: 'Anti-forgery Token', link: '/features/xsrf-token' },
          { text: 'User Interface Utilities', link: '/features/ui-utils' },
          { text: 'Using JavaScript', link: '/features/js' },
        ]
      },
      {
        text: 'Advanced',
        items: [
          { text: 'Request Queuing', link: '/advanced/request-queuing' },
          // { text: 'Load balancing', link: '/advanced/load-balancing' },
        ]
      },
      {
        text: 'Utilities',
        items: [
          { text: 'BlazeFusion Views', link: '/utilities/blaze-views' },
        ]
      },
      {
        text: 'Examples',
        items: [
          { text: 'Apps', link: '/examples/apps' },
        ]
      }
    ],

    socialLinks: [
      { icon: 'github', link: 'https://github.com/vuejs/vitepress' }
    ]
  }
})

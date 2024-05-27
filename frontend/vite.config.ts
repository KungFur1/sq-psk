import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
    server: {
      proxy: {
        '/api/recipe': 'http://localhost:7001',

        '/api/search': 'http://localhost:7002',

        '/api/auth': 'http://localhost:7003',
        '/api/users': 'http://localhost:7003',

        '/api/images': 'http://localhost:7004',
      },
    },
})

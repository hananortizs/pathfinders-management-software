import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  build: {
    rollupOptions: {
      output: {
        manualChunks: {
          // Separar bibliotecas grandes em chunks próprios
          vendor: ['react', 'react-dom'],
          mui: ['@mui/material', '@mui/icons-material'],
          router: ['react-router-dom'],
          forms: ['react-hook-form', '@hookform/resolvers'],
          query: ['@tanstack/react-query'],
          store: ['zustand'],
        },
      },
    },
    // Aumentar limite de aviso para 600kB
    chunkSizeWarningLimit: 600,
  },
  server: {
    port: 5173,
    host: true,
  },
})

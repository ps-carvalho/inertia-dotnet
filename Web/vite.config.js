import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { resolve } from 'path';

export default defineConfig({
  plugins: [react()],
  build: {
    manifest: true,
    outDir: resolve(__dirname, 'wwwroot/dist'),
    emptyOutDir: true,
    rollupOptions: {
      input: resolve(__dirname, 'src/app.jsx'),
    },
  },
  server: {
    port: 5173,
    strictPort: true,
    cors: true,
  },
});

import { createInertiaApp } from '@inertiajs/react';
import { createRoot } from 'react-dom/client';
import './app.css';

const pages = import.meta.glob('./pages/**/*.jsx', { eager: true });

const resolve = (name) => {
  const path = `./pages/${name}.jsx`;
  return pages[path]?.default || pages[`./pages/${name}/Index.jsx`]?.default;
};

createInertiaApp({
  resolve,
  setup({ el, App, props }) {
    const root = createRoot(el);
    root.render(<App {...props} />);
  },
});

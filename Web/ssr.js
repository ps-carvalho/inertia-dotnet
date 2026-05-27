import { createInertiaApp } from '@inertiajs/react';
import createServer from '@inertiajs/react/server';
import ReactDOMServer from 'react-dom/server';

const pages = import.meta.glob('./wwwroot/js/pages/**/*.jsx', { eager: true });

const resolve = (name) => {
  const path = `./wwwroot/js/pages/${name}.jsx`;
  return pages[path]?.default;
};

createServer((page) =>
  createInertiaApp({
    page,
    resolve,
    render: ReactDOMServer.renderToString,
    setup({ App, props }) {
      return <App {...props} />;
    },
  })
);

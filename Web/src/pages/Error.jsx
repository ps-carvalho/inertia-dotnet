import React from 'react';

export default function Error({ status, message }) {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="text-center">
        <h1 className="text-6xl font-bold text-gray-900 mb-4">{status || 500}</h1>
        <p className="text-xl text-gray-600">{message || 'Something went wrong.'}</p>
      </div>
    </div>
  );
}

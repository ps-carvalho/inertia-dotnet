import React from 'react';

export default function Privacy({ policy }) {
  return (
    <div className="max-w-4xl mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold text-gray-900 mb-4">Privacy Policy</h1>
      <p className="text-gray-700 leading-relaxed">{policy}</p>
    </div>
  );
}

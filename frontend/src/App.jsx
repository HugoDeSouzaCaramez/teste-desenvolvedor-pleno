import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import ProductList from './pages/ProductList';
import ProductForm from './pages/ProductForm';

const App = () => {
  return (
    <Router>
      <div className="container mx-auto p-4">
        <h1 className="text-2xl font-bold mb-4">Gestão de Produtos</h1>
        <Routes>
          <Route path="/" element={<ProductList />} />
          <Route path="/produto/:id?" element={<ProductForm />} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;

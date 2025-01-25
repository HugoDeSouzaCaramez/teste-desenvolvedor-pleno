import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button } from '../components/ui/Button';
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5082';

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/Produtos/todos`);
      setProducts(Array.isArray(response.data) ? response.data : []);
    } catch (error) {
      console.error('Falha ao buscar produtos:', error);
    }
  };

  const handleDelete = async (id) => {
    try {
      await axios.delete(`${API_BASE_URL}/Produtos/${id}`);
      fetchProducts();
    } catch (error) {
      console.error('Falha ao excluir produto:', error);
    }
  };

  return (
    <div>
      <Button onClick={() => navigate('/produto')} className="mb-4">
        Novo Produto
      </Button>
      <table className="table-auto w-full border-collapse border border-gray-300">
        <thead>
          <tr className="bg-gray-100">
            <th className="border px-4 py-2">Nome</th>
            <th className="border px-4 py-2">Descrição</th>
            <th className="border px-4 py-2">Preço</th>
            <th className="border px-4 py-2">Categoria</th>
            <th className="border px-4 py-2">Fornecedor</th>
            <th className="border px-4 py-2">Ações</th>
          </tr>
        </thead>
        <tbody>
          {products.map((product) => (
            <tr key={product.produtoId}>
              <td className="border px-4 py-2">{product.nome}</td>
              <td className="border px-4 py-2">{product.descricao}</td>
              <td className="border px-4 py-2">{product.preco}</td>
              <td className="border px-4 py-2">{product.categoria?.nome}</td>
              <td className="border px-4 py-2">{product.fornecedor?.nome}</td>
              <td className="border px-4 py-2">
                <Button onClick={() => navigate(`/produto/${product.produtoId}`)}>Editar</Button>
                <Button
                  onClick={() => handleDelete(product.produtoId)}
                  className="ml-2"
                  variant="destructive"
                >
                  Excluir
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ProductList;

import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Table, 
  TableBody, 
  TableCell, 
  TableContainer, 
  TableHead, 
  TableRow, 
  Paper, 
  Typography, 
  Container, 
  Button
} from '@mui/material';
import api from '../api';


const ProductList = () => {
  const [products, setProducts] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    try {
      const response = await api.get('/Produtos/todos');
      setProducts(Array.isArray(response.data) ? response.data : []);
    } catch (error) {
      console.error('Falha ao buscar produtos:', error);
    }
  };

  const handleDelete = async (id) => {
    try {
      await api.delete((`/Produtos/${id}`));
      fetchProducts();
    } catch (error) {
      console.error('Falha ao excluir produto:', error);
    }
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Lista de Produtos
      </Typography>
      <Button onClick={() => navigate('/produto')} className="mb-4">
        Novo Produto
      </Button>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Nome</TableCell>
              <TableCell>Descrição</TableCell>
              <TableCell>Preço</TableCell>
              <TableCell>Categoria</TableCell>
              <TableCell>Fornecedor</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {products.map((product) => (
              <TableRow key={product.produtoId}>
                <TableCell>{product.nome}</TableCell>
                <TableCell>{product.descricao}</TableCell>
                <TableCell>{product.preco}</TableCell>
                <TableCell>{product.categoriaId}</TableCell>
                <TableCell>{product.fornecedorId}</TableCell>
                <TableCell>
                  <Button onClick={() => navigate(`/produto/${product.produtoId}`)}>
                    Editar
                  </Button>
                  <Button onClick={() => handleDelete(product.produtoId)} color="secondary">
                    Excluir
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
};

export default ProductList;

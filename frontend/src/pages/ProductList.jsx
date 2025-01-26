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
  Button,
  TableFooter,
  TablePagination
} from '@mui/material';
import api from '../api';

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(5);
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
      await api.delete(`/Produtos/${id}`);
      fetchProducts();
    } catch (error) {
      console.error('Falha ao excluir produto:', error);
    }
  };

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const displayedProducts = products.slice(
    page * rowsPerPage,
    page * rowsPerPage + rowsPerPage
  );

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
            {displayedProducts.map((product) => (
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
                  <Button
                    onClick={() => handleDelete(product.produtoId)}
                    color="secondary"
                  >
                    Excluir
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
          <TableFooter>
            <TableRow>
              <TablePagination
                rowsPerPageOptions={[5, 10, 25]}
                colSpan={6}
                count={products.length}
                rowsPerPage={rowsPerPage}
                page={page}
                onPageChange={handleChangePage}
                onRowsPerPageChange={handleChangeRowsPerPage}
                labelRowsPerPage="Itens por página:"
                labelDisplayedRows={({ from, to, count }) => `${from}-${to} de ${count}`}
              />
            </TableRow>
          </TableFooter>
        </Table>
      </TableContainer>
    </Container>
  );
};

export default ProductList;

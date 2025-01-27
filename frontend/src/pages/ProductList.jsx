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
  TablePagination,
  TextField,
  Box
} from '@mui/material';
import api from '../api';

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [searchTerm, setSearchTerm] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    if (searchTerm) {
      const delayDebounceFn = setTimeout(() => {
        searchProducts(searchTerm);
      }, 800);

      return () => clearTimeout(delayDebounceFn);
    } else {
      fetchProducts();
    }
  }, [searchTerm]);

  const fetchProducts = async () => {
    try {
      const response = await api.get('/Produtos');
      setProducts(Array.isArray(response.data) ? response.data : []);
    } catch (error) {
      console.error('Falha ao buscar produtos:', error);
    }
  };

  const searchProducts = async (term) => {
    try {
      const response = await api.get(`/Produtos/buscar`, {
        params: { nome: term }
      });
      setProducts(Array.isArray(response.data) ? response.data : []);
    } catch (error) {
      console.error('Falha ao buscar produtos por nome:', error);
    }
  };

  const handleDelete = async (id, nome) => {
    const confirmDelete = window.confirm(`Tem certeza de que deseja excluir o produto "${nome}"?`);
    if (!confirmDelete) return;
  
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
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        mb={2}
        sx={{ flexDirection: { xs: 'column', sm: 'row' }, gap: 2 }}
      >
        <TextField
          label="Buscar Produtos"
          variant="outlined"
          size="small"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          sx={{ width: { xs: '100%', sm: 'auto' } }}
        />
        <Button
          onClick={() => navigate('/produto')}
          variant="contained"
          color="primary"
        >
          Novo Produto
        </Button>
      </Box>
      <TableContainer
        component={Paper}
        sx={{
          overflowX: 'auto',
          maxWidth: '100%',
          '&::-webkit-scrollbar': { display: 'none' },
        }}
      >
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
                  <Button
                    onClick={() => navigate(`/produto/${product.produtoId}`)}
                    variant="contained"
                    color="primary"
                    size="small"
                    sx={{ fontSize: { xs: '0.75rem', sm: '0.875rem' } }}
                  >
                    Editar
                  </Button>
                  <Button
                    onClick={() => handleDelete(product.produtoId, product.nome)}
                    variant="contained"
                    color="secondary"
                    size="small"
                    sx={{
                      fontSize: { xs: '0.75rem', sm: '0.875rem' },
                      marginLeft: { xs: 0, sm: 1 },
                      marginTop: { xs: 1, sm: 0 },
                    }}
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
                labelDisplayedRows={({ from, to, count }) =>
                  `${from}-${to} de ${count}`
                }
                sx={{
                  '.MuiTablePagination-toolbar': {
                    flexDirection: { xs: 'column', sm: 'row' },
                    alignItems: { xs: 'start', sm: 'center' },
                  },
                }}
              />
            </TableRow>
          </TableFooter>
        </Table>
      </TableContainer>
    </Container>
  );
};

export default ProductList;

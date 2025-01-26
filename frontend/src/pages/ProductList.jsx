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
} from '@mui/material';
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5082';

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const navigate = useNavigate();

  useEffect(() => {
    fetchProducts();
  }, [pageNumber, pageSize]);

  const fetchProducts = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/Produtos`, {
        params: { pageNumber, pageSize },
      });

      if (response.data && response.data.data) {
        setProducts(response.data.data);
        setTotalCount(response.data.totalCount);
      } else {
        setProducts([]);
        setTotalCount(0);
      }
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

  const handlePageChange = (event, newPage) => {
    setPageNumber(newPage + 1);
  };

  const handleRowsPerPageChange = (event) => {
    setPageSize(parseInt(event.target.value, 10));
    setPageNumber(1);
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
          <TableFooter>
            <TableRow>
              <TablePagination
                rowsPerPageOptions={[5, 10, 20, 40, 60, 100, 200]}
                count={totalCount}
                rowsPerPage={pageSize}
                page={pageNumber - 1}
                onPageChange={handlePageChange}
                onRowsPerPageChange={handleRowsPerPageChange}
                labelRowsPerPage="Itens por página"
                labelDisplayedRows={({ from, to, count }) =>
                  `${from}-${to} de ${count !== -1 ? count : `mais de ${to}`}`
                }
              />
            </TableRow>
          </TableFooter>
        </Table>
      </TableContainer>
    </Container>
  );
};

export default ProductList;

import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { TextField, Grid, Container, Typography, Button, MenuItem } from '@mui/material';
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5082';

const ProductForm = () => {
  const [form, setForm] = useState({
    produtoId: null,
    nome: '',
    descricao: '',
    preco: 0,
    imagemUrl: '',
    estoque: 0,
    categoriaId: 1,
    fornecedorId: 1,
  });
  const [categorias, setCategorias] = useState([]);
  const [fornecedores, setFornecedores] = useState([]);
  const { id } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
    fetchCategorias();
    fetchFornecedores();
    if (id) {
      fetchProduct(id);
    }
  }, [id]);

  const fetchProduct = async (id) => {
    try {
      const response = await axios.get(`${API_BASE_URL}/Produtos/${id}`);
      setForm(response.data);
    } catch (error) {
      console.error('Falha ao buscar o produto:', error);
    }
  };

  const fetchCategorias = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/Categorias`);
      setCategorias(response.data);
    } catch (error) {
      console.error('Falha ao buscar categorias:', error);
    }
  };

  const fetchFornecedores = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/Fornecedores`);
      setFornecedores(response.data);
    } catch (error) {
      console.error('Falha ao buscar fornecedores:', error);
    }
  };

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (id) {
        await axios.put(`${API_BASE_URL}/Produtos/${id}`, form);
      } else {
        await axios.post(`${API_BASE_URL}/Produtos`, form);
      }
      navigate('/');
    } catch (error) {
      console.error('Falha ao salvar o produto:', error);
    }
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        {id ? 'Editar Produto' : 'Novo Produto'}
      </Typography>
      <form onSubmit={handleSubmit}>
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="Nome do Produto"
              name="nome"
              value={form.nome}
              onChange={handleFormChange}
              required
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="Descrição"
              name="descricao"
              value={form.descricao}
              onChange={handleFormChange}
              required
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              type="number"
              label="Preço"
              name="preco"
              value={form.preco}
              onChange={handleFormChange}
              required
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="URL da Imagem"
              name="imagemUrl"
              value={form.imagemUrl}
              onChange={handleFormChange}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              type="number"
              label="Estoque"
              name="estoque"
              value={form.estoque}
              onChange={handleFormChange}
              required
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              select
              label="Categoria"
              name="categoriaId"
              value={form.categoriaId}
              onChange={handleFormChange}
            >
              {categorias.map((categoria) => (
                <MenuItem key={categoria.categoriaId} value={categoria.categoriaId}>
                  {categoria.nome}
                </MenuItem>
              ))}
            </TextField>
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              select
              label="Fornecedor"
              name="fornecedorId"
              value={form.fornecedorId}
              onChange={handleFormChange}
            >
              {fornecedores.map((fornecedor) => (
                <MenuItem key={fornecedor.fornecedorId} value={fornecedor.fornecedorId}>
                  {fornecedor.nome}
                </MenuItem>
              ))}
            </TextField>
          </Grid>
          <Grid item xs={12}>
            <Button type="submit" color="primary" variant="contained">
              Salvar
            </Button>
            <Button
              onClick={() => navigate('/')}
              color="secondary"
              variant="outlined"
              style={{ marginLeft: '1rem' }}
            >
              Cancelar
            </Button>
          </Grid>
        </Grid>
      </form>
    </Container>
  );
};

export default ProductForm;

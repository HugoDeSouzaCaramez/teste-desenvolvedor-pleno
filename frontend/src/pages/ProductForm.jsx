import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { TextField, Grid, Container, Typography, Button, MenuItem } from '@mui/material';
import api from '../api';


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
  const [errors, setErrors] = useState({});
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
      const response = await api.get(`/Produtos/${id}`);
      setForm(response.data);
    } catch (error) {
      console.error('Falha ao buscar o produto:', error);
    }
  };

  const fetchCategorias = async () => {
    try {
      const response = await api.get('/Categorias');
      setCategorias(response.data);
    } catch (error) {
      console.error('Falha ao buscar categorias:', error);
    }
  };

  const fetchFornecedores = async () => {
    try {
      const response = await api.get('/Fornecedores');
      setFornecedores(response.data);
    } catch (error) {
      console.error('Falha ao buscar fornecedores:', error);
    }
  };

  const validate = () => {
    const newErrors = {};
    if (form.nome.length < 3 || form.nome.length > 80) {
      newErrors.nome = 'O nome deve ter entre 3 e 80 caracteres.';
    }
    if (form.descricao.length < 5 || form.descricao.length > 300) {
      newErrors.descricao = 'A descrição deve ter entre 5 e 300 caracteres.';
    }
    if (form.preco < 1 || form.preco > 99999) {
      newErrors.preco = 'O preço deve ser entre 1 e 99999.';
    }
    if (form.imagemUrl.length < 3 || form.imagemUrl.length > 80) {
      newErrors.imagemUrl = 'A URL da imagem deve ter entre 3 e 80 caracteres.';
    }
    if (form.estoque < 0 || form.estoque > 99999) {
      newErrors.estoque = 'O estoque deve ser entre 0 e 99999.';
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setForm({
      ...form,
      [name]: name === 'preco'
        ? parseFloat(value)
        : name === 'estoque'
          ? parseInt(value, 10)
          : value,
    });
    setErrors({ ...errors, [name]: '' });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validate()) return;

    try {
      const formToSubmit = id ? form : { ...form, produtoId: 0 };

      if (id) {
        await api.put(`/Produtos/${id}`, formToSubmit);
      } else {
        await api.post('/Produtos', formToSubmit);
      }
      navigate('/produtos');
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
              error={!!errors.nome}
              helperText={errors.nome}
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
              error={!!errors.descricao}
              helperText={errors.descricao}
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
              error={!!errors.preco}
              helperText={errors.preco}
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
              error={!!errors.imagemUrl}
              helperText={errors.imagemUrl}
              required
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
              error={!!errors.estoque}
              helperText={errors.estoque}
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
              required
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
              required
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
              onClick={() => navigate('/produtos')}
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

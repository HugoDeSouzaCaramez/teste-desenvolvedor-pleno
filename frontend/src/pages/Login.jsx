import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TextField, Button, Container, Typography, Grid } from '@mui/material';
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5082';

const Login = () => {
  const [form, setForm] = useState({ nome: '', senha: '' });
  const navigate = useNavigate();

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(`${API_BASE_URL}/Autenticacoes/login`, form);
      localStorage.setItem('token', response.data.token);
      navigate('/produtos');
    } catch (error) {
      console.error('Falha ao realizar login:', error);
    }
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Login
      </Typography>
      <form onSubmit={handleSubmit}>
        <Grid container spacing={2}>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Nome"
              name="nome"
              value={form.nome}
              onChange={handleFormChange}
              required
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Senha"
              type="password"
              name="senha"
              value={form.senha}
              onChange={handleFormChange}
              required
            />
          </Grid>
          <Grid item xs={12}>
            <Button type="submit" color="primary" variant="contained">
              Entrar
            </Button>
            <Button
              onClick={() => navigate('/register')}
              color="secondary"
              variant="outlined"
              style={{ marginLeft: '1rem' }}
            >
              Registrar-se
            </Button>
          </Grid>
        </Grid>
      </form>
    </Container>
  );
};

export default Login;
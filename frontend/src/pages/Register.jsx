import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TextField, Button, Container, Typography, Grid } from '@mui/material';
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5082';

const Register = () => {
  const [form, setForm] = useState({ nome: '', senha: '' });
  const navigate = useNavigate();

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post(`${API_BASE_URL}/Usuarios`, form);
      navigate('/');
    } catch (error) {
      console.error('Falha ao registrar usuário:', error);
    }
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Registrar-se
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
              Registrar
            </Button>
            <Button
              onClick={() => navigate('/login')}
              color="secondary"
              variant="outlined"
              style={{ marginLeft: '1rem' }}
            >
              Voltar para Login
            </Button>
          </Grid>
        </Grid>
      </form>
    </Container>
  );
};

export default Register;
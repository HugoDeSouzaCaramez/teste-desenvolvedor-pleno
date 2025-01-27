import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TextField, Button, Container, Typography, Grid } from '@mui/material';
import api from '../api';


const Login = () => {
  const [form, setForm] = useState({ nome: '', senha: '' });
  const [errors, setErrors] = useState({ nome: '', senha: '' });
  const navigate = useNavigate();

  const validate = () => {
    const newErrors = {};
    if (form.nome.length < 3 || form.nome.length > 80) {
      newErrors.nome = 'O nome deve ter entre 3 e 80 caracteres.';
    }
    if (form.senha.length < 8 || form.senha.length > 80) {
      newErrors.senha = 'A senha deve ter entre 8 e 80 caracteres.';
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
    setErrors({ ...errors, [name]: '' });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validate()) return;
    try {
      const response = await api.post('/Autenticacoes/login', form);
      localStorage.setItem('token', response.data.token);
      navigate('/produtos');
    } catch (error) {
      console.error('Falha ao realizar login:', error);
      alert(error.response.data)
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
              error={!!errors.nome}
              helperText={errors.nome}
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
              error={!!errors.senha}
              helperText={errors.senha}
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
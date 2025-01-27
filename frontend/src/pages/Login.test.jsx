import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import Login from './Login';
import api from '../api';

jest.mock('../api');

const mockNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => mockNavigate,
}));

describe('Login Component', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        global.alert = jest.fn();
    });

    it('deve renderizar o formulário de login corretamente', () => {
        render(
            <MemoryRouter>
                <Login />
            </MemoryRouter>
        );

        expect(screen.getByLabelText(/nome/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/senha/i)).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /entrar/i })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /registrar-se/i })).toBeInTheDocument();
    });

    it('deve exibir mensagens de erro para validação inválida', async () => {
        render(
            <MemoryRouter>
                <Login />
            </MemoryRouter>
        );

        fireEvent.change(screen.getByLabelText(/nome/i), { target: { value: 'Jo' } });
        fireEvent.change(screen.getByLabelText(/senha/i), { target: { value: '12345' } });

        fireEvent.click(screen.getByRole('button', { name: /entrar/i }));

        expect(await screen.findByText('O nome deve ter entre 3 e 80 caracteres.')).toBeInTheDocument();
        expect(await screen.findByText('A senha deve ter entre 8 e 80 caracteres.')).toBeInTheDocument();
    });

    it('deve limpar mensagens de erro ao corrigir os campos', async () => {
        render(
            <MemoryRouter>
                <Login />
            </MemoryRouter>
        );

        fireEvent.change(screen.getByLabelText(/nome/i), { target: { value: 'Jo' } });
        fireEvent.change(screen.getByLabelText(/senha/i), { target: { value: '12345' } });

        fireEvent.click(screen.getByRole('button', { name: /entrar/i }));

        expect(await screen.findByText('O nome deve ter entre 3 e 80 caracteres.')).toBeInTheDocument();
        expect(await screen.findByText('A senha deve ter entre 8 e 80 caracteres.')).toBeInTheDocument();

        fireEvent.change(screen.getByLabelText(/nome/i), { target: { value: 'João' } });
        fireEvent.change(screen.getByLabelText(/senha/i), { target: { value: '12345678' } });

        expect(screen.queryByText('O nome deve ter entre 3 e 80 caracteres.')).not.toBeInTheDocument();
        expect(screen.queryByText('A senha deve ter entre 8 e 80 caracteres.')).not.toBeInTheDocument();
    });

    it('deve chamar a API e redirecionar ao realizar login com sucesso', async () => {
        const mockResponse = { data: { token: 'mock-token' } };
        api.post.mockResolvedValueOnce(mockResponse);

        render(
            <MemoryRouter>
                <Login />
            </MemoryRouter>
        );

        fireEvent.change(screen.getByLabelText(/nome/i), { target: { value: 'usuario' } });
        fireEvent.change(screen.getByLabelText(/senha/i), { target: { value: 'senhaValida123' } });

        fireEvent.click(screen.getByRole('button', { name: /entrar/i }));

        await waitFor(() => expect(api.post).toHaveBeenCalledWith('/Autenticacoes/login', {
            nome: 'usuario',
            senha: 'senhaValida123',
        }));

        expect(localStorage.getItem('token')).toBe('mock-token');
        expect(mockNavigate).toHaveBeenCalledWith('/produtos');
    });
});

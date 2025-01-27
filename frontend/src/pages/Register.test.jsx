import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import Register from './Register';
import api from '../api';

jest.mock('../api');

const mockNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => mockNavigate,
}));

describe('Register Component', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        global.alert = jest.fn();
    });

    it('deve renderizar o formulário de registro corretamente', () => {
        render(
            <MemoryRouter>
                <Register />
            </MemoryRouter>
        );

        expect(screen.getByText(/registrar-se/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/nome/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/senha/i)).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /registrar/i })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /voltar para login/i })).toBeInTheDocument();
    });

    it('deve exibir mensagens de erro para validação inválida', async () => {
        render(
            <MemoryRouter>
                <Register />
            </MemoryRouter>
        );

        fireEvent.change(screen.getByLabelText(/nome/i), { target: { value: 'Jo' } });
        fireEvent.change(screen.getByLabelText(/senha/i), { target: { value: '12345' } });

        fireEvent.click(screen.getByRole('button', { name: /registrar/i }));

        expect(await screen.findByText('O nome deve ter entre 3 e 80 caracteres.')).toBeInTheDocument();
        expect(await screen.findByText('A senha deve ter entre 8 e 80 caracteres.')).toBeInTheDocument();
    });

    it('deve limpar mensagens de erro ao corrigir os campos', async () => {
        render(
            <MemoryRouter>
                <Register />
            </MemoryRouter>
        );

        fireEvent.change(screen.getByLabelText(/nome/i), { target: { value: 'Jo' } });
        fireEvent.change(screen.getByLabelText(/senha/i), { target: { value: '12345' } });

        fireEvent.click(screen.getByRole('button', { name: /registrar/i }));

        expect(await screen.findByText('O nome deve ter entre 3 e 80 caracteres.')).toBeInTheDocument();
        expect(await screen.findByText('A senha deve ter entre 8 e 80 caracteres.')).toBeInTheDocument();

        fireEvent.change(screen.getByLabelText(/nome/i), { target: { value: 'João' } });
        fireEvent.change(screen.getByLabelText(/senha/i), { target: { value: '12345678' } });

        expect(screen.queryByText('O nome deve ter entre 3 e 80 caracteres.')).not.toBeInTheDocument();
        expect(screen.queryByText('A senha deve ter entre 8 e 80 caracteres.')).not.toBeInTheDocument();
    });

    it('deve chamar a API e navegar para a página de login ao registrar com sucesso', async () => {
        api.post.mockResolvedValueOnce({});

        render(
            <MemoryRouter>
                <Register />
            </MemoryRouter>
        );

        fireEvent.change(screen.getByLabelText(/nome/i), { target: { value: 'usuario' } });
        fireEvent.change(screen.getByLabelText(/senha/i), { target: { value: 'senhaValida123' } });

        fireEvent.click(screen.getByRole('button', { name: /registrar/i }));

        await waitFor(() => expect(api.post).toHaveBeenCalledWith('/Usuarios', {
            nome: 'usuario',
            senha: 'senhaValida123',
        }));

        expect(mockNavigate).toHaveBeenCalledWith('/');
    });

    it('deve exibir mensagem de erro ao falhar o registro', async () => {
        api.post.mockRejectedValueOnce({
            response: {
                data: {
                    errors: {
                        Nome: ['Nome já está em uso.'],
                    },
                },
            },
        });

        render(
            <MemoryRouter>
                <Register />
            </MemoryRouter>
        );

        fireEvent.change(screen.getByLabelText(/nome/i), { target: { value: 'usuarioExistente' } });
        fireEvent.change(screen.getByLabelText(/senha/i), { target: { value: 'senhaValida123' } });

        fireEvent.click(screen.getByRole('button', { name: /registrar/i }));

        await waitFor(() => expect(api.post).toHaveBeenCalled());

        expect(global.alert).toHaveBeenCalledWith('Nome já está em uso.');
    });

    it('deve navegar de volta para a página de login ao clicar no botão "Voltar para Login"', () => {
        render(
            <MemoryRouter>
                <Register />
            </MemoryRouter>
        );

        fireEvent.click(screen.getByRole('button', { name: /voltar para login/i }));

        expect(mockNavigate).toHaveBeenCalledWith('/');
    });
});

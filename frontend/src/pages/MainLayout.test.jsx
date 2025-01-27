import { render, screen, fireEvent } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import MainLayout from './MainLayout';

const mockNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => mockNavigate,
}));

describe('MainLayout', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        localStorage.clear();
    });

    it('deve renderizar o título e o botão de logout corretamente', () => {
        render(
            <MemoryRouter>
                <MainLayout>
                    <div>Conteúdo de teste</div>
                </MainLayout>
            </MemoryRouter>
        );

        expect(screen.getByText(/gestão de produtos/i)).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /logout/i })).toBeInTheDocument();
        expect(screen.getByText(/conteúdo de teste/i)).toBeInTheDocument();
    });

    it('deve redirecionar para a página inicial se o token não estiver presente', () => {
        render(
            <MemoryRouter>
                <MainLayout>
                    <div>Conteúdo protegido</div>
                </MainLayout>
            </MemoryRouter>
        );

        expect(mockNavigate).toHaveBeenCalledWith('/');
    });

    it('deve permanecer na página se o token estiver presente', () => {
        localStorage.setItem('token', 'mock-token');

        render(
            <MemoryRouter>
                <MainLayout>
                    <div>Conteúdo protegido</div>
                </MainLayout>
            </MemoryRouter>
        );

        expect(mockNavigate).not.toHaveBeenCalled();
        expect(screen.getByText(/conteúdo protegido/i)).toBeInTheDocument();
    });

    it('deve remover o token do localStorage e redirecionar ao clicar no botão de logout', () => {
        localStorage.setItem('token', 'mock-token');

        render(
            <MemoryRouter>
                <MainLayout>
                    <div>Conteúdo protegido</div>
                </MainLayout>
            </MemoryRouter>
        );

        const logoutButton = screen.getByRole('button', { name: /logout/i });
        fireEvent.click(logoutButton);

        expect(localStorage.getItem('token')).toBeNull();
        expect(mockNavigate).toHaveBeenCalledWith('/');
    });
});

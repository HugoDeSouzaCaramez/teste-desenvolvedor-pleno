import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import ProductList from './ProductList';
import api from '../api';

jest.mock('../api', () => ({
    get: jest.fn(),
    delete: jest.fn(),
}));

const mockNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => mockNavigate,
}));

describe('ProductList Component', () => {
    beforeEach(() => {
        jest.clearAllMocks();
        api.get.mockResolvedValue({ data: [] });
    });

    it('deve renderizar a lista de produtos corretamente', async () => {
        api.get.mockResolvedValueOnce({
            data: [
                { produtoId: 1, nome: 'Produto 1', descricao: 'Desc 1', preco: 100, categoriaId: 'Cat 1', fornecedorId: 'Fornecedor 1' },
                { produtoId: 2, nome: 'Produto 2', descricao: 'Desc 2', preco: 200, categoriaId: 'Cat 2', fornecedorId: 'Fornecedor 2' },
            ]
        });

        render(
            <MemoryRouter>
                <ProductList />
            </MemoryRouter>
        );

        await waitFor(() => expect(api.get).toHaveBeenCalledWith('/Produtos'));

        expect(screen.getByText(/lista de produtos/i)).toBeInTheDocument();
        expect(screen.getByText('Produto 1')).toBeInTheDocument();
        expect(screen.getByText('Produto 2')).toBeInTheDocument();
    });

    it('deve exibir uma mensagem quando não há produtos', async () => {
        api.get.mockResolvedValueOnce({ data: [] });

        render(
            <MemoryRouter>
                <ProductList />
            </MemoryRouter>
        );

        await waitFor(() => expect(api.get).toHaveBeenCalledWith('/Produtos'));

        expect(screen.getByText(/lista de produtos/i)).toBeInTheDocument();
        expect(screen.queryByText('Produto 1')).not.toBeInTheDocument();
    });

    it('deve navegar para a página de criação ao clicar no botão "Novo Produto"', () => {
        render(
            <MemoryRouter>
                <ProductList />
            </MemoryRouter>
        );

        const newProductButton = screen.getByRole('button', { name: /novo produto/i });
        fireEvent.click(newProductButton);

        expect(mockNavigate).toHaveBeenCalledWith('/produto');
    });

    it('deve excluir um produto ao clicar no botão "Excluir"', async () => {
        window.confirm = jest.fn().mockReturnValueOnce(true);
        api.get.mockResolvedValueOnce({
            data: [
                { produtoId: 1, nome: 'Produto 1', descricao: 'Desc 1', preco: 100, categoriaId: 'Cat 1', fornecedorId: 'Fornecedor 1' },
            ]
        });
        api.delete.mockResolvedValueOnce({});

        render(
            <MemoryRouter>
                <ProductList />
            </MemoryRouter>
        );

        await waitFor(() => expect(api.get).toHaveBeenCalled());

        const deleteButton = screen.getByRole('button', { name: /excluir/i });
        fireEvent.click(deleteButton);

        await waitFor(() => expect(api.delete).toHaveBeenCalledWith('/Produtos/1'));
        expect(api.get).toHaveBeenCalledTimes(2);
    });

    it('deve navegar para a página de edição ao clicar no botão "Editar"', async () => {
        api.get.mockResolvedValueOnce({
            data: [
                { produtoId: 1, nome: 'Produto 1', descricao: 'Desc 1', preco: 100, categoriaId: 'Cat 1', fornecedorId: 'Fornecedor 1' },
            ]
        });

        render(
            <MemoryRouter>
                <ProductList />
            </MemoryRouter>
        );

        await waitFor(() => expect(api.get).toHaveBeenCalled());

        const editButton = screen.getByRole('button', { name: /editar/i });
        fireEvent.click(editButton);

        expect(mockNavigate).toHaveBeenCalledWith('/produto/1');
    });
});

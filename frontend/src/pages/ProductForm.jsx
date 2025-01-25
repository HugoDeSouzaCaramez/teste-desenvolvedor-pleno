import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Button } from '../components/ui/Button';
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
    const [categories, setCategories] = useState([]);
    const [suppliers, setSuppliers] = useState([]);
    const [currentCategory, setCurrentCategory] = useState(null);
    const [currentSupplier, setCurrentSupplier] = useState(null);
    const { id } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        fetchCategories();
        fetchSuppliers();
        if (id) fetchProduct(id);
    }, [id]);

    const fetchProduct = async (id) => {
        try {
            const response = await axios.get(`${API_BASE_URL}/Produtos/${id}`);
            const product = response.data;

            setForm(product);
            if (product.categoriaId) fetchCategoryById(product.categoriaId);
            if (product.fornecedorId) fetchSupplierById(product.fornecedorId);

        } catch (error) {
            console.error('Falha ao buscar o produto:', error);
        }
    };

    const fetchCategories = async () => {
        try {
            const response = await axios.get(`${API_BASE_URL}/Categorias`);
            setCategories(response.data);
        } catch (error) {
            console.error('Falha ao buscar categorias:', error);
        }
    };

    const fetchSuppliers = async () => {
        try {
            const response = await axios.get(`${API_BASE_URL}/Fornecedores`);
            setSuppliers(response.data);
        } catch (error) {
            console.error('Falha ao buscar fornecedores:', error);
        }
    };

    const fetchCategoryById = async (id) => {
        try {
            const response = await axios.get(`${API_BASE_URL}/Categorias/${id}`);

            setCurrentCategory(response.data);
        } catch (error) {
            console.error("Falha ao buscar a categoria:", error);
        }
    };

    const fetchSupplierById = async (id) => {
        try {
            const response = await axios.get(`${API_BASE_URL}/Fornecedores/${id}`);
            setCurrentSupplier(response.data);
        } catch (error) {
            console.error("Falha ao buscar o fornecedor:", error);
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
        <form onSubmit={handleSubmit} className="grid grid-cols-1 gap-4 md:grid-cols-2">
            <input
                type="text"
                name="nome"
                value={form.nome}
                onChange={handleFormChange}
                placeholder="Nome do Produto"
                required
                className="border p-2 rounded"
            />
            <input
                type="text"
                name="descricao"
                value={form.descricao}
                onChange={handleFormChange}
                placeholder="Descrição"
                required
                className="border p-2 rounded"
            />
            <input
                type="number"
                name="preco"
                value={form.preco}
                onChange={handleFormChange}
                placeholder="Preço"
                required
                className="border p-2 rounded"
            />
            <input
                type="text"
                name="imagemUrl"
                value={form.imagemUrl}
                onChange={handleFormChange}
                placeholder="URL da Imagem"
                className="border p-2 rounded"
            />
            <input
                type="number"
                name="estoque"
                value={form.estoque}
                onChange={handleFormChange}
                placeholder="Estoque"
                required
                className="border p-2 rounded"
            />
            <select
                name="categoriaId"
                value={form.categoriaId}
                onChange={handleFormChange}
                className="border p-2 rounded"
            >
                {currentCategory && (
                    <option value={currentCategory.id}>
                        {currentCategory.nome}
                    </option>
                )}

                {categories.map((category) => (
                    <option key={category.id} value={category.id}>
                        {category.nome}
                    </option>
                ))}
            </select>
            <select
                name="fornecedorId"
                value={form.fornecedorId}
                onChange={handleFormChange}
                className="border p-2 rounded"
            >
                {currentSupplier && (
                    <option value={currentSupplier.id}>
                        {currentSupplier.nome}
                    </option>
                )}

                {suppliers.map((supplier) => (
                    <option key={supplier.id} value={supplier.id}>
                        {supplier.nome}
                    </option>
                ))}
            </select>
            <Button type="submit">Salvar</Button>
            <Button onClick={() => navigate('/')} variant="secondary">
                Cancelar
            </Button>
        </form>
    );
};

export default ProductForm;

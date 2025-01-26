import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import ProductList from "./pages/ProductList";
import ProductForm from "./pages/ProductForm";
import { CssBaseline, ThemeProvider, createTheme, Grid } from "@mui/material";


const theme = createTheme({
  palette: {
    primary: {
      main: "#1976d2",
    },
    secondary: {
      main: "#f50057",
    },
  },
  typography: {
    fontFamily: "Roboto, Arial, sans-serif",
  },
});

const App = () => {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Grid 
          container 
          direction="column" 
          justifyContent="center" 
          alignItems="center" 
          style={{ minHeight: "100vh", width: "100%" }}
        >
          <h1 style={{ textAlign: "center", marginBottom: "2rem" }}>Gestão de Produtos</h1>
          <Routes>
            <Route path="/" element={<ProductList />} />
            <Route path="/produto/:id?" element={<ProductForm />} />
          </Routes>
        </Grid>
      </Router>
    </ThemeProvider>
  );
};

export default App;

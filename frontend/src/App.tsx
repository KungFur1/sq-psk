import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import Register from './pages/Register';
import Login from './pages/Login';
import RecipeDetail from './pages/RecipeDetail';
import CreateRecipe from './pages/CreateRecipe';
import './styles/App.css';
import SearchResult from "./pages/SearchResults.tsx";
import MyRecipes from "./pages/MyRecipes.tsx";

const App: React.FC = () => {
    return (
        <Router>
            <div className="App">
                <Routes>
                    <Route path="/" element={<Home/>} />
                    <Route path="/register" element={<Register/>} />
                    <Route path="/login" element={<Login/>} />
                    <Route path="/recipe/:recipeId" element={<RecipeDetail/>} />
                    <Route path="/create" element={<CreateRecipe/>} />
                    <Route path="/search" element={<SearchResult/>} />
                    <Route path="/my-recipes" element={<MyRecipes/>} />
                </Routes>
            </div>
        </Router>
    );
};

export default App;

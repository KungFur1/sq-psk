import React, {useState, useEffect} from 'react';
import Header from '../components/Header';
import RecipeCard from '../components/RecipeCard';
import defaultServerConfig from "../common/server-info.ts";
import "../styles/Home.css";
import Button from "../components/Button.tsx";
import {useNavigate, useSearchParams} from "react-router-dom";
import Recipe from "../types/Recipe.ts";
import axios from "axios";
import frontimage from "../assets/frontpage.png";

const MyRecipes: React.FC = () => {
    const {apiUrl} = defaultServerConfig;
    const {key} = JSON.parse(localStorage.getItem('sessionInfo'));

    const [recipes, setRecipes] = useState<Array<Recipe>>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [searchQuery, setSearchQuery] = useState('');
    const [getUserId, setUserId] = useState<string | null>(null);

    const navigate = useNavigate();
    const handleSearchClick = () => {
        navigate(`/search?q=${searchQuery}`)
    }

    useEffect(() => {
        axios.get(`${apiUrl}/api/recipes`)
            .then(async response => {
                setRecipes(response.data);
                setLoading(false);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    }, []);

    useEffect(() => {
        axios.post(`${apiUrl}/api/auth/decode-session`, {
            sessionKey: key,
        })
            .then(async response => {
                console.debug(response.data)
                const {userId} = response.data;
                setUserId(userId);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    }, []);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error loading recipes: {error.message}</p>;

    return (
        <div className="home-container">
            <Header/>
            <main className='home-container'>
                <h2>My recipes</h2>
                <div className="recipe-list">
                    {recipes.filter(r => r.userId == getUserId).map((recipe) => {

                        console.error(getUserId)
                        console.error(recipe.userId)

                        return (
                            <RecipeCard
                                id={recipe.id}
                                title={recipe.title}
                                duration={`${recipe.cookTime + recipe.prepTime} min.`}
                                imageId={recipe.imageId}
                            />
                        );
                    })}
                </div>
            </main>
        </div>
    );
};

export default MyRecipes;


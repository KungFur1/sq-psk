import React, {useState, useEffect} from 'react';
import Header from '../components/Header';
import RecipeCard from '../components/RecipeCard';
import defaultServerConfig from "../common/server-info.ts";
import "../styles/Home.css";
import Button from "../components/Button.tsx";
import {useNavigate, useSearchParams} from "react-router-dom";
import SearchRecipe from "../types/SearchRecipe.ts";

const SearchResult: React.FC = () => {
    const {apiUrl} = defaultServerConfig;
    const [recipes, setRecipes] = useState<Array<SearchRecipe>>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [searchText, setSearchText] = useState('');
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();

    useEffect( () => {
        const searchTerm = searchParams.get("q") || '';
        console.debug(searchParams.get("q"));
        setSearchText(searchTerm);
        console.debug("SEARCH PARAM")
        console.debug(searchTerm)

        const fetchData = async () => {
            try {
                const response = await fetch(`${apiUrl}/api/search?q=${searchTerm}`, {
                    method: "GET",
                });
                console.debug(response)
                let data = await response.json()
                console.debug(data.results);
                data = data.results
                console.debug(data);
                setRecipes(data);
                // setRecipes(data.map((recipe: Recipe, index: number) => ({
                //     ...recipe, id: index
                // })));
                setLoading(false);
            } catch (error) {
                console.error(error)
                setError(error);
                setLoading(false);
            }
        };

        fetchData()
    }, [apiUrl, searchParams]);

    const handleSearchClick = (e: React.FormEvent) => {
        e.preventDefault();
        navigate(`/search?q=${searchText}`);
    };

    console.debug(recipes);

    return (
        <div className="home-container">
            <Header/>
            <main className='home-container'>
                <form className="search-bar" onSubmit={handleSearchClick}>
                    <input
                        type="text"
                        placeholder="Search"
                        value={searchText}
                        onChange={(e) => setSearchText(e.target.value)}
                    />
                    <Button
                        text="Search"
                        type="submit"
                    />
                </form>
                <h2>Best recipe results</h2>
                {
                    loading ? (
                        <p>Loading...</p>
                    ) : ( error ? (
                        <p>Error loading recipes: {error.message}</p>
                    ) : (
                        <div className="recipe-list">
                            {recipes.map((recipe) => (
                                <RecipeCard
                                    key={recipe.id}
                                    id={recipe.id}
                                    title={recipe.title}
                                    duration={`X min.`}
                                    rating={4}
                                    imageUrl={recipe.imageId}
                                />
                            ))}
                        </div>

                    ))
                }
            </main>
        </div>
    );
};

export default SearchResult;

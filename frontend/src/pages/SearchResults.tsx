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

    function getRandomInt(max) {
        return Math.floor(Math.random() * max);
    }


    const fetchData = async () => {

        const searchTerm = localStorage.getItem('searchTerm');
        setSearchText(searchTerm);

        try {
            const response = await fetch(
                `${apiUrl}/api/search?searchTerm=${searchTerm}&pageNumber=1&pageSize=100`,
                { method: "GET", }
            );

            let data = (await response.json()).results;

            // console.error(data);
            // alert(JSON.stringify(data));

            // const imageId =(await (await fetch(
            //     `${apiUrl}/api/recipe/${data.id}`,
            // )).json()).imageId;
            // data.imageId = imageId;

            setRecipes(data);
            setLoading(false);
        } catch (error) {
            console.error(error)
            setError(error);
            setLoading(false);
        }
    };

    useEffect( () => {
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
                        onChange={(e) => { setSearchText(e.target.value); localStorage.setItem('searchTerm', e.target.value)}}
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
                    ) : ( (recipes.length == 0) ?
                        <p> No results </p>
                    : (
                            <div className="recipe-list">
                                {recipes.map((recipe) => {
                                    return (
                                    <RecipeCard
                                        key={recipe.id}
                                        id={recipe.id}
                                        title={recipe.title}
                                        duration={`${getRandomInt(30)} min.`}
                                        rating={4}
                                        imageId={recipe.imageId}
                                    />
                                )})}
                            </div>
                    )))
                }
            </main>
        </div>
    );
};

export default SearchResult;

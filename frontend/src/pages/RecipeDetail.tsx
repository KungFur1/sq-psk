import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/RecipeDetail.css';
import axios from "axios";
import defaultServerConfig from "../common/server-info.ts";
import Recipe from "../types/Recipe.ts";
import InfoChip from "../components/InfoChip.tsx";
import placeholder from "../assets/placeholder.jpeg";
import Image from "../components/Image.tsx";

const RecipeDetail: React.FC = () => {
    const { apiUrl } = defaultServerConfig;
    const params = useParams();
    const id = params['recipeId'];
    const [recipe, setRecipe] = useState<Recipe | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    console.debug(params);
    console.debug(`Recipe id: ${id}`);

    useEffect(() => {
        axios.get(`${apiUrl}/api/recipes/${id}`)
            .then(response => {
                setRecipe(response.data);
                setLoading(false);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    }, [id, apiUrl]);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error loading recipe: {error.message}</p>;

    return (
        <div>
            <Header />
            <main className="recipe-detail">
                <div className="recipe-header">
                    <h1>{recipe?.title}</h1>
                    <div className="rating">
                        <span>⭐⭐⭐⭐⭐</span>
                    </div>
                </div>
                <p>{recipe?.shortDescription}</p>
                <Image
                    src={recipe?.imageId ? `${apiUrl}/api/images/${recipe.imageId}` : placeholder}
                    placeholder={placeholder}
                    alt='Photo of the dish'
                />
                <div className="recipe-info">
                    <InfoChip title="Prep time" value={recipe?.prepTime} postfix="min."></InfoChip>
                    <InfoChip title="Cook time" value={`${recipe?.cookTime}`} postfix="min."></InfoChip>
                    <InfoChip title="Total time" value={`${recipe?.prepTime + recipe?.cookTime}`} postfix="min"></InfoChip>
                    <InfoChip title="Servings" value={`${recipe?.servings}`} postfix=""></InfoChip>
                </div>
                <h3>Ingredients:</h3>
                <ul>
                    {recipe?.ingredientsList.split(',').map((ingredient, index) => (
                        <li key={index}>{ingredient}</li>
                    ))}
                </ul>
                <h3>Directions:</h3>
                <ol>
                    {recipe?.cookingSteps.split(',').map((step, index) => (
                        <li key={index}>{step}</li>
                    ))}
                </ol>
                <h3>Reviews:</h3>
                <strong>TODO</strong>
                {/*{recipe?.reviews.map((review, index) => (*/}
                {/*    <div className="review" key={index}>*/}
                {/*        <p><strong>{review.user}</strong></p>*/}
                {/*        <p>{'⭐'.repeat(review.rating)}</p>*/}
                {/*        <p>{review.comment}</p>*/}
                {/*    </div>*/}
                {/*))}*/}
                <div className="write-review">
                    <h3>Write your own review:</h3>
                    <strong>TODO</strong>
                    {/*<form>*/}
                    {/*    <label>*/}
                    {/*        <span>Rating:</span>*/}
                    {/*        <input type="number" min="0" max="5" />*/}
                    {/*    </label>*/}
                    {/*    <label>*/}
                    {/*        <textarea placeholder="Leave your feedback"></textarea>*/}
                    {/*    </label>*/}
                    {/*    <button type="submit">Publish</button>*/}
                    {/*</form>*/}
                </div>
            </main>
        </div>
    );
};

export default RecipeDetail;

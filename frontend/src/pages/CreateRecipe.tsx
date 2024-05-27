import React, {useState} from 'react';
import Header from '../components/Header';
import InfoChip from '../components/InfoChip';
import '../styles/CreateRecipe.css';
import defaultServerConfig from "../common/server-info.ts";
import Recipe from "../types/Recipe.ts";

const CreateRecipe: React.FC = () => {
    const {apiUrl} = defaultServerConfig;
    const {key} = JSON.parse(localStorage.getItem('sessionInfo'));

    const [recipeName, setRecipeName] = useState('');
    const [description, setDescription] = useState('');
    const [prepTime, setPrepTime] = useState('');
    const [cookTime, setCookTime] = useState('');
    const [servings, setServings] = useState('-');
    const [ingredients, setIngredients] = useState<string[]>([]);
    const [directions, setDirections] = useState<string[]>([]);
    const [ingredient, setIngredient] = useState('');
    const [direction, setDirection] = useState('');
    const [image, setImage] = useState<File | null>(null);

    const handleAddIngredient = () => {
        if (ingredient) {
            setIngredients([...ingredients, ingredient]);
            setIngredient('');
        }
    };

    const handleAddDirection = () => {
        if (direction) {
            setDirections([...directions, direction]);
            setDirection('');
        }
    };

    const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            setImage(e.target.files[0]);
        }
    };

    const uploadImage = async (image: File) => {
        const formData = new FormData();
        formData.append('file', image);

        const response = await fetch(`${apiUrl}/api/images`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${key}`,
            },
            body: formData,
        });

        if (!response.ok) {
            console.error(response);
            throw new Error('Failed to upload image');
        }

        const data = await response.json();
        return data.imageId;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        const imageId = await uploadImage(image);

        const formData: Recipe = {
            title: recipeName,
            shortDescription: description,
            ingredientsList: ingredients.join(', '),
            cookingSteps: directions.join(', '),
            prepTime: parseInt(prepTime),
            cookTime: parseInt(cookTime),
            imageId: imageId, // Include the uploaded image URL
        };

        try {
            console.debug(formData);
            console.debug(JSON.stringify(formData));
            const response = await fetch(`${apiUrl}/api/recipes`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${key}`,
                },
                body: JSON.stringify(formData),
            });

            if (!response.ok) {
                console.error(response);
                console.error(await response.json());
                throw new Error('Network response was not ok');
            }

            console.log('Recipe submitted successfully:', response.data);
        } catch (error) {
            console.error('Error submitting recipe:', error);
        }
    };

    return (
        <div>
            <Header/>
            <main className="create-recipe">
                <form onSubmit={handleSubmit}>
                    <label>
                        <input
                            type="text"
                            value={recipeName}
                            onChange={(e) => setRecipeName(e.target.value)}
                            placeholder="Type in recipe name..."
                            className="large-input"
                        />
                    </label>
                    <label>
                        <textarea
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            placeholder="Add recipe description..."
                            className="large-input"
                        ></textarea>
                    </label>
                    <div className="image-upload">
                        <label htmlFor="imageUpload" className="image-label">
                            {image ? (
                                <img
                                    src={URL.createObjectURL(image)}
                                    alt="Uploaded"
                                    className="uploaded-image"
                                />
                            ) : (
                                <div className="placeholder-image">
                                    <p>Include an image</p>
                                </div>
                            )}
                        </label>
                        <input
                            type="file"
                            id="imageUpload"
                            className="image-input"
                            onChange={handleImageChange}
                        />
                    </div>
                    <div className="recipe-info">
                        <InfoChip
                            title="Prep time"
                            value={prepTime}
                            postfix="min."
                            editable={true}
                            onChange={(value) => {
                                setPrepTime(value);
                            }}
                        />
                        <InfoChip
                            title="Cook time"
                            value={cookTime}
                            postfix="min."
                            editable={true}
                            onChange={(value) => {
                                setCookTime(value);
                            }}
                        />
                        <InfoChip
                            title="Total time"
                            value={`${parseInt(prepTime) + parseInt(cookTime)}`}
                            postfix="min."
                        />
                        <InfoChip
                            title="Servings"
                            value={servings}
                            postfix=""
                            editable={true}
                            onChange={(value) => {
                                setServings(value);
                            }}
                        />
                    </div>
                    <h3>Ingredients:</h3>
                    <div className="ingredient-inputs">
                        <label>
                            List ingredient measurement:
                            <input
                                type="text"
                                value={ingredient}
                                onChange={(e) => setIngredient(e.target.value)}
                                placeholder="Measurement"
                            />
                        </label>
                        <button type="button" onClick={handleAddIngredient}>
                            Add ingredient
                        </button>
                    </div>
                    <ul>
                        {ingredients.map((ing, index) => (
                            <li key={index}>{ing}</li>
                        ))}
                    </ul>
                    <h3>Directions:</h3>
                    <div className="direction-inputs">
                        <textarea
                            value={direction}
                            onChange={(e) => setDirection(e.target.value)}
                            placeholder="Add the directions of the recipe..."
                        ></textarea>
                        <button type="button" onClick={handleAddDirection}>
                            Add direction
                        </button>
                    </div>
                    <ol>
                        {directions.map((dir, index) => (
                            <li key={index}>{dir}</li>
                        ))}
                    </ol>
                    <button type="submit" className="publish-button">
                        Publish
                    </button>
                </form>
            </main>
        </div>
    );
};

export default CreateRecipe;

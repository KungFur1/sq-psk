import React, {useEffect, useRef, useState} from 'react';
import Header from '../components/Header';
import InfoChip from '../components/InfoChip';
import defaultServerConfig from "../common/server-info.ts";
import Recipe from "../types/Recipe.ts";
import '../styles/CreateRecipe.css';
import '../styles/TextArea.css';
import {useNavigate} from "react-router-dom";

const CreateRecipe: React.FC = () => {
    const {apiUrl} = defaultServerConfig;
    const {key} = JSON.parse(localStorage.getItem('sessionInfo'));

    const navigate = useNavigate();

    const [recipeName, setRecipeName] = useState('');
    const [description, setDescription] = useState('');
    const [prepTime, setPrepTime] = useState('X');
    const [cookTime, setCookTime] = useState('X');
    const [servings, setServings] = useState('-');
    const [ingredients, setIngredients] = useState<string[]>([]);
    const [steps, setSteps] = useState<string[]>([]);
    const [ingredient, setIngredient] = useState('');
    const [step, setStep] = useState('');
    const [image, setImage] = useState<File | null>(null);

    const addFakeData = () => {
        setRecipeName('Vegan Pancakes');
        setDescription('The BEST and fluffiest vegan pancakes made with just 6 easy pantry staples you probably have on hand right now! Made in 1 bowl, the recipe can easily doubled, tripled or even quadrupled.');

        setIngredients([
            'Flour – I used regular all purpose flour here for the fluffiest texture，but feel free to substitute whole wheat pastry flour，spelt flour or a gluten free flour mix.',
            'Baking powder – What makes the pancakes so fluffy!',
            'Salt',
            'Sugar – Just a few tablespoons，but you could substitute pure maple syrup，agave or coconut sugar if desired.',
            'Soy milk – Any other non-dairy milk will work here，such as almond，coconut，hemp or oat milk.',
            'Water',
            'Oil – Only 2 tablespoons. You can use canola oil，melted coconut oil or melted vegan butter. Avocado oil or even olive oil would work as well. If you don’t use oil，simply omit and add more non-dairy milk.'
        ]);

        setSteps([
            'First， whisk together the dry ingredients in a large bowl – flour， baking powder， salt and sugar.',
            'Pour in the non-dairy milk， water and oil.',
            'Stir with a large spoon until just combined. A few lumps are okay; mix them too much and the pancakes will be dense.',
            'Heat a large griddle or pan over medium-high heat. Grease the pan with a little vegan butter or coconut oil and drop about 1/3 cup pancake batter onto the pan. Cook until bubbles form， flip and cook for another minute.',
        ]);

        setPrepTime('1');
        setCookTime('2');
        setServings('4');
    };

    const textareaRef: React.MutableRefObject<HTMLTextAreaElement | null> = useRef(null);

    useEffect(() => {
        if (textareaRef.current) {
            textareaRef.current.style.height = 'auto';
            textareaRef.current.style.height = `${textareaRef.current.scrollHeight}px`;
        }
    }, [description]);

    const handleAddIngredient = () => {
        if (ingredient) {
            setIngredients([...ingredients, ingredient.replace(',', '，')]);
            setIngredient('');
        }
    };

    const handleAddStep = () => {
        if (step) {
            setSteps([...steps, step.replace(',', '，')]);
            setStep('');
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
            return '00000000-0000-0000-0000-000000000000';
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
            cookingSteps: steps.join(', '),
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
                alert('Failed to create recipe');
            }

            navigate(`/recipe/${(await response.json()).id}`);

            console.log('Recipe submitted successfully:', response.json());
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
                            ref={textareaRef}
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            placeholder="Add recipe description..."
                            className="large-input growable"
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

                    <div className="step-ingredient-container">
                        <div className='ingredients-container'>
                            <h3>Ingredients:</h3>
                            <div className="ingredient-inputs">
                                <label>
                                    List ingredients:
                                    <input
                                        type="text"
                                        value={ingredient}
                                        onChange={(e) => setIngredient(e.target.value)}
                                        placeholder="An ingredient"
                                    />
                                </label>
                                <button type="button" onClick={handleAddIngredient}>
                                    +
                                </button>
                            </div>
                            <ul>
                                {ingredients.map((ing, index) => (
                                    <li key={index}>{ing}</li>
                                ))}
                            </ul>
                        </div>

                        <div className='steps-container'>
                            <h3>Directions:</h3>
                            <div className="ingredient-inputs">
                                <label>
                                    Add a step to the recipe...
                                    <input
                                        type="text"
                                        value={step}
                                        onChange={(e) => setStep(e.target.value)}
                                        placeholder="A Step"
                                    />
                                </label>
                                <button type="button" onClick={handleAddStep}>
                                    +
                                </button>
                            </div>
                            <ol>
                                {steps.map((dir, index) => (
                                    <li key={index}>{dir}</li>
                                ))}
                            </ol>
                        </div>
                    </div>

                    <button type="submit" className="publish-button">
                        Publish
                    </button>
                </form>
                <button type='button' onClick={addFakeData}> Add fake data</button>
            </main>
        </div>
    );
};

export default CreateRecipe;

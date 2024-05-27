type SearchRecipe = {
    id: string;
    userId: string, //Guid,
    title: string,
    shortDescription: string,
    ingredientsList: string,
    cookingSteps: string,
    imageId: string, //Guid,
    createdAt: string, //DateTime,
    updatedAt: string, //DateTime,
};

export default SearchRecipe;
type Recipe = {
    id: unknown,
    userId: any,
    title: string,
    shortDescription: string,
    ingredientsList: string, // TODO: List of strings
    cookingSteps: string, // TODO: List of strings
    imageId: string,
    prepTime: number,
    cookTime: number,
    servings: any,
    createdAt: any,
    updatedAt: any,
};

export default Recipe;
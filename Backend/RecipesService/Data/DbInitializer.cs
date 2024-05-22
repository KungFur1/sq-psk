using Microsoft.EntityFrameworkCore;
using RecipesService.Data;
using RecipesService.Models;

namespace RecipesService;

public static class DbInitializer
{
    public static void InitializeDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<RecipesDbContext>());
    }

    private static void SeedData(RecipesDbContext recipesDbContext)
    {
        Console.WriteLine("DbInitializer: applying migrations...");
        recipesDbContext.Database.Migrate();

        if (recipesDbContext.Recipes.Any())
        {
            Console.WriteLine("DbInitializer: data present, no need to seed...");
            return;
        }

        Console.WriteLine("DbInitializer: adding seed data...");
        var recipes = new List<Recipe>()
        {
            new Recipe
            {
                Id = Guid.Parse("cbdc3659-30b6-449b-b998-84f2216c3e86"),
                UserId = Guid.Parse("f1819dad-76e3-41b3-9a76-e7fa6b974cc4"),
                Title = "Summer Peach Salad",
                ShortDescription = "A refreshing salad featuring ripe peaches and a tangy vinaigrette.",
                IngredientsList = "3 ripe peaches, 1 cup arugula, 1/2 cup crumbled feta cheese, 1/4 cup sliced almonds, 2 tablespoons olive oil, 1 tablespoon balsamic vinegar",
                CookingSteps = "Slice peaches and combine with arugula. Top with feta and almonds. Whisk together olive oil and vinegar, drizzle over salad, and serve immediately.",
                ImageUrl = "https://fastly.picsum.photos/id/234/200/200.jpg?hmac=xPu1ioX9SWsHBaICVti0qPl5c7fPKvztdkSxpv8w6oo"
            },

            new Recipe
            {
                Id = Guid.Parse("4a842e83-29b9-4e2a-a8af-f71f7e7ca3eb"),
                UserId = Guid.Parse("c3d1eb34-b848-48ca-b277-ed3bb3e242ae"),
                Title = "Classic Beef Stroganoff",
                ShortDescription = "A rich and creamy dish with tender strips of beef and mushrooms.",
                IngredientsList = "500g beef sirloin, 1 onion, 250g mushrooms, 1 cup beef broth, 1/2 cup sour cream, 2 tablespoons mustard, 300g egg noodles",
                CookingSteps = "Brown beef strips and set aside. Sauté onion and mushrooms. Return beef to pan, add broth, and simmer. Stir in sour cream and mustard, serve over cooked noodles.",
                ImageUrl = "https://fastly.picsum.photos/id/889/200/200.jpg?hmac=mzo0mKfXHC9qb2dLw47jTrXZmlF9g6c6EuUFOWz0T5U"
            },

            new Recipe
            {
                Id = Guid.Parse("6a5d5afe-adca-41b8-b18e-80dd5c9c7b86"),
                UserId = Guid.Parse("cb8c4501-be84-4377-98a9-f40de26f1f84"),
                Title = "Vegan Chocolate Cake",
                ShortDescription = "A moist and decadent cake that's completely dairy-free and egg-free.",
                IngredientsList = "1 cup almond milk, 1 teaspoon apple cider vinegar, 3/4 cup granulated sugar, 1/3 cup vegetable oil, 1 teaspoon vanilla extract, 1 cup all-purpose flour, 1/3 cup cocoa powder, 1/2 teaspoon baking soda, 1/4 teaspoon salt",
                CookingSteps = "Mix almond milk and vinegar. Add sugar, oil, vanilla. Combine flour, cocoa, baking soda, salt, mix into wet ingredients. Bake at 350°F for 25 minutes.",
                ImageUrl = "https://fastly.picsum.photos/id/61/200/200.jpg?hmac=RL-JLeHTLsWK7354qjYNQ1iuDxeoCv-kYRTUYun2h34"
            },

            new Recipe
            {
                Id = Guid.Parse("29f81774-ad49-4e3c-848d-463ab14e35e9"),
                UserId = Guid.Parse("9e6e4431-aa91-470d-9a82-2397658508cd"),
                Title = "Spicy Sichuan Noodles",
                ShortDescription = "A fiery dish of noodles tossed with a spicy chili oil sauce and Sichuan peppercorns.",
                IngredientsList = "200g noodles, 2 tablespoons sesame oil, 3 garlic cloves, 1 tablespoon grated ginger, 2 tablespoons soy sauce, 1 tablespoon chili oil, 1 teaspoon Sichuan peppercorns",
                CookingSteps = "Cook noodles. Sauté garlic and ginger in sesame oil. Add soy sauce, chili oil, and peppercorns. Toss noodles in the sauce and serve.",
                ImageUrl = "https://fastly.picsum.photos/id/268/200/200.jpg?hmac=I5JAWzISJc5x0jlDhEngvCIwyM0zxRh22iIIzHqOT80"
            },

            new Recipe
            {
                Id = Guid.Parse("56a47da4-6952-4581-901b-c0340a130843"),
                UserId = Guid.Parse("3840e2da-730e-43d1-897a-4e4f4fa4c003"),
                Title = "Mediterranean Quinoa Salad",
                ShortDescription = "A light and healthy salad packed with protein and fresh vegetables.",
                IngredientsList = "1 cup quinoa, 1 cucumber, 2 tomatoes, 1/2 red onion, 1/4 cup olives, 1/4 cup feta cheese, 2 tablespoons lemon juice, 3 tablespoons olive oil",
                CookingSteps = "Cook quinoa. Chop vegetables and combine with quinoa. Whisk together lemon juice and olive oil, pour over salad. Top with feta and olives.",
                ImageUrl = "https://fastly.picsum.photos/id/254/200/200.jpg?hmac=wM9u9N0tgdWKFIr8MxBLr8rLoV0JjUUKLk32XFV8agQ"
            },

            new Recipe
            {
                Id = Guid.Parse("2f5339b9-f543-4e76-98f1-2701b193fedc"),
                UserId = Guid.Parse("6810c260-4849-4256-af7c-434d9c894b15"),
                Title = "Mango Avocado Salsa",
                ShortDescription = "A zesty and fresh salsa with chunks of mango and avocado.",
                IngredientsList = "2 ripe mangoes, 1 ripe avocado, 1/4 cup finely chopped red onion, 1/2 cup chopped cilantro, Juice of 1 lime, Salt to taste",
                CookingSteps = "Dice mangoes and avocado, and combine with onion, cilantro, and lime juice. Season with salt and mix well.",
                ImageUrl = "https://fastly.picsum.photos/id/132/200/200.jpg?hmac=meVrCoOURNB7iKK3Mv-yuRrvxvXgv4h2vIRLM4sKwK4"
            },

            new Recipe
            {
                Id = Guid.Parse("8f591bf4-0259-4037-a6d8-1e345f36edd4"),
                UserId = Guid.Parse("aa3648f3-fa81-44fa-b153-7c61102d2286"),
                Title = "Spicy Grilled Chicken",
                ShortDescription = "Juicy chicken thighs marinated in a spicy sauce and grilled to perfection.",
                IngredientsList = "8 chicken thighs, 3 tablespoons hot sauce, 2 tablespoons olive oil, 1 tablespoon paprika, Salt and pepper",
                CookingSteps = "Marinate chicken thighs in hot sauce, olive oil, paprika, salt, and pepper. Grill until fully cooked.",
                ImageUrl = "https://fastly.picsum.photos/id/625/200/200.jpg?hmac=oIwf4IzbglfXYZo-9VXZTHju2-ox3D-Vooeuioav_nw"
            },

            new Recipe
            {
                Id = Guid.Parse("90f04a00-0e18-419c-8eb6-a9d47c0d65f0"),
                UserId = Guid.Parse("f3466fdc-f38b-4ff7-87f9-85ae3c34e08f"),
                Title = "Vegetarian Chili",
                ShortDescription = "A hearty chili made with beans, tomatoes, and a blend of spices.",
                IngredientsList = "1 can black beans, 1 can kidney beans, 1 large can diced tomatoes, 1 onion, 2 cloves garlic, 1 tablespoon chili powder",
                CookingSteps = "Sauté onion and garlic, add beans and tomatoes. Season with chili powder and simmer.",
                ImageUrl = "https://fastly.picsum.photos/id/264/200/200.jpg?hmac=O4sRY3iZeFvmPRuanICCCZi-CDz0HdRHMsHttvNCgmw"
            },

            new Recipe
            {
                Id = Guid.Parse("f2672eee-4c63-4cbe-8943-709ca9dbb56b"),
                UserId = Guid.Parse("1444fe2a-6884-4011-8f37-c585cff34b0d"),
                Title = "Chocolate Banana Smoothie",
                ShortDescription = "A smooth and creamy smoothie with ripe bananas and rich chocolate.",
                IngredientsList = "2 ripe bananas, 1 cup milk, 2 tablespoons cocoa powder, 1 tablespoon honey",
                CookingSteps = "Blend all ingredients until smooth.",
                ImageUrl = "https://fastly.picsum.photos/id/594/200/200.jpg?hmac=owYWnk-K1Yd10c-pUQt8S_02BsTYKsMvSxMWNexClq8"
            },

            new Recipe
            {
                Id = Guid.Parse("fa41dd48-b355-49e8-8bd8-767594fba3cb"),
                UserId = Guid.Parse("f25dfd3e-77b5-488f-b11c-c5e8c950a231"),
                Title = "Simple Greek Salad",
                ShortDescription = "A traditional Greek salad with fresh vegetables and feta cheese.",
                IngredientsList = "3 tomatoes, 1 cucumber, 1/2 red onion, 1/4 cup olives, 100g feta cheese, 2 tablespoons olive oil",
                CookingSteps = "Chop vegetables, combine with olives and crumbled feta. Drizzle with olive oil.",
                ImageUrl = "https://fastly.picsum.photos/id/809/200/200.jpg?hmac=2U0kkZGtbw4L4bQc3aC8cZA6ywfn2MvR0d-YC4ITcI8"
            },

            new Recipe
            {
                Id = Guid.Parse("f01b6bce-77ef-441b-bdb6-630c00c0b7f7"),
                UserId = Guid.Parse("5f111d5b-7716-45eb-ae50-1774992c61f8"),
                Title = "Spring Vegetable Stir Fry",
                ShortDescription = "A vibrant dish of fresh spring vegetables lightly sautéed with soy sauce and garlic.",
                IngredientsList = "2 cups mixed spring vegetables, 2 tablespoons soy sauce, 1 tablespoon minced garlic, 1 teaspoon sesame oil",
                CookingSteps = "Stir fry vegetables in oil with garlic until crisp-tender. Drizzle with soy sauce before serving.",
                ImageUrl = "https://fastly.picsum.photos/id/860/200/200.jpg?hmac=xEnSgZhxWVFOWiVCBQzYpKUH7S5nFb7-QTZ8Hfqwq4M"
            },

            new Recipe
            {
                Id = Guid.Parse("7cd1afb3-24a7-4a80-b568-48d47cae207d"),
                UserId = Guid.Parse("d3cdb1f7-8d63-490c-b741-cf618e5b09f0"),
                Title = "Hearty Chicken Soup",
                ShortDescription = "A comforting soup with chicken, vegetables, and noodles, perfect for a chilly day.",
                IngredientsList = "2 chicken breasts, 3 cups chicken broth, 1 cup diced carrots, 1 cup noodles, 1 teaspoon thyme",
                CookingSteps = "Boil chicken until cooked, shred. Simmer broth with vegetables and noodles until tender. Stir in chicken and thyme.",
                ImageUrl = "https://fastly.picsum.photos/id/996/200/200.jpg?hmac=nRtkfqKyD3p2uHiFO5LmGt31UcH3NKWg80H5yUkZ8_k"
            },

            new Recipe
            {
                Id = Guid.Parse("c42438cd-2726-4867-8a44-090fb8bf2358"),
                UserId = Guid.Parse("9a3fe071-7c69-41b3-8bcf-9fdc5c435f22"),
                Title = "Spiced Lamb Kebabs",
                ShortDescription = "Juicy lamb kebabs marinated in a spicy mixture and grilled to perfection.",
                IngredientsList = "500g diced lamb, 2 tablespoons curry paste, 1 onion, 1 green bell pepper, 1 cup yogurt",
                CookingSteps = "Marinate lamb in spices and yogurt overnight. Thread onto skewers with vegetables and grill.",
                ImageUrl = "https://fastly.picsum.photos/id/527/200/200.jpg?hmac=pt4SE0tD3d9wOZOKl-3uFHKRXPwF77K_UHZATkDnP5k"
            },

            new Recipe
            {
                Id = Guid.Parse("a0d7c72b-0d91-441b-b42b-d7a56acd59bd"),
                UserId = Guid.Parse("2950af60-d8a0-4bc2-b0a2-fc8df482e9ee"),
                Title = "Vegetarian Chili",
                ShortDescription = "A rich and hearty chili made with beans, tomatoes, and an assortment of vegetables.",
                IngredientsList = "1 can black beans, 1 can diced tomatoes, 1 cup corn, 1 onion, 2 cloves garlic, 1 tablespoon chili powder",
                CookingSteps = "Sauté onion and garlic, add beans, tomatoes, and corn. Simmer with chili powder for 30 minutes.",
                ImageUrl = "https://fastly.picsum.photos/id/838/200/200.jpg?hmac=a2ZUJPqhEFH-OzhHFaKdtDdV2XnIE7t1tP2iXnP5Fj0"
            },

            new Recipe
            {
                Id = Guid.Parse("319cc08e-a91c-40c3-88cf-7a3c3230c5e0"),
                UserId = Guid.Parse("360b9e1b-429e-4e1d-b914-67543ab71d76"),
                Title = "Maple-Glazed Salmon",
                ShortDescription = "Delicious salmon fillets with a sweet maple syrup glaze, broiled to a golden finish.",
                IngredientsList = "2 salmon fillets, 1/4 cup maple syrup, 1 tablespoon soy sauce, 1 teaspoon garlic powder",
                CookingSteps = "Mix maple syrup, soy sauce, and garlic powder. Brush on salmon and broil until cooked through.",
                ImageUrl = "https://fastly.picsum.photos/id/565/200/200.jpg?hmac=QvKo8qgzFFNcZoXCpT0CNMDTwWd3ynwqLXxrzK2o8fw"
            }
        };

        recipesDbContext.AddRange(recipes);
        recipesDbContext.SaveChanges();
        Console.WriteLine("DbInitializer: seed data added");
    }
}

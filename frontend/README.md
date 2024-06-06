# Slay Queen Frontend (sqf)

## TODO

- [X] Redirect to my recipes after successful creation
- [X] Grow textarea
- [+-] Align create recipe with wireframe
- [X] Fix inputs in InfoChips
- [ ] Show errors after unsuccessful:
  - [ ] log-in/registration
  - [ ] recipe creation
- [X] Search button
- [X] My recipes results
- [ ] Prepare test data for demo

## DEMO

* All recipes + search
* RecipeDetails
* Registration / log-in
* Recipe creation

## Prerequisites

> [!IMPORTANT]
> 
> These instructions might be a inaccurate,
> or missing some dependencies.

You'll need to install: `pnpm`, maybe/probably `npm` & `node` (for MacOS: `brew install npm node pnpm`).

## React + TypeScript + Vite

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react/README.md) uses [Babel](https://babeljs.io/) for Fast Refresh
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react-swc) uses [SWC](https://swc.rs/) for Fast Refresh

### Expanding the ESLint configuration

If you are developing a production application, we recommend updating the configuration to enable type aware lint rules:

- Configure the top-level `parserOptions` property like this:

```js
export default {
  // other rules...
  parserOptions: {
    ecmaVersion: 'latest',
    sourceType: 'module',
    project: ['./tsconfig.json', './tsconfig.node.json'],
    tsconfigRootDir: __dirname,
  },
}
```

- Replace `plugin:@typescript-eslint/recommended` to `plugin:@typescript-eslint/recommended-type-checked` or `plugin:@typescript-eslint/strict-type-checked`
- Optionally add `plugin:@typescript-eslint/stylistic-type-checked`
- Install [eslint-plugin-react](https://github.com/jsx-eslint/eslint-plugin-react) and add `plugin:react/recommended` & `plugin:react/jsx-runtime` to the `extends` list

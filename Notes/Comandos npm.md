# Comandos de npm

## Prettier

### Instalar

```bash
npm install --save-dev --save-exact prettier
```

### Configurar

```bash
echo {}> .prettierrc.json
```

## Tailwind CSS

### instalar

Install `tailwindcss` via npm, and create your `tailwind.config.js` file.

```bash
npm install -D tailwindcss
npx tailwindcss init
```

### configurar los templates

Add the paths to all of your template files in your `tailwind.config.js` file.

```js
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./**/*.{razor,html}'],
  theme: {
    extend: {},
  },
  plugins: [],
};
```

## Agregar la carpeta styles

```bash
mkdir styles
```

### archivo de app.css dentro de styles

```bash
touch styles/app.css
```

### añadir las directivas de tailwind al CSS

Add the `@tailwind` directives for each of Tailwind’s layers to your main CSS file `/styles/app.css`.

```js
@tailwind base;
@tailwind components;
@tailwind utilities;
```

### Agregar al archivo `package.json` los scripts para construir el CSS

Run the CLI tool to scan your template files for classes and build your CSS.

```json
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1",
    "dev": "tailwindcss -i ./styles/app.css -o ./wwwroot/css/app.css --watch"
  },
```

Run the command `npm run dev` to generate your CSS file. It will be created in the `./dist` folder.

```bash
npm run dev
```

### Empezar a usar Tailwind CSS

Add your compiled CSS file to the `<head>` and start using Tailwind’s utility classes to style your content.

## Tailwind Prettier

### Install

To get started, just install `prettier-plugin-tailwindcss` as a dev-dependency:

```bash
npm install -D prettier prettier-plugin-tailwindcss
```

## Standard

### Instalacion

The easiest way to use JavaScript Standard Style is to install it globally as a Node command line program. Run the following command in Terminal

```bash
npm install standard --global
```

### Client app infrastructure
- Shared across the app CSS classes should be contained inside `index.scss`
- Assets like images, svgs etc should be contained inside `assets` folder
- CSS reset should be contained inside `App.scss`
- Variable based SCSS should be contained inside `styles` folder
- New pages should have new folders inside `pages` folder
- Layouts should be contained inside `core` folder
- Contexts should be contained inside `contexts` 
- Shared components should be contained inside `components folder` and should get own folder
- API related behaviours should be contained inside `api` folder
- Keep new colors inside the `theme.scss` 

### Common CSS rules
- For inputs use `mts-input` class
- For buttons use `mts-button` class 
- Only use colors contained inside `theme.scss`
- Try to use sizes from variables if possible

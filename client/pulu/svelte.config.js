import adapter from '@sveltejs/adapter-auto';
import { vitePreprocess } from '@sveltejs/kit/vite';


/** @type {import('@sveltejs/kit').Config} */
const config = {
	// Consult https://kit.svelte.dev/docs/integrations#preprocessors
	// for more information about preprocessors
	preprocess: vitePreprocess(),

	kit: {
		// adapter-auto only supports some environments, see https://kit.svelte.dev/docs/adapter-auto for a list.
		// If your environment is not supported or you settled on a specific environment, switch out the adapter.
		// See https://kit.svelte.dev/docs/adapters for more information about adapters.
		adapter: adapter({
			optimizeImages: true,
			images: {
				formats: ['jpeg', 'png', 'webp'],
			  },
		})
	},
	// This will tell SvelteKit to look for the /static/public directory relative to the /src directory, 
	// which may be more consistent with your GitHub Actions workflow.
    srcDir: "src" 
};

export default config;

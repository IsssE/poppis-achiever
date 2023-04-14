<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import { loginUser } from './auth';
	import store from '$lib/userStore';

	const dispatch = createEventDispatcher();
	let error = false;

	export const handleLogin = async (event: SubmitEvent) => {
		const formData = new FormData(event.target as HTMLFormElement);
		const username = formData.get('username') as string;
		const password = formData.get('password') as string;
		loginUser(username, password)
			.then((res) => {
				error = false;
				store.setUser({ id: username, displayName: res.getUser.displayName });
				dispatch('closeModal');
			})
			.catch(() => {
				error = true;
			});
	};
</script>

<form on:submit|preventDefault={handleLogin} class="flex flex-col w-64">
	{#if error}
	<div class=" text-red-700 text-center">Problem in login</div>
		
	{/if}
	
	<div class="flex flex-col pt-5">
		<div>Username</div>
		<input
			class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
			name="username"
			type="text"
		/>
	</div>

	<div class="flex flex-col py-5">
		<div>Password</div>
		<input
			class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
			name="password"
			type="password"
		/>
	</div>
	<button class="bg-blue-400 hover:bg-blue-500 font-bold py-2 px-4 rounded">login</button>
</form>

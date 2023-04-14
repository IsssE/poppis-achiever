<script lang="ts">
	import { createEventDispatcher } from 'svelte';
	import { getLoginUser, registerUser } from './auth';

	const dispatch = createEventDispatcher();

	export const handleRegistration = async (event: SubmitEvent) => {
		const formData = new FormData(event.target as HTMLFormElement);
		const username = formData.get('username') as string;
		const password = formData.get('password') as string;
		const displayName = formData.get('displayName') as string;

		registerUser(username, password, displayName).then((res) => {
			getLoginUser(res.registerUser.userId, password);
			dispatch('closeModal');
		}).catch(() => {
			alert("something went wrong")
		});
	};
</script>

<form on:submit|preventDefault={handleRegistration} class="flex flex-col w-64">
	<div class="flex flex-col py-2 ">
		<div>Username</div>
		<input
			required
			class=" bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
			name="username"
			type="text"
		/>
	</div>

	<div class="flex flex-col py-2 ">
		<div>Display Name</div>
		<input
			required
			class=" bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
			name="displayName"
			type="text"
		/>
	</div>

	<div class="flex flex-col py-2 ">
		<div>Password</div>
		<input
			required
			class=" bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
			name="password"
			type="password"
		/>
	</div>
	<!-- disable button while  waiting for confirmation disabled={loading} -->
	<button class="mx-5 mt-5 bg-blue-400 hover:bg-blue-500 font-bold py-2 px-4 rounded" type="submit"
		>Register</button
	>
</form>

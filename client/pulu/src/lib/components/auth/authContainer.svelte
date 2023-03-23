<script lang="ts">
	import Modal from '../util/modal.svelte';
	import Login from './login.svelte';
	import Register from './register.svelte';

	export let status: 'Login' | 'Register' = 'Login';
	export let showModal = false;
	let showRegister = false;

	const handleRegister = () => {
		showRegister = !showRegister;
		status = showRegister ? 'Register' : 'Login';
	};
</script>

<Modal bind:showModal={showModal}>
	<div class="text-center w-100" slot="header">
		<span class="font-bold">{status}</span>
	</div>
	{#if !showRegister}
		<Login on:closeModal />
		<div class="flex flex-row-reverse">
			<button class="flex-row-reverse p-5 text-blue-700" on:click={handleRegister}>Register</button>
		</div>
	{:else}
		<Register on:closeModal />
	{/if}
</Modal>

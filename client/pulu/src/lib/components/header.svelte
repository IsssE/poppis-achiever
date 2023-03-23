<script lang="ts">
	import userStore from '$lib/userStore';
	import AuthContainer from './auth/authContainer.svelte';
	let showModal = false;
	let userName: string | undefined = undefined;

	userStore.subscribe(user => {
		userName = user.currentUser.displayName
	})

	const closeLogin = () => {
		showModal = false;
	};
</script>

<div class="flex bg-gray-400 h-10">
	<h1 class="flex-auto text-3xl font-bold text-center ml-20">Pulu</h1>
	{#if userName}
	<div class= "flex-none py-2 px-5 w-15 ">{userName}</div>
	{:else}
	<button on:click={() => (showModal = true)} type="button" class="flex-none px-5 w-15 ">
		Login</button
		>
	{/if}

</div>

{#if showModal}
	<AuthContainer bind:showModal on:closeModal={closeLogin} />
{/if}

<script lang="ts">
	import Karttu from '$lib/images/pyokkikarttu.png';
	import store from '$lib/userStore';
	import type { ICurrentUser } from '$lib/types';
	import { onMount } from 'svelte';
	import { getToken } from '$lib/components/auth/auth';

	let loggedUser: ICurrentUser;

	store.subscribe((update) => {
		loggedUser = update.currentUser;
	});
	onMount(async () => {
		getToken();
	});
</script>

<div class="text-center font-bold">THE PULU CONTENT</div>

{#if loggedUser.displayName}
	<div class="text-center">
		Sori {loggedUser.displayName}, en oo vielä kerenny tekee logouttia :D
	</div>
	<img src={Karttu} alt="karttu" class="w-2/4 h-2/4 mx-auto mt-24" />
{/if}

<style lang="scss">
	img {
		animation: rotation 6s infinite linear;
	}

	@keyframes rotation {
		from {
			transform: rotate(0deg);
		}
		to {
			transform: rotate(359deg);
		}
	}
</style>

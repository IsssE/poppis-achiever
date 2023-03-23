import { writable } from 'svelte/store';
import type { ICurrentUser } from './types';

export interface IStore {
	currentUser: ICurrentUser;
}

const createUserStore = () => {
    const init: IStore = {
        currentUser: {}
    }

    const {subscribe, set, update } = writable(init)

	return {
        subscribe,
        set,
		setUser: (newUser: ICurrentUser) => update((state) =>{ 
            return {...state, currentUser: newUser};
        }),
	};
};
        // (currentUser: ICurrentUser, ...rest) => ({
        //     ...rest,
        //     newUser
        // })

export default createUserStore();

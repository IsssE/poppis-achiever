import clientGQL from '$lib/api';
import store from '$lib/userStore';
import { gql } from 'graphql-request';
import { getUserData } from '../user/util';

export const registerUser = (
	user: string,
	password: string,
	displayName: string
): Promise<{ registerUser: { userId: string } }> => {
	const mutation = gql`
		mutation ($id: String!, $password: String!, $displayName: String!) {
			registerUser(userName: $id, password: $password, displayName: $displayName) {
				userId
			}
		}
	`;
	const variables = { id: user, password: password, displayName: displayName };

	return clientGQL.request(mutation, variables);
};

export const getLoginUser = (user: string, password: string) => {
	const query = gql`
		query ($id: String!, $password: String!) {
			getToken(userName: $id, password: $password)
			getUser(id: $id) {
				displayName
			}
		}
	`;
	const variables = { id: user, password: password };

	clientGQL.request<{ getToken: string, getUser: {displayName: string} }>(query, variables).then((res) => {
		store.setUser({ id: user, displayName: res.getUser.displayName });
	});
};

export const getToken = () => {
	const query = gql`
		query {
			getIdForToken
		}
	`;

	clientGQL.request<{ getIdForToken: string }>(query).then((res) => {
		getUserData(res.getIdForToken).then((userData) => {
			store.setUser({
				id: userData.getUser.userId,
				displayName: userData.getUser.displayName
			});
		});
	});
};

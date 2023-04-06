import clientGQL from '$lib/api';
import { gql } from 'graphql-request';

export interface IUserData {
	userId: string;
	displayName: string;
}

export const getUserData = (id: string): Promise<{ getUser: IUserData }> => {
	const query = gql`
		query ($id: String!) {
			getUser(id: $id) {
				userId
				displayName
			}
		}
	`;
	const variables = { id: id };

	return clientGQL.request(query, variables);
};

// Feel like there should be a better way to do this

import { GraphQLClient } from 'graphql-request';
import { PUBLIC_API_URL } from '$env/static/public';

import Cookies from 'js-cookie';
const jwt = Cookies.get('jwt');
// https://localhost:7029/ when local
// http://localhost:7000/ when container
const clientGQL = new GraphQLClient(`${PUBLIC_API_URL}/graphql`, {
	credentials: 'include',
	mode: 'cors'
}).setHeader('Authorization', jwt ? jwt : '');

export default clientGQL;

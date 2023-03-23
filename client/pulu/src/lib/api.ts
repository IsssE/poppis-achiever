// Feel like there should be a better way to do this

import { GraphQLClient } from 'graphql-request';
import Cookies from 'js-cookie';
const jwt = Cookies.get('jwt');
// https://localhost:7029/ when local
// https://localhost:7000/ when container
const clientGQL = new GraphQLClient('https://localhost:7029/graphql', {
	credentials: 'include',
	mode: 'cors'
}).setHeader('Authorization', jwt ? jwt : '');

export default clientGQL;

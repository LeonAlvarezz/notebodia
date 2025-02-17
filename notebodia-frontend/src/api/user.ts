import type { UserAuthPayload } from '@/types/auth.type'
import { fetchApi } from '@/util/fetch-client'
import type { User } from '../types/user.type'
const prefix = '/users'
export async function login(payload: UserAuthPayload) {
    const data = fetchApi.post<User>(prefix + '/login', payload)
    return data
}
export async function signup(payload: UserAuthPayload) {
    const data = fetchApi.post<User>(prefix + '/signup', payload)
    return data
}
export async function getMe() {
    const data = fetchApi.get<User>(prefix + '/me')
    return data
}

export async function logout() {
    const data = fetchApi.post(prefix + '/signout')
    return data
}

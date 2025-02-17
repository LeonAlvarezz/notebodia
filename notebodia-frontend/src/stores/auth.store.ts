import { getMe } from '@/api/user'
import type { UserAuthPayload } from '@/types/auth.type'
import type { User } from '@/types/user.type'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useAuthStore = defineStore('auth', () => {
    const payload = ref<UserAuthPayload>()
    const user = ref<User | null>(null)
    const isAuthenticated = ref(false)
    function clearPayload() {
        payload.value = undefined
    }
    function setUser(me: User | null) {
        user.value = me
    }
    async function getUser() {
        try {
            const { data, error } = await getMe()
            console.log('data:', data)
            if (error) {
                console.log(error.message)
                return
            }
            user.value = data
            console.log(user.value)
            isAuthenticated.value = true
            return data
        } catch (error) {
            console.log(error)
        }
    }
    async function clearUser() {
        user.value = null
        isAuthenticated.value = false
    }
    return {
        payload,
        clearPayload,
        user,
        setUser,
        getUser,
        isAuthenticated,
        clearUser,
    }
})

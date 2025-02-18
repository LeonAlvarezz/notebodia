import { HOME_PATH, LOGIN_PATH } from '@/constant/base'
import { Login, Home, Signup } from '@/pages/index.ts'
import { useAuthStore } from '@/stores/auth.store'
// src/router.ts
import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
const routes: RouteRecordRaw[] = [
    { path: '/', redirect: '/notes' },
    { path: '/auth/login', component: Login },
    { path: '/auth/signup', component: Signup },
    {
        path: '/notes',
        component: Home,
        meta: {
            requiresAuth: true,
        },
    },
    // { path: '/notes/:id', component: NoteDetail, props: true },
    // { path: '/notes/:id/edit', component: NoteEdit, props: true },
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

router.beforeEach(async (to, _, next) => {
    const authStore = useAuthStore()
    const userInfo = authStore.user || (await authStore.getUser())
    console.log('userInfo:', userInfo)
    if (to.path === LOGIN_PATH && authStore.isAuthenticated) {
        next({
            path: HOME_PATH,
        })
        return
    }
    if (to.matched.some((record) => record.meta.requiresAuth)) {
        if (!authStore.isAuthenticated)
            next({ path: LOGIN_PATH, query: { nextUrl: to.fullPath } })
        else next()
    } else {
        next()
    }
})

export default router

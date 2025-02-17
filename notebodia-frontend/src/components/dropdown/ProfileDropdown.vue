<script setup lang="ts">
import { NDropdown, NAvatar, type MenuOption } from 'naive-ui'
import { IconLogout, IconSelector, IconSettings } from '@tabler/icons-vue'
import { renderIcon } from '@/util/render'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import { onMounted, watch } from 'vue'
import { logout } from '@/api/user'
import { toast } from '@/composables/toast'
import { LOGIN_PATH } from '@/constant/base'
const router = useRouter()
const authStore = useAuthStore()

const options: MenuOption[] = [
    {
        label: 'Settings',
        key: 'setting',
        icon: renderIcon(IconSettings),
    },
    {
        label: 'Log out',
        key: 'logout',
        icon: renderIcon(IconLogout, '!text-red-400'),
        props: {
            class: '!text-red-400',
        },
    },
]
const handleSelect = async (key: string) => {
    if (key === 'logout') {
        try {
            const { error } = await logout()
            if (error) {
                toast(error.message, {
                    type: 'error',
                })
            }
            authStore.clearUser()
            router.push(LOGIN_PATH)
        } catch (error) {
            const message =
                error instanceof Error ? error.message : String(error)
            toast(message, {
                type: 'error',
            })
        }
    }
}
watch(
    () => authStore.user,
    () => {
        console.log('authStore.user:', authStore.user)
    }
)
onMounted(() => {
    console.log('authStore.user:', authStore.user)
})
</script>

<template>
    <NDropdown :options="options" trigger="click" @select="handleSelect">
        <div
            class="hover:bg-muted flex items-center gap-2 overflow-hidden rounded-full p-1 hover:cursor-pointer"
        >
            <NAvatar
                round
                size="medium"
                src="https://07akioni.oss-cn-beijing.aliyuncs.com/07akioni.jpeg"
            />
            <p class="mr-2">{{ authStore.user?.username }}</p>
            <IconSelector size="20" />
            <!-- <p class="text-lg">Ponleu</p> -->
        </div>
    </NDropdown>
</template>

<style scoped></style>

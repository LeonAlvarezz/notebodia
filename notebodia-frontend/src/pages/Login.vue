<template>
    <div class="flex min-h-svh items-center justify-center">
        <NCard title="Notebodia Login" class="max-w-lg min-w-xs">
            <NForm
                ref="formRef"
                style="margin-top: 20px"
                label-placement="left"
                label-width="auto"
                :model="payload"
                :rules="rules"
            >
                <NFormItem path="email" label="Email" label-placement="top">
                    <NInput
                        v-model:value="payload.email"
                        placeholder="Please enter your email"
                    />
                </NFormItem>
                <NFormItem
                    path="username"
                    label="Username"
                    label-placement="top"
                >
                    <NInput
                        v-model:value="payload.username"
                        placeholder="Please enter your username"
                    />
                </NFormItem>
                <NFormItem
                    path="password"
                    label="Password"
                    label-placement="top"
                >
                    <NInput
                        v-model:value="payload.password"
                        type="password"
                        placeholder="Please enter your password"
                    />
                </NFormItem>
                <NButton
                    secondary
                    :disabled="isLoading"
                    :loading="isLoading"
                    @click="submit"
                    class="!bg-primary hover:!bg-primary/80 !mt-4 !w-full"
                >
                    Login
                </NButton>

                <NButton
                    text
                    class="!text-primary/80 !mt-6 !w-full"
                    @click="router.push('/auth/signup')"
                >
                    Create an account
                </NButton>
            </NForm>
        </NCard>
    </div>
</template>

<script setup lang="ts">
import { login } from '@/api/user'
import { toast } from '@/composables/toast'
import { HOME_PATH } from '@/constant/base'
import { useAuthStore } from '@/stores/auth.store'
import type { UserAuthPayload } from '@/types/auth.type'
import { NForm, NFormItem, type FormInst, type FormRules } from 'naive-ui'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
const payloadStore = useAuthStore()
const defaultValue: UserAuthPayload = {
    email: payloadStore.payload?.email || '',
    username: payloadStore.payload?.username || '',
    password: payloadStore.payload?.password || '',
    confirm_password: '',
}
const router = useRouter()
const isLoading = ref(false)
const formRef = ref<FormInst | null>(null)
const payload = ref<UserAuthPayload>({ ...defaultValue })
const rules: FormRules = {
    // email: [
    //     {
    //         required: true,
    //         message: 'Email is required',
    //         trigger: 'blur',
    //     },
    //     {
    //         type: 'email',
    //         message: 'Invalid Email',
    //         trigger: 'blur',
    //     },
    // ],
    username: {
        required: true,
        message: 'Username is required',
        trigger: 'blur',
    },
    password: {
        required: true,
        message: 'Password is required',
        trigger: 'blur',
    },
}
const submit = () => {
    isLoading.value = true
    formRef.value?.validate(async (errors) => {
        try {
            if (errors) return
            const { error } = await login(payload.value)
            console.log('error:', error)
            if (error) {
                toast(error.message, {
                    type: 'error',
                })
                return
            }
            toast('Success', {
                type: 'success',
            })

            // router.push('/notes')
            payloadStore.clearPayload()
            payload.value = defaultValue
            router.push(HOME_PATH)
        } catch (error) {
            const message =
                error instanceof Error ? error.message : String(error)
            toast(message, {
                type: 'error',
            })
        } finally {
            isLoading.value = false
        }
    })
}
</script>

<style scoped></style>

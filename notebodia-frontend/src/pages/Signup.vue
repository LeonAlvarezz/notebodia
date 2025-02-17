<template>
    <div class="flex min-h-svh items-center justify-center">
        <NCard title="Notebodia Sign Up" class="max-w-lg min-w-xs">
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
                    v
                    path="password"
                    label="Password"
                    label-placement="top"
                >
                    <NInput
                        type="password"
                        v-model:value="payload.password"
                        placeholder="Please enter your password"
                    />
                </NFormItem>
                <NFormItem
                    path="confirm_password"
                    label="Confirm Password"
                    label-placement="top"
                >
                    <NInput
                        type="password"
                        v-model:value="payload.confirm_password"
                        placeholder="Please re-enter your password"
                    />
                </NFormItem>
                <NButton
                    secondary
                    :disabled="isLoading"
                    :loading="isLoading"
                    class="!bg-primary hover:!bg-primary/80 !mt-4 !w-full"
                    @click="submit"
                >
                    Continue
                </NButton>

                <NButton
                    text
                    class="!text-primary/80 !mt-6 !w-full"
                    @click="router.push('/auth/login')"
                >
                    Already Have an Account?
                </NButton>
            </NForm>
        </NCard>
    </div>
</template>

<script setup lang="ts">
import { signup } from '@/api/user'
import { toast } from '@/composables/toast'
import { useAuthStore } from '@/stores/auth.store'
import type { UserAuthPayload } from '@/types/auth.type'
import {
    NForm,
    NFormItem,
    type FormInst,
    type FormItemRule,
    type FormRules,
} from 'naive-ui'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
const defaultValue: UserAuthPayload = {
    email: '',
    username: '',
    password: '',
    confirm_password: '',
}
const router = useRouter()
const isLoading = ref(false)
const formRef = ref<FormInst | null>(null)
const payload = ref<UserAuthPayload>({ ...defaultValue })
const payloadStore = useAuthStore()
function validatePasswordStartWith(_: FormItemRule, value: string): boolean {
    return (
        !!payload.value.password &&
        payload.value.password.startsWith(value) &&
        payload.value.password.length >= value.length
    )
}
const rules: FormRules = {
    email: [
        {
            required: true,
            message: 'Email is required',
            trigger: 'blur',
        },
        {
            type: 'email',
            message: 'Invalid Email',
            trigger: 'blur',
        },
    ],
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
    confirm_password: [
        {
            required: true,
            message: 'Please re-enter your password',
            trigger: 'blur',
        },
        {
            validator: validatePasswordStartWith,
            message: 'Password is not same as re-entered password!',
            trigger: 'input',
        },
    ],
}

const submit = async () => {
    isLoading.value = true
    formRef.value?.validate(async (errors) => {
        try {
            if (errors) return
            const { error } = await signup(payload.value)
            if (error) {
                toast(error.message, {
                    type: 'error',
                })
                return
            }
            payloadStore.payload = payload.value
            //TODO: Replace with actual Signup Logic
            toast('Success', {
                type: 'success',
            })
            setTimeout(() => {
                router.push('/auth/login')
            }, 2000)
            isLoading.value = false
        } catch (error: unknown) {
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
